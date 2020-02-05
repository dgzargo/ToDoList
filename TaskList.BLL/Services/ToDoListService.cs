using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using TaskList.BLL.Enums;
using TaskList.DAL.Entities;
using TaskList.DAL.Repositories;
using static TaskList.BLL.Helpers.CompletedFilterModifierHelper;

namespace TaskList.BLL.Services
{
    public class ToDoListService
    {
        private readonly IRepository<List> _listRepository;
        private readonly IRepository<ToDo> _taskRepository;

        public ToDoListService(IRepository<List> listRepository, IRepository<ToDo> taskRepository)
        {
            _listRepository = listRepository;
            _taskRepository = taskRepository;
        }

        public async Task<List> CreateList(List list)
        {
            await _listRepository.Create(list);
            return list;
        }

        public async Task UpdateList(List listToUpdate)
        {
            await _listRepository.Update(listToUpdate);
        }

        public async Task DeleteList(int id)
        {
            await _listRepository.RemoveById(id);
        }

        public async Task<List> GetListWithGoalsWithin(int id, bool uncompletedOnly = false)
        {
            var list = await _listRepository.GetById(id);
            var filter = ModifyFilterIfCompletedDisallowed(todo => todo.ListId == id, uncompletedOnly);
            list.Tasks = await _taskRepository.GetMultiple(filter);
            return list;
        }

        public Task<List> GetSmartList(SmartLists smartListName, bool uncompletedOnly = false)
        {
            return smartListName switch
            {
                SmartLists.AllTasks => CreateVirtualList("All tasks", todo => true, uncompletedOnly),
                SmartLists.DueDateSet => CreateVirtualList("Due Date Set", todo => todo.Deadline.HasValue,
                    uncompletedOnly),
                SmartLists.ImportantTasks => CreateVirtualList("Important Tasks",
                    todo => todo.Importance == ImportanceEnum.High, uncompletedOnly),
                SmartLists.TasksOfToday => CreateVirtualList("Today’s tasks", todo => todo.Deadline == DateTime.UtcNow,
                    uncompletedOnly),
                _ => throw new NotImplementedException()
            };
        }

        public async Task<List> CreateVirtualList(string title, Expression<Func<ToDo, bool>> filter,
            bool uncompletedOnly = false)
        {
            filter = ModifyFilterIfCompletedDisallowed(filter, uncompletedOnly);
            var list = new List
            {
                Title = title,
                Tasks = await _taskRepository.GetMultiple(filter)
            };
            return list;
        }
    }
}
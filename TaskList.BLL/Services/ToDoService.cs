using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TaskList.DAL.Entities;
using TaskList.DAL.Repositories;

namespace TaskList.BLL.Services
{
    public class ToDoService
    {
        private readonly IRepository<ToDo> _taskRepository;

        public ToDoService(IRepository<ToDo> taskRepository)
        {
            _taskRepository = taskRepository;
        }

        public async Task<ToDo> GetById(int id)
        {
            var found = await _taskRepository.GetById(id);
            return found.IsDeleted ? null : found;
        }

        public async Task<ToDo> DeleteById(int id)
        {
            var toDelete = await _taskRepository.GetById(id);
            if (toDelete.IsCompleted)
            {
                toDelete.IsDeleted = true;
                await _taskRepository.Update(toDelete);
            }
            else
            {
                await _taskRepository.Remove(toDelete);
            }

            return toDelete;
        }

        public async Task DeleteMultiple(IEnumerable<int> ids)
        {
            foreach (var id in ids) await DeleteById(id);
        }

        public Task Create(ToDo toDo)
        {
            toDo.Created = DateTime.UtcNow;
            return _taskRepository.Create(toDo);
        }

        public async Task Update(ToDo toDo)
        {
            toDo.Created = (await _taskRepository.GetById(toDo.Id)).Created;
            await _taskRepository.Update(toDo);
        }

        public Task<List<ToDo>> FindToDoByPartialName(string partialName)
        {
            return _taskRepository.GetMultiple(list => EF.Functions.Like(list.Title, partialName + '%'));
        }
    }
}
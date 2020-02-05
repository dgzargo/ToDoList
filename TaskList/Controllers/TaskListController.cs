using System;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using NSwag.Annotations;
using TaskList.BLL;
using TaskList.BLL.Enums;
using TaskList.BLL.Services;
using TaskList.DAL.Entities;
using TaskList.ViewModels;

namespace TaskList.Controllers
{
    [Route("/taskLists")]
    public class TaskListController: ControllerBase
    {
        private readonly ToDoListService _toDoListService;

        public TaskListController(ToDoListService toDoListService)
        {
            _toDoListService = toDoListService;
        }
        
        [HttpPost("create")]
        public async Task<IActionResult> CreateList([FromBody]List list)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            list = await _toDoListService.CreateList(list);
            return Ok(list);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetList(int id, [FromQuery] SortOrderViewModel sortOrderViewModel)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var list = await _toDoListService.GetListWithGoalsWithin(id);
            list.Sort(sortOrderViewModel);
            return Ok(list);
        }
        
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteList(int id)
        {
            await _toDoListService.DeleteList(id);
            return Ok();
        }
        
        [HttpPut("update")]
        public async Task<IActionResult> UpdateList([FromBody]List list)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            await _toDoListService.UpdateList(list);
            return Ok();
        }

        [HttpGet("smartList/{smartListName}")]
        public async Task<IActionResult> GetAll([FromRoute]SmartLists smartListName, [FromQuery] SortOrderViewModel sortOrderViewModel, [FromQuery] bool uncompletedOnly = false)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var list = await _toDoListService.GetSmartList(smartListName, uncompletedOnly);
            list.Sort(sortOrderViewModel);
            return Ok(list);
        }
    }
}
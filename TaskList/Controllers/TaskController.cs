using System;
using System.Threading.Tasks;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Mvc;
using TaskList.BLL;
using TaskList.BLL.Services;
using TaskList.DAL.Entities;

namespace TaskList.Controllers
{
    [Route("/task")]
    public class TaskController: ControllerBase
    {
        private readonly ToDoService _toDoService;

        public TaskController(ToDoService toDoService)
        {
            _toDoService = toDoService;
        }
        
        [HttpGet("{id}")]
        public async Task<IActionResult> GetTask([FromRoute] int id)
        {
            var goal = await _toDoService.GetById(id);
            return Ok(goal);
        }
        
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTask([FromRoute] int id)
        {
            await _toDoService.DeleteById(id);
            return Ok();
        }
        
        [HttpDelete]
        public async Task<IActionResult> DeleteTasks([FromQuery] int[] ids)
        {
            await _toDoService.DeleteMultiple(ids);
            return Ok();
        }
        
        [HttpPost("create")]
        public async Task<IActionResult> CreateTask([CustomizeValidator(RuleSet = "create")][FromBody] ToDo toDo)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            await _toDoService.Create(toDo);
            return Ok(toDo);
        }
        
        [HttpPut("update")]
        public async Task<IActionResult> UpdateTask([CustomizeValidator(RuleSet = "update")][FromBody] ToDo toDo)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            await _toDoService.Update(toDo);
            return Ok();
        }
        
        [HttpGet("search")]
        public async Task<IActionResult> FindByPartialName([FromQuery(Name = "q")] string partialName)
        {
            var lists = await _toDoService.FindToDoByPartialName(partialName);
            return Ok(lists);
        }
    }
}
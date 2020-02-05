using System;
using FluentValidation;
using TaskList.DAL.Entities;

namespace TaskList.Validators
{
    public class ToDoValidator: AbstractValidator<ToDo>
    {
        public ToDoValidator()
        {
            RuleFor(todo => todo.Title).MaximumLength(30);
            RuleFor(todo => todo.Description).MaximumLength(100);
            RuleFor(todo => todo.Deadline).Must(dt => !dt.HasValue || dt.Value > DateTime.UtcNow);
            RuleFor(todo => todo.Importance).IsInEnum();
            RuleFor(todo => todo.Created).Must(dt => dt == default);
            RuleFor(todo => todo.ListId).Must(id => id > 0);
            
            RuleSet("create", () =>
            {
                RuleFor(todo => todo.Id).Must(id => id == 0);
                RuleFor(todo => todo.IsCompleted).Must(b => b == false);
                RuleFor(todo => todo.IsDeleted).Must(b => b == false);
            });
            
            RuleSet("update", () =>
            {
                RuleFor(todo => todo.Id).Must(id => id > 0);
            });
        }
    }
}
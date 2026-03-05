using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectManager.Application.Features.TaskItems.Commands.UpdateTaskItem
{
    public class UpdateTaskItemCommandValidator : AbstractValidator<UpdateTaskItemCommand>
    {
        public UpdateTaskItemCommandValidator()
        {
            RuleFor(v => v.Id).NotEmpty();

            RuleFor(p => p.Title)
                .NotEmpty().WithMessage("Título es obligatorio.")
                .MaximumLength(250).WithMessage("Título no debe exceder los 250 caracteres.");

            RuleFor(v => v.Priority)
            .Must(p => new[] { "Low", "Medium", "High" }.Contains(p))
            .WithMessage("Prioridad inválida.");

            RuleFor(v => v.ProjectId)
            .GreaterThan(0).WithMessage("Debe pertenecer a un proyecto válido.");
        }
    }
}

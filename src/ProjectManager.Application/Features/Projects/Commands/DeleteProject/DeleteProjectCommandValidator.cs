using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectManager.Application.Features.Projects.Commands.DeleteProject
{
    public class DeleteProjectCommandValidator : AbstractValidator<DeleteProjectCommand>
    {
        public DeleteProjectCommandValidator()
        {
            RuleFor(p => p.Id)
                .NotEmpty().WithMessage("El ID del proyecto es obligatorio para actualizar.")
                .GreaterThan(0).WithMessage("El ID debe ser un número positivo.");

        }
    }
}

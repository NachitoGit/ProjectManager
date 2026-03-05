using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectManager.Application.Features.Projects.Commands.UpdateProject
{
    public class UpdateProjectCommandValidator : AbstractValidator<UpdateProjectCommand>
    {
        public UpdateProjectCommandValidator()
        {
            RuleFor(p => p.Id)
                .NotEmpty().WithMessage("El ID del proyecto es obligatorio para actualizar.")
                .GreaterThan(0).WithMessage("El ID debe ser un número positivo.");

            RuleFor(p => p.Name)
                .NotEmpty().WithMessage("El nombre del proyecto es obligatorio")
                .MaximumLength(100).WithMessage("El nombre del proyecto no debe superar los 100 caracteres");

            RuleFor(p => p.Description)
                .NotEmpty().WithMessage("La descripción es obligatoria")
                .MinimumLength(10).WithMessage("La descripción del proyecto debe tener al menos 10 caracteres")
                .MaximumLength(250).WithMessage("La descripción del proyecto no debe superar los 250 caracteres");
        }
    }
}

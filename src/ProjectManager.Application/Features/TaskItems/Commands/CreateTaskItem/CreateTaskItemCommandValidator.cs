using FluentValidation;
using ProjectManager.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectManager.Application.Features.TaskItems.Commands.CreateTaskItem
{
    public class CreateTaskItemCommandValidator : AbstractValidator<CreateTaskItemCommand>
    {
        private readonly IUnitOfWork _unitOfWork; 

        public CreateTaskItemCommandValidator(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;

            RuleFor(p => p.Title)
                .NotEmpty().WithMessage("{PropertyName es obligatorio.}")
                .MaximumLength(250).WithMessage("{PropertyName} no debe exceder {MaxLength} caracteres.");

            RuleFor(p => p.ProjectId)
                .NotEmpty().WithMessage("{PropertyName}  es obligatorio.")
                .GreaterThan(0).WithMessage("{PropertyName} debe ser un número positivo.")
                .MustAsync(ProjectMustExist).WithMessage("El Proyecto especificado no existe.");

            RuleFor(p => p.Description)
                .NotEmpty().WithMessage("{PropertyName} es obligatorio.")
                .MaximumLength(1000).WithMessage("{PropertyName} no debe exceder {MaxLength} caracteres.");

            
            RuleFor(p => p.Priority)
                .NotEmpty().WithMessage("La prioridad es obligatoria.")
                .Must(x => new[] { "Low", "Medium", "High" }.Contains(x))
                .WithMessage("Prioridad inválida. Use: Low, Medium o High.");
        }

        private async Task<bool> ProjectMustExist(int projectId, CancellationToken cancellation)
        {
            var project = await _unitOfWork.Projects.GetByIdAsync(projectId);
            return project != null;
        }

    }
}

using FluentValidation;
using ProjectManager.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectManager.Application.Features.Projects.Commands.CreateProject
{
    public class CreateProjectCommandValidator : AbstractValidator<CreateProjectCommand>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUserService _currentUserService;
        public CreateProjectCommandValidator(IUnitOfWork unitOfWork, ICurrentUserService currentUserService)
        {
            _unitOfWork = unitOfWork;
            _currentUserService = currentUserService;

            RuleFor(p => p.Name)
                .NotEmpty().MaximumLength(100)
                .MustAsync(BeUniqueName).WithMessage("Ya tienes un proyecto con este nombre.");

            RuleFor(p => p.Description)
                .NotEmpty().WithMessage("La descripción es obligatoria")
                .MinimumLength(10).WithMessage("La descripción del proyecto debe tener al menos 10 caracteres")
                .MaximumLength(250).WithMessage("La descripción del proyecto no debe superar los 250 caracteres");
        }

        private async Task<bool> BeUniqueName (string name, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(_currentUserService.UserId)) return true;

            return await _unitOfWork.Projects.IsNameUniqueAsync(name, _currentUserService.UserId);
        }
    }
}

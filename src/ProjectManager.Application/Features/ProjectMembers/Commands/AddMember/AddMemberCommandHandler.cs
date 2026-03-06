using MediatR;
using Microsoft.AspNetCore.Identity;
using ProjectManager.Domain.Constants;
using ProjectManager.Domain.Entities;
using ProjectManager.Domain.Exceptions;
using ProjectManager.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectManager.Application.Features.ProjectMembers.Commands.AddMember
{
    public class AddMemberCommandHandler : IRequestHandler<AddMemberCommand, int>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUserService _currentUserService;
        private readonly UserManager<ApplicationUser> _userManager;

        public AddMemberCommandHandler(
            IUnitOfWork unitOfWork,
            UserManager<ApplicationUser> userManager,
            ICurrentUserService currentUserService)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
            _currentUserService = currentUserService;
        }

        public async Task<int> Handle(AddMemberCommand request, CancellationToken cancellationToken)
        {
            var currentUserId = _currentUserService.UserId;
            if (string.IsNullOrEmpty(currentUserId))
            {
                throw new UnauthorizedAccessException("Usuario no autenticado.");
            }

            var project = await _unitOfWork.Projects.GetByIdAsync(request.ProjectId);
            if (project == null)
                throw new NotFoundException(nameof(Project), request.ProjectId);

            var currentMember = await _unitOfWork.ProjectMembers.GetByIdsAsync(request.ProjectId, currentUserId);

            if (currentMember == null || currentMember.Role != ProjectRoles.Owner)
            {
                throw new UnauthorizedAccessException("Solo el propietario del proyecto puede agregar nuevos miembros.");
            }

            var userToAdd = await _userManager.FindByEmailAsync(request.UserEmail);
            if (userToAdd == null)
                throw new NotFoundException("User", request.UserEmail);

            var existingMember = await _unitOfWork.ProjectMembers.GetByIdsAsync(request.ProjectId, userToAdd.Id);
            if (existingMember != null)
                throw new Exception("El usuario ya es miembro de este proyecto.");

            var member = new ProjectMember
            {
                ProjectId = request.ProjectId,
                UserId = userToAdd.Id,
                Role = request.Role
            };

            await _unitOfWork.ProjectMembers.AddAsync(member);
            await _unitOfWork.CommitAsync();

            return member.Id;
        }
    }
}

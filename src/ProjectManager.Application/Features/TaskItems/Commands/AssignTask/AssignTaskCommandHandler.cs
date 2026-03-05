using MediatR;
using ProjectManager.Domain.Entities;
using ProjectManager.Domain.Exceptions;
using ProjectManager.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectManager.Application.Features.TaskItems.Commands.AssignTask
{
    public class AssignTaskCommandHandler : IRequestHandler<AssignTaskCommand, bool>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUserService _currentUserService;
        private readonly IPermissionService _permissionService;

        public AssignTaskCommandHandler(
            IUnitOfWork unitOfWork,
            ICurrentUserService currentUserService,
            IPermissionService permissionService)
        {
            _unitOfWork = unitOfWork;
            _currentUserService = currentUserService;
            _permissionService = permissionService;
        }

        public async Task<bool> Handle(AssignTaskCommand request, CancellationToken cancellationToken)
        {
            var canManage = await _permissionService.CanManageTasksAsync(request.ProjectId, _currentUserService.UserId);
            if (!canManage)
            {
                throw new UnauthorizedAccessException("No tienes permiso para asignar tareas en este proyecto.");
            }

            var isMember = await _permissionService.IsMemberAsync(request.ProjectId, request.AssigneeUserId);
            if (!isMember)
            {
                throw new Exception("No puedes asignar tareas a usuarios que no pertenecen al proyecto.");
            }

            var task = await _unitOfWork.Tasks.GetByIdAsync(request.TaskId);
            if (task == null || task.ProjectId != request.ProjectId)
            {
                throw new NotFoundException(nameof(TaskItem), request.TaskId);
            }

            task.AssignedToId = request.AssigneeUserId;

            await _unitOfWork.CommitAsync();
            return true;
        }
    }
}

using MediatR;
using Microsoft.Extensions.Logging;
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
        private readonly ILogger<AssignTaskCommandHandler> _logger;

        public AssignTaskCommandHandler(
            IUnitOfWork unitOfWork,
            ICurrentUserService currentUserService,
            IPermissionService permissionService,
            ILogger<AssignTaskCommandHandler> logger)
        {
            _unitOfWork = unitOfWork;
            _currentUserService = currentUserService;
            _permissionService = permissionService;
            _logger = logger;
        }

        public async Task<bool> Handle(AssignTaskCommand request, CancellationToken cancellationToken)
        {
            var userId = _currentUserService.UserId;
            var assigneeUserId = request.AssigneeUserId;
            var projectId = request.ProjectId;

            var canManage = await _permissionService.CanManageTasksAsync(request.ProjectId, userId);
            if (!canManage)
            {
                _logger.LogWarning("ACCESO DENEGADO: Usuario {userId} intentó asignar a {assigneeUserId} al proyecto {projectId} sin permisos", userId, assigneeUserId, projectId);
                throw new UnauthorizedAccessException("No tienes permiso para asignar tareas en este proyecto.");
            }

            var isMember = await _permissionService.IsMemberAsync(projectId, assigneeUserId);
            if (!isMember)
            {
                _logger.LogWarning("ACCESO DENEGADO: Usuario {userId} intentó asignar a {assigneeUserId} el cuál no es miembro del proyecto {projectId}", userId, assigneeUserId, projectId);
                throw new Exception("No puedes asignar tareas a usuarios que no pertenecen al proyecto.");
            }

            var task = await _unitOfWork.Tasks.GetByIdAsync(request.TaskId);
            if (task == null || task.ProjectId != projectId)
            {
                throw new NotFoundException(nameof(TaskItem), request.TaskId);
            }

            task.AssignedToId = assigneeUserId;

            await _unitOfWork.CommitAsync();
            return true;
        }
    }
}

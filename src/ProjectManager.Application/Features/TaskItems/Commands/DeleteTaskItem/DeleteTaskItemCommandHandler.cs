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

namespace ProjectManager.Application.Features.TaskItems.Commands.DeleteTaskItem
{
    public class DeleteTaskItemCommandHandler : IRequestHandler<DeleteTaskItemCommand, Unit>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPermissionService _permissionService;
        private readonly ICurrentUserService _currentUserService;
        private readonly ILogger<DeleteTaskItemCommandHandler> _logger;

        public DeleteTaskItemCommandHandler(
            IUnitOfWork unitOfWork, 
            ICurrentUserService currentUserService, 
            IPermissionService permissionService, 
            ILogger<DeleteTaskItemCommandHandler> logger)
        {
            _unitOfWork = unitOfWork;
            _currentUserService = currentUserService;
            _permissionService = permissionService;
            _logger = logger;
        }

        public async Task<Unit> Handle (DeleteTaskItemCommand request, CancellationToken cancellationToken)
        {
            var userId = _currentUserService.UserId;

            var taskItem = await _unitOfWork.Tasks.GetByIdAsync(request.Id);

            if (taskItem == null)
                throw new NotFoundException(nameof(TaskItem), request.Id);

            var isMember = await _permissionService.IsMemberAsync(taskItem.ProjectId, userId);
            if (!isMember) {
                _logger.LogWarning("ACCESO DENEGADO: El usuario {userId} intentó eliminar una tarea sin permisos en el proyecto", userId);
                throw new UnauthorizedAccessException("No tienes permiso para eliminar tareas en este proyecto.");
            }
                


            _unitOfWork.Tasks.Delete(taskItem);

            await _unitOfWork.CommitAsync();

            return Unit.Value;
        }
    }
}

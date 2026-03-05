using MediatR;
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

        public DeleteTaskItemCommandHandler(IUnitOfWork unitOfWork, ICurrentUserService currentUserService, IPermissionService permissionService)
        {
            _unitOfWork = unitOfWork;
            _currentUserService = currentUserService;
            _permissionService = permissionService;
        }

        public async Task<Unit> Handle (DeleteTaskItemCommand request, CancellationToken cancellationToken)
        {
            var taskItem = await _unitOfWork.Tasks.GetByIdAsync(request.Id);

            if (taskItem == null)
                throw new NotFoundException(nameof(TaskItem), request.Id);

            var isMember = await _permissionService.IsMemberAsync(taskItem.ProjectId, _currentUserService.UserId);
            if (!isMember)
                throw new UnauthorizedAccessException("No tienes permiso para eliminar tareas en este proyecto.");


            _unitOfWork.Tasks.Delete(taskItem);

            await _unitOfWork.CommitAsync();

            return Unit.Value;
        }
    }
}

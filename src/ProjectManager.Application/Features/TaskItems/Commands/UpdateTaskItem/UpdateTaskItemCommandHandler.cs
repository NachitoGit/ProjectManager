using AutoMapper;
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

namespace ProjectManager.Application.Features.TaskItems.Commands.UpdateTaskItem
{
    public class UpdateTaskItemCommandHandler : IRequestHandler<UpdateTaskItemCommand, Unit>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IPermissionService _permissionService;
        private readonly ICurrentUserService _currentUserService;
        private readonly ILogger<UpdateTaskItemCommandHandler> _logger;

        public UpdateTaskItemCommandHandler 
            (IUnitOfWork unitOfWork, 
            IMapper mapper, 
            IPermissionService permissionService, 
            ICurrentUserService currentUserService, 
            ILogger<UpdateTaskItemCommandHandler> logger)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _permissionService = permissionService;
            _currentUserService = currentUserService;
            _logger = logger;
        }

        public async Task<Unit> Handle(UpdateTaskItemCommand request, CancellationToken cancellationToken)
        {
            var userId = _currentUserService.UserId;

            var taskItem = await _unitOfWork.Tasks.GetByIdAsync(request.Id);

            if (taskItem == null)
                throw new NotFoundException(nameof(TaskItem), request.Id);

            var isMember = await _permissionService.IsMemberAsync(taskItem.ProjectId, userId);
            if (!isMember)
            {
                _logger.LogWarning("ACCESO DENEGADO: El usuario {userId} intentó eliminar una tarea sin permisos", userId);
                throw new UnauthorizedAccessException("No tienes permiso para editar esta tarea.");
            }

            _mapper.Map(request, taskItem);

            _unitOfWork.Tasks.Update(taskItem);

            await _unitOfWork.CommitAsync();

            return Unit.Value;
        }
    }
}

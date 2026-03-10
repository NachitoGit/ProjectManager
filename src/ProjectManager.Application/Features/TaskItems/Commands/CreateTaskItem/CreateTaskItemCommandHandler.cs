using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using ProjectManager.Domain.Constants;
using ProjectManager.Domain.Entities;
using ProjectManager.Domain.Exceptions;
using ProjectManager.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectManager.Application.Features.TaskItems.Commands.CreateTaskItem
{
    public class CreateTaskItemCommandHandler :IRequestHandler<CreateTaskItemCommand, int>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ICurrentUserService _currentUserService;
        private readonly IPermissionService _permissionService;
        private readonly ILogger<CreateTaskItemCommandHandler> _logger;

        public CreateTaskItemCommandHandler (IUnitOfWork unitOfWork, IMapper mapper, ICurrentUserService currentUserService, IPermissionService permissionService, ILogger<CreateTaskItemCommandHandler> logger)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _currentUserService = currentUserService;
            _permissionService = permissionService;
            _logger = logger;
        }

        public async Task<int> Handle (CreateTaskItemCommand request, CancellationToken cancellationToken)
        {
            var userId = _currentUserService.UserId;

            _logger.LogInformation("Usuario {UserId} intentando crear tarea en Proyecto {ProjectId}", userId, request.ProjectId);

            var canManage = await _permissionService.CanManageTasksAsync(request.ProjectId, userId);
            if (!canManage)
            {
                _logger.LogWarning("ACCESO DENEGADO: Usuario {UserId} intentó crear tarea en Proyecto {ProjectId} sin permisos", userId, request.ProjectId);
                throw new ForbiddenAccessException();
            }

            var projectExists = await _unitOfWork.Projects.GetByIdAsync(request.ProjectId);
            if (projectExists == null)
            {
                _logger.LogWarning("Proyecto {ProjectId} no encontrado para la creación de tarea", request.ProjectId);
                throw new Exception($"El proyecto con ID {request.ProjectId} no existe.");
            }

            var taskItem = _mapper.Map<TaskItem>(request);

            taskItem.Priority ??= "Low";

            await _unitOfWork.Tasks.AddAsync(taskItem);
            await _unitOfWork.CommitAsync();

            _logger.LogInformation("Tarea {TaskId} creada exitosamente para el Proyecto {ProjectId}", taskItem.Id, request.ProjectId);

            return taskItem.Id;
        }
    }
}

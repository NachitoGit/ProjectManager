using AutoMapper;
using MediatR;
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

        public CreateTaskItemCommandHandler (IUnitOfWork unitOfWork, IMapper mapper, ICurrentUserService currentUserService, IPermissionService permissionService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _currentUserService = currentUserService;
            _permissionService = permissionService;
        }

        public async Task<int> Handle (CreateTaskItemCommand request, CancellationToken cancellationToken)
        {
            var canManage = await _permissionService.CanManageTasksAsync(request.ProjectId, _currentUserService.UserId);

            if (!canManage)
            {
                throw new ForbiddenAccessException();
            }

            var projectExists = await _unitOfWork.Projects.GetByIdAsync(request.ProjectId);
            if (projectExists == null)
            {
                throw new Exception($"El proyecto con ID {request.ProjectId} no existe.");
            }

            var taskItem = _mapper.Map<TaskItem>(request);
            taskItem.ProjectId = request.ProjectId;

            if (string.IsNullOrEmpty(taskItem.Priority))
            {
                taskItem.Priority = "Low";
            }

            try
            {
                await _unitOfWork.Tasks.AddAsync(taskItem);
                await _unitOfWork.CommitAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Error al guardar en BD: " + ex.InnerException?.Message ?? ex.Message);
            }

            return taskItem.Id;
        }
    }
}

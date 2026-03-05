using AutoMapper;
using MediatR;
using ProjectManager.Domain.Entities;
using ProjectManager.Domain.Exceptions;
using ProjectManager.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectManager.Application.Features.TaskItems.Queries.GetTaskItemDetail
{
    public class GetTaskItemDetailQueryHandler : IRequestHandler<GetTaskItemDetailQuery, TaskItemDetailDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IPermissionService _permissionService;
        private readonly ICurrentUserService _currentUserService;

        public GetTaskItemDetailQueryHandler (IUnitOfWork unitOfWork, IMapper mapper, IPermissionService permissionService, ICurrentUserService currentUserService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _permissionService = permissionService;
            _currentUserService = currentUserService;
        }

        public async Task<TaskItemDetailDto> Handle (GetTaskItemDetailQuery request, CancellationToken cancellationToken)
        {
            var taskItem = await _unitOfWork.Tasks.GetByIdAsync (request.TaskItemId);

            if (taskItem == null)
            {
                throw new NotFoundException(nameof(TaskItem), request.TaskItemId);
            }

            var isMember = await _permissionService.IsMemberAsync(taskItem.ProjectId, _currentUserService.UserId);

            if (!isMember)
            {
                throw new UnauthorizedAccessException("No tienes permiso para ver esta tarea.");
            }

            var dto = _mapper.Map<TaskItemDetailDto>(taskItem);

            return dto;
        }
    }
}

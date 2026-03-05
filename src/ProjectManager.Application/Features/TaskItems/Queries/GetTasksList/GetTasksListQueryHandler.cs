using AutoMapper;
using MediatR;
using ProjectManager.Application.Common.Models;
using ProjectManager.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectManager.Application.Features.TaskItems.Queries.GetTasksList
{
    public class GetTasksListQueryHandler : IRequestHandler<GetTasksListQuery, PaginatedResponse<TaskItemListItemDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IPermissionService _permissionService;
        private readonly ICurrentUserService _currentUserService;
        public GetTasksListQueryHandler (IUnitOfWork unitOfWork, IMapper mapper, IPermissionService permissionService, ICurrentUserService currentUserService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _permissionService = permissionService;
            _currentUserService = currentUserService;
        }

        public async Task<PaginatedResponse<TaskItemListItemDto>> Handle (GetTasksListQuery request, CancellationToken cancellationToken)
        {
            var isMember = await _permissionService.IsMemberAsync(request.ProjectId, _currentUserService.UserId);

            if (!isMember)
            {
                throw new UnauthorizedAccessException("No tienes permiso para ver las tareas de este proyecto.");
            }

            var (taskList, totalCount) = await _unitOfWork.Tasks.GetPaginatedByProjectIdAsync(
                request.ProjectId,
                request.PageNumber,
                request.PageSize,
                request.IsCompleted,
                request.SearchTerm
                );


            var dtoList = _mapper.Map<List<TaskItemListItemDto>>(taskList);

            return new PaginatedResponse<TaskItemListItemDto>(
                dtoList,
                totalCount,
                request.PageNumber,
                request.PageSize
                );
        }
    }
}

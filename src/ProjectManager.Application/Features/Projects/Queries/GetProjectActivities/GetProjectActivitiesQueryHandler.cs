using AutoMapper;
using MediatR;
using ProjectManager.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectManager.Application.Features.Projects.Queries.GetProjectActivities
{
    public class GetProjectActivitiesQueryHandler : IRequestHandler<GetProjectActivitiesQuery, IEnumerable<ProjectActivityDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ICurrentUserService _currentUserService;

        public GetProjectActivitiesQueryHandler(IUnitOfWork unitOfWork, IMapper mapper, ICurrentUserService currentUserService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _currentUserService = currentUserService;
        }

        public async Task<IEnumerable<ProjectActivityDto>> Handle(GetProjectActivitiesQuery request, CancellationToken cancellationToken)
        {
            var userId = _currentUserService.UserId;

            var isMember = await _unitOfWork.ProjectMembers.IsUserInProjectAsync(request.ProjectId, userId);

            if (!isMember)
            {
                throw new UnauthorizedAccessException("No tienes permiso para ver el historial de este proyecto.");
            }

            var activities = await _unitOfWork.ProjectActivities.GetByProjectIdAsync(
                request.ProjectId,
                request.PageNumber,
                request.PageSize);

            return _mapper.Map<IEnumerable<ProjectActivityDto>>(activities);
        }
    }
}

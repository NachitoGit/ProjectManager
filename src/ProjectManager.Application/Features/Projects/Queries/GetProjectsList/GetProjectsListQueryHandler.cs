using AutoMapper;
using MediatR;
using ProjectManager.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectManager.Application.Features.Projects.Queries.GetProjectsList
{
    public class GetProjectsListQueryHandler : IRequestHandler<GetProjectsListQuery, IEnumerable<ProjectListItemDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ICurrentUserService _currentUserService;

        public GetProjectsListQueryHandler(IUnitOfWork unitOfWork , IMapper mapper, ICurrentUserService currentUserService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _currentUserService = currentUserService;
        }

        public async Task<IEnumerable<ProjectListItemDto>> Handle(GetProjectsListQuery request, CancellationToken cancellationToken)
        {
            var userId = _currentUserService.UserId;

            var projectList = await _unitOfWork.Projects.GetProjectsByUserIdAsync(userId);

            return _mapper.Map<List<ProjectListItemDto>>(projectList);
        }
    }
}

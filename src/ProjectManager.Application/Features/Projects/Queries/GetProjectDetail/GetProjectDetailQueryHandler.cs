using AutoMapper;
using MediatR;
using ProjectManager.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectManager.Application.Features.Projects.Queries.GetProjectDetail
{
    public class GetProjectDetailQueryHandler : IRequestHandler<GetProjectDetailQuery, ProjectDetailDto>
    {
        private readonly IProjectRepository _projectRepository;
        private readonly IMapper _mapper;
        private readonly IPermissionService _permissionService;
        private readonly ICurrentUserService _currentUserService;

        public GetProjectDetailQueryHandler(IProjectRepository projectRepository, IMapper mapper, ICurrentUserService currentUserService, IPermissionService permissionService)
        {
            _projectRepository = projectRepository;
            _mapper = mapper;
            _currentUserService = currentUserService;
            _permissionService = permissionService;
        }

        public async Task<ProjectDetailDto> Handle(GetProjectDetailQuery request, CancellationToken cancellationToken)
        {

            var project = await _projectRepository.GetByIdAsync(request.Id);

            if (project == null)
            {
                return null;
            }

            var isMember = await _permissionService.IsMemberAsync(request.Id, _currentUserService.UserId);

            if (!isMember)
            {
                throw new UnauthorizedAccessException("No tienes permiso para ver los detalles de este proyecto.");
            }

            return _mapper.Map<ProjectDetailDto>(project);
        }
    }
}

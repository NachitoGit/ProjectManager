using AutoMapper;
using MediatR;
using ProjectManager.Domain.Constants;
using ProjectManager.Domain.Entities;
using ProjectManager.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectManager.Application.Features.Projects.Commands.CreateProject
{
    public class CreateProjectCommandHandler : IRequestHandler<CreateProjectCommand, int>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ICurrentUserService _currentUserService;

        public CreateProjectCommandHandler(IUnitOfWork unitOfWork, IMapper mapper, ICurrentUserService currentUserService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _currentUserService = currentUserService;
        }

        public async Task<int> Handle(CreateProjectCommand request, CancellationToken cancellationToken)
        {
            var userId = _currentUserService.UserId;

            if (string.IsNullOrEmpty(userId))
            {
                throw new UnauthorizedAccessException("Debes estar autenticado para crear proyectos.");
            }

            var project = _mapper.Map<Project>(request);

            project.Members.Add(new ProjectMember
            {
                UserId = userId,
                Role = ProjectRoles.Owner,
                JoinedDate = DateTime.UtcNow
            });

            await _unitOfWork.Projects.AddAsync(project);


            await _unitOfWork.CommitAsync();

            return project.Id;
        }
    }
}

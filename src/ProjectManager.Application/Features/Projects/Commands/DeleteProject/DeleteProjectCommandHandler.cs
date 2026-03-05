using AutoMapper;
using MediatR;
using ProjectManager.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectManager.Application.Features.Projects.Commands.DeleteProject
{
    public class DeleteProjectCommandHandler : IRequestHandler<DeleteProjectCommand, Unit>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public DeleteProjectCommandHandler (IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Unit> Handle(DeleteProjectCommand request, CancellationToken cancellationToken)
        {
            var projectToDelete = await _unitOfWork.Projects.GetByIdAsync(request.Id);

            if (projectToDelete == null)
            {
                throw new ArgumentException($"Project with ID {request.Id} not found.");
            }

            _mapper.Map(request, projectToDelete);

            await _unitOfWork.Projects.DeleteAsync(projectToDelete);

            await _unitOfWork.CommitAsync();

            return Unit.Value;
        }
    }
}

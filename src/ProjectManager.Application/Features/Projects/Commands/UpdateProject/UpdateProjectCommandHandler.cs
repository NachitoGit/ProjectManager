using AutoMapper;
using MediatR;
using ProjectManager.Domain.Entities;
using ProjectManager.Domain.Interfaces;

namespace ProjectManager.Application.Features.Projects.Commands.UpdateProject
{
    public class UpdateProjectCommandHandler : IRequestHandler<UpdateProjectCommand, Unit>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public UpdateProjectCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
            
        public async Task<Unit> Handle(UpdateProjectCommand request, CancellationToken cancellationToken)
        {
            var projectToUpdate = await _unitOfWork.Projects.GetByIdAsync(request.Id);

            if (projectToUpdate == null)
            {
                throw new Exception($"Project with ID {request.Id} not found.");
            }

            _mapper.Map(request, projectToUpdate);

            await _unitOfWork.CommitAsync();

            return Unit.Value;
        }
    }
}

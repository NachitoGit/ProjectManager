using AutoMapper;
using ProjectManager.Domain.Entities;
using ProjectManager.Application.Features.Projects.Commands.CreateProject;
using ProjectManager.Application.Features.Projects.Queries.GetProjectDetail;
using ProjectManager.Application.Features.Projects.Commands.UpdateProject;
using ProjectManager.Application.Features.Projects.Commands.DeleteProject;
using ProjectManager.Application.Features.Projects.Queries.GetProjectsList;
using ProjectManager.Application.Features.TaskItems.Commands.CreateTaskItem;
using ProjectManager.Application.Features.TaskItems.Queries.GetTasksList;
using ProjectManager.Application.Features.TaskItems.Queries.GetTaskItemDetail;
using ProjectManager.Application.Features.TaskItems.Commands.UpdateTaskItem;

namespace ProjectManager.Application.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // COMMANDS
            CreateMap<CreateProjectCommand, Project>();
            CreateMap<UpdateProjectCommand, Project>().ReverseMap();
            CreateMap<DeleteProjectCommand, Project>();
            CreateMap<Project, ProjectListItemDto>();

            CreateMap<CreateTaskItemCommand, TaskItem>();
            CreateMap<UpdateTaskItemCommand, TaskItem>()
                .ForMember(dest => dest.Id, opt => opt.Ignore());

            // QUERIES
            CreateMap<Project, ProjectDetailDto>()
                .ForMember(dest => dest.TotalTasks,
                           opt => opt.MapFrom(src => src.Tasks.Count));

            CreateMap<ApplicationUser, UserSummaryDto>();
            CreateMap<TaskItem, TaskItemListItemDto>();
            CreateMap<TaskItem, TaskItemDetailDto>();

            // Mapeo de la tarea
            CreateMap<TaskItem, TaskItemDto>()
                .ForMember(dest => dest.AssignedToUser,
                           // Asume que el repositorio incluye el objeto AssignedTo (ApplicationUser)
                           opt => opt.MapFrom(src => src.AssignedTo != null ? src.AssignedTo.UserName : "Unassigned"));

            // Mapeo del proyecto
            CreateMap<Project, ProjectDetailDto>()
                .ForMember(dest => dest.TotalTasks,
                           opt => opt.MapFrom(src => src.Tasks.Count))
                .ForMember(dest => dest.Tasks, opt => opt.MapFrom(src => src.Tasks));


        }
    }
}

using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProjectManager.Application.Features.Projects.Commands.CreateProject;
using ProjectManager.Application.Features.Projects.Commands.UpdateProject;
using ProjectManager.Application.Features.Projects.Queries.GetProjectDetail;
using FluentValidation;
using ProjectManager.Application.Features.Projects.Commands.DeleteProject;
using ProjectManager.Application.Features.Projects.Queries.GetProjectsList;
using ProjectManager.Application.Features.ProjectMembers.Commands.AddMember;
using ProjectManager.Domain.Interfaces;
using ProjectManager.Infrastructure.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using ProjectManager.Application.Features.Projects.Queries.GetProjectActivities;

namespace ProjectManager.WebAPI.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiController]
    [Route("api/[controller]")]
    public class ProjectsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ProjectsController(IMediator mediator) {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> CreateProject([FromBody] CreateProjectCommand command)
        {
            var projectId = await _mediator.Send(command);
            return CreatedAtAction(nameof(GetProjectById), new { id = projectId }, null);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ProjectDetailDto>> GetProjectById(int id)
        {
            var projectDto = await _mediator.Send(new GetProjectDetailQuery { Id = id });

            return projectDto != null ? Ok(projectDto) : NotFound();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProject(int id, [FromBody] UpdateProjectCommand command)
        {
            if (id != command.Id)
            {
                return BadRequest("El ID de la ruta no coincide con el ID del cuerpo.");
            }

            await _mediator.Send(command);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProject(int id)
        {
            await _mediator.Send(new DeleteProjectCommand { Id = id });
            return NoContent();
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProjectListItemDto>>> GetProjects()
        {
            var projects = await _mediator.Send(new GetProjectsListQuery());
            return Ok(projects);
        }

        [HttpPost("{projectId}/members")]
        public async Task<IActionResult> AddMember(int projectId, [FromBody] AddMemberCommand command)
        {
            command.ProjectId = projectId;
            var result = await _mediator.Send(command);
            return Ok();
        }

        [HttpGet("{id}/activities")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<ProjectActivityDto>>> GetActivities(int id, [FromQuery] GetProjectActivitiesQuery query)
        {
            query.ProjectId = id;
            var activities = await _mediator.Send(new GetProjectActivitiesQuery { ProjectId = id });
            return Ok(activities);
        }

    }
}

using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProjectManager.Application.Features.TaskItems.Commands.CreateTaskItem;
using ProjectManager.Application.Features.TaskItems.Queries.GetTasksList;
using ProjectManager.Application.Features.TaskItems.Queries.GetTaskItemDetail;
using ProjectManager.Application.Features.TaskItems.Commands.UpdateTaskItem;
using ProjectManager.Application.Features.TaskItems.Commands.DeleteTaskItem;

namespace ProjectManager.WebAPI.Controllers
{
    [Route("api/projects/{projectId}/tasks")]
    [ApiController]
    [Authorize]
    public class TasksController : ControllerBase
    {
        private readonly IMediator _mediator;

        public TasksController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> Create (int projectId, [FromBody] CreateTaskItemCommand command)
        {
            command.ProjectId = projectId;
            var newTaskId = await _mediator.Send(command);
            return CreatedAtAction(nameof(GetTaskDetail), new { projectId, taskId = newTaskId }, newTaskId);
        }

        [HttpGet]
        public async Task<IActionResult> GetTasksByProject (
            int projectId,
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10,
            [FromQuery] bool? isCompleted = null,
            [FromQuery]string? searchTerm = null)
        {
            var query = new GetTasksListQuery
            {
                ProjectId = projectId,
                PageNumber = pageNumber,
                PageSize = pageSize,
                IsCompleted = isCompleted,
                SearchTerm = searchTerm
            };

            return Ok(await _mediator.Send(query));
        }

        [HttpGet("{taskId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetTaskDetail (int taskId)
        {
            return Ok(await _mediator.Send(new GetTaskItemDetailQuery { TaskItemId = taskId }));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTask(int projectId, int id, UpdateTaskItemCommand command)
        {
            if (id != command.Id) return BadRequest("El ID no coincide.");

            command.ProjectId = projectId;
            await _mediator.Send(command);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTask(int id)
        {
            await _mediator.Send(new DeleteTaskItemCommand { Id = id });
            return NoContent();
        }
    }
}
 
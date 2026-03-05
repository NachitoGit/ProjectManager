using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectManager.Application.Features.TaskItems.Commands.UpdateTaskItem
{
    public class UpdateTaskItemCommand : IRequest<Unit>
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public DateTime? DueDate { get; set; }

        public bool IsCompleted { get; set; }
        public string Priority { get; set; } = "Low";

        public int ProjectId { get; set; }
        public string? AssignedToId { get; set; }
    }
}

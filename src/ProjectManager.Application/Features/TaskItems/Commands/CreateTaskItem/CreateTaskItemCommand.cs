using MediatR;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectManager.Application.Features.TaskItems.Commands.CreateTaskItem
{
    public class CreateTaskItemCommand : IRequest<int>
    {
        [Required]
        public int ProjectId { get; set; }

        [Required]
        [MaxLength(250)]
        public string Title { get; set; }

        [MaxLength(1000)]
        public string Description { get; set; }

        public DateTime? DueDate { get; set; }

        public string Priority { get; set; }

        public string AssignedToId { get; set; }
    }
}

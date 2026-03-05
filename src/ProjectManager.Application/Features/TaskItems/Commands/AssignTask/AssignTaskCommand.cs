using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectManager.Application.Features.TaskItems.Commands.AssignTask
{
    public class AssignTaskCommand : IRequest<bool>
    {
        public int TaskId { get; set; }
        public int ProjectId  { get; set; }
        public string AssigneeUserId { get; set; }
    }
}

using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectManager.Application.Features.TaskItems.Commands.DeleteTaskItem
{
    public class DeleteTaskItemCommand : IRequest<Unit>
    {
        public int Id { get; set; }
    }
}

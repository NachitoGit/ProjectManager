using MediatR;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectManager.Application.Features.Projects.Commands.DeleteProject
{
    public class DeleteProjectCommand : IRequest<Unit>
    {
        [Required]
        public int Id { get; set; }
    }
}

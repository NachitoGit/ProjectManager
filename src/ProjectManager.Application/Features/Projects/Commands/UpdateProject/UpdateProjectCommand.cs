using MediatR;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectManager.Application.Features.Projects.Commands.UpdateProject
{
    public class UpdateProjectCommand : IRequest<Unit>
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public string Name {  get; set; }
        [Required]
        public string Description { get; set; }
    }
}

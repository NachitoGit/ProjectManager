using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectManager.Application.Features.ProjectMembers.Commands.AddMember
{
    public class AddMemberCommand : IRequest<int>
    {
        public int ProjectId { get; set; }
        public string UserEmail { get; set; }
        public string Role { get; set; } = "Contributor";   
    }
}

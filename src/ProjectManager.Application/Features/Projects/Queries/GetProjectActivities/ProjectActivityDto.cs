using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectManager.Application.Features.Projects.Queries.GetProjectActivities
{
    public class ProjectActivityDto
    {
        public string Message { get; set; }
        public string ActivityType { get; set; }
        public DateTime OcurredAt { get; set; }
        public string CreatedBy { get; set; }
    }
}

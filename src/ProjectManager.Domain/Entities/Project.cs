using ProjectManager.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectManager.Domain.Entities
{
    public class Project : BaseEntity
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime StartDate { get; set; } = DateTime.UtcNow;
        public string Status { get; set; } = "Active";

        //Relations
        public ICollection<TaskItem> Tasks { get; set; } = new List<TaskItem>();

        public ICollection<ProjectMember> Members { get; set; } = new List<ProjectMember>();
    }
}

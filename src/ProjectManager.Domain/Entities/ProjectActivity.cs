using ProjectManager.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectManager.Domain.Entities
{
    public  class ProjectActivity : BaseEntity
    {
        public int ProjectId { get; set; }
        public string Message { get; set; } = string.Empty;
        public string ActivityType { get; set; } = string.Empty;
        public DateTime OccurredAt { get; set; } = DateTime.Now;

        public Project Project { get; set; } = null!;
    }
}

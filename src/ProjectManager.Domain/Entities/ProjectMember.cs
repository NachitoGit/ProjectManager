using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectManager.Domain.Entities
{
    public class ProjectMember
    {
        public int Id { get; set; }

        //Relation with the Project
        public int ProjectId { get; set; }
        public Project Project { get; set; } = null!;

        //Relation with the User
        public string UserId { get; set; } = null!;
        public ApplicationUser User { get; set; }

        //Specific rol in the project
        public string Role { get; set; } = string.Empty;

        public DateTime JoinedDate { get; set; } = DateTime.UtcNow;
    }
}

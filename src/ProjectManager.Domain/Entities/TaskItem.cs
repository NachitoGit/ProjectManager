using ProjectManager.Domain.Common;
using ProjectManager.Domain.Events;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectManager.Domain.Entities
{
    public class TaskItem : BaseEntity
    {
        [Required]
        [MaxLength(250)]
        public string Title { get; set; }
        [MaxLength(1000)]
        public string Description { get; set; }
        public DateTime? DueDate { get; set; }
        private bool _isCompleted;
        public bool IsCompleted
        {
            get => _isCompleted;
            set
            {
                if (value == true && _isCompleted == false)
                {
                    AddDomainEvent(new TaskCompletedEvent(this));
                }
                _isCompleted = value;
            }
        }
        public string Priority { get; set; }

        // Foreign Keys and navegation
        public int ProjectId { get; set; }
        public Project Project { get; set; }

        public string? AssignedToId { get; set; }
        public virtual ApplicationUser? AssignedTo { get; set; }

    }
}

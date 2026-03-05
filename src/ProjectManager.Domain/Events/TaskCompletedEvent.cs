using MediatR;
using ProjectManager.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectManager.Domain.Events
{
    public class TaskCompletedEvent : INotification
    {
        public TaskItem Item { get; set; }
        public TaskCompletedEvent (TaskItem item) => Item = item;
    }
}

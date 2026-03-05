using MediatR;
using Microsoft.Extensions.Logging;
using ProjectManager.Domain.Entities;
using ProjectManager.Domain.Events;
using ProjectManager.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectManager.Application.Features.TaskItems.Events
{
    public class TaskCompletedEventHandler : INotificationHandler<TaskCompletedEvent>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<TaskCompletedEventHandler> _logger;

        public TaskCompletedEventHandler(IUnitOfWork unitOfWork, ILogger<TaskCompletedEventHandler> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task Handle(TaskCompletedEvent notification, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Manejando evento de dominio: Tarea {TaskId} completada", notification.Item.Id);

            var activity = new ProjectActivity
            {
                ProjectId = notification.Item.ProjectId,
                ActivityType = "TaskCompleted",
                Message = $"La tarea '{notification.Item.Title}' ha sido marcada como completada.",
                OccurredAt = DateTime.UtcNow
            };

            await _unitOfWork.ProjectActivities.AddAsync(activity);

            await _unitOfWork.CommitAsync();
        }
    }
}

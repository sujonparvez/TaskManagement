using Application.Interfaces;
using Infrastructure.Hubs;
using Microsoft.AspNetCore.SignalR;

namespace Infrastructure.Services
{
    public class TaskNotifier(IHubContext<TaskHub> _hubContext, Domain.Interfaces.INotificationRepository _notificationRepository, Domain.Interfaces.IUnitOfWork _unitOfWork) : ITaskNotifier
    {
        public async System.Threading.Tasks.Task NotifyTaskAssignedAsync(int userId, int taskId, string title, CancellationToken cancellationToken)
        {
            var notification = new Domain.Entities.Notification
            {
                UserId = userId,
                TaskId = taskId,
                Message = $"You have been assigned to task '{title}'.",
                CreatedAt = DateTime.UtcNow,
                IsRead = false
            };

            await _notificationRepository.AddAsync(notification, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            await _hubContext.Clients.All.SendAsync("TaskAssigned", taskId, title, cancellationToken);
        }
        public async System.Threading.Tasks.Task NotifyTaskCompletedAsync(int userId, int taskId, string title, CancellationToken cancellationToken)
        {
             var notification = new Domain.Entities.Notification
            {
                UserId = userId,
                TaskId = taskId,
                Message = $"Task '{title}' has been completed.",
                CreatedAt = DateTime.UtcNow,
                IsRead = false
            };

            await _notificationRepository.AddAsync(notification, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            await _hubContext.Clients.All.SendAsync("TaskCompleted", taskId, title, cancellationToken);
        }
    }
}

using MediatR;
using Application.DTOs;
using Domain.Interfaces;
using Application.Interfaces;

namespace Application.Features.Notifications.Queries
{
    public class GetNotificationsQuery : IRequest<IEnumerable<NotificationDto>>
    {
    }

    public class GetNotificationsQueryHandler : IRequestHandler<GetNotificationsQuery, IEnumerable<NotificationDto>>
    {
        private readonly INotificationRepository _notificationRepository;
        private readonly ICurrentUserService _currentUserService;

        public GetNotificationsQueryHandler(INotificationRepository notificationRepository, ICurrentUserService currentUserService)
        {
            _notificationRepository = notificationRepository;
            _currentUserService = currentUserService;
        }

        public async Task<IEnumerable<NotificationDto>> Handle(GetNotificationsQuery request, CancellationToken cancellationToken)
        {
            var userId = _currentUserService.UserId;
            var notifications = await _notificationRepository.GetByUserIdAsync(userId, cancellationToken);
            
            return notifications.Select(n => new NotificationDto
            {
                Id = n.Id,
                Message = n.Message,
                TaskId = n.TaskId,
                CreatedAt = n.CreatedAt,
                IsRead = n.IsRead
            });
        }
    }
}

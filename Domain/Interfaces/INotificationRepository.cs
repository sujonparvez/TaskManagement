using Domain.Entities;

namespace Domain.Interfaces
{
    public interface INotificationRepository
    {
        System.Threading.Tasks.Task AddAsync(Notification notification, CancellationToken cancellationToken);
        System.Threading.Tasks.Task<IEnumerable<Notification>> GetByUserIdAsync(int userId, CancellationToken cancellationToken);
    }
}

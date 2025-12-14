namespace Application.Interfaces
{
    public interface ITaskNotifier
    {
        Task NotifyTaskAssignedAsync(int userId, int taskId, string title, CancellationToken cancellationToken);
        Task NotifyTaskCompletedAsync(int userId, int taskId, string title, CancellationToken cancellationToken);
    }
}

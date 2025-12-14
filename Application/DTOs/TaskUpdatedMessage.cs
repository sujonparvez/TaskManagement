namespace Application.DTOs
{
    public class TaskUpdatedMessage
    {
        public TaskUpdatedMessage(Domain.Entities.Task task)
        {
            Task = task;
        }

        public Domain.Entities.Task Task { get; }
    }
}

namespace Application.DTOs
{
    public class TaskCreatedMessage
    {
        public TaskCreatedMessage(Domain.Entities.Task task)
        {
            Task = task;
        }

        public Domain.Entities.Task Task { get; }
    }
}

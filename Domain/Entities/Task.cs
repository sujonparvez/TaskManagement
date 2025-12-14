namespace Domain.Entities
{
    public class Task
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public Domain.Enums.TaskStatus Status { get; set; }
        public int? AssignedUserId { get; set; }
        public User? AssignedUser { get; set; }
        public int CreatedByUserId { get; set; }
        public User CreatedByUser { get; set; }
        public int TeamId { get; set; }
        public Team Team { get; set; }
        public DateTime DueDate { get; set; }

    }
}

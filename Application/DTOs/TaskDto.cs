using Application.DTOs;

namespace Application.DTOs
{
    public class TaskDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public Domain.Enums.TaskStatus Status { get; set; }
        public int? AssignedUserId { get; set; }
        public string? AssignedUserName { get; set; }
        public int CreatedByUserId { get; set; }
        public string CreatedByUserName { get; set; }
        public int TeamId { get; set; }
        public string TeamName { get; set; }
        public DateTime DueDate { get; set; }
    }
}

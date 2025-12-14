using Domain.Enums;

namespace Domain.Entities
{
    public class User
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public UserRole Role { get; set; }
        public ICollection<Domain.Entities.Task> AssignedTasks { get; set; } = new List<Domain.Entities.Task>();
        public ICollection<Domain.Entities.Task> CretaedTasks { get; set; } = new List<Domain.Entities.Task>();

    }
}

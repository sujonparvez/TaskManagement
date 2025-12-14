namespace Domain.Entities
{
    public class Team
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public ICollection<Domain.Entities.Task> Tasks { get; set; }
    }
}

namespace TaskManager.Domain.Entities
{
    public class Projects
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public required string Description { get; set; }
        public required string OwnerUser { get; set; }
        public bool Active { get; set; }
        public ICollection<Tasks>? Tasks { get; set; }
    }
}

using TaskManager.Domain.Enums;

namespace TaskManager.Domain.Entities
{
    public class Tasks
    {
        public int Id { get; set; }
        public int ProjectId { get; set; }
        public required string Name { get; set; }
        public PriorityEnum Priority { get; set; }
        public IEnumerable<string>? Comments { get; set; }
        public StatusTaskEnum Status { get; set; }
        public string Historic { get; set; }
    }
}

using TaskManager.Domain.Enums;

namespace TaskManager.Domain.Entities
{
    public class TaskEntity
    {
        public required Guid TaskId { get; set; }
        public required string Name { get; set; }
        public PriorityEnum Priority { get; set; }
        public required IEnumerable<string> Comments { get; set; }
        public StatusTaskEnum Status { get; set; }
    }
}

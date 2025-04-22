using TaskManager.Domain.Enums;

namespace TaskManager.Domain.DTOs
{
    public class TaskDTO
    {
        public required int TaskId { get; set; }
        public required int ProjectId { get; set; }
        public required string Name { get; set; }
        public required PriorityEnum Priority { get; set; }
        public required StatusTaskEnum Status { get; set; }
        public IEnumerable<string>? Comments { get; set; }
    }
}

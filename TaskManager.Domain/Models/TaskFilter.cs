using TaskManager.Domain.Enums;

namespace TaskManager.Domain.Models
{
    public class TaskFilter
    {
        public int Id { get; set; }
        public int ProjectId { get; set; }
        public string? Name { get; set; }
        public PriorityEnum? Priority { get; set; }
        public StatusTaskEnum? Status { get; set; }
        public int LimitTaskInProject { get; set; } = 20;
    }
}

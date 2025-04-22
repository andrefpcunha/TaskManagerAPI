using TaskManager.Domain.Enums;

namespace TaskManager.Application.UseCases.Task.v1.NewTask
{
    public class NewTaskResult
    {
        public required int TaskId { get; set; }
        public required int ProjectId { get; set; }
        public required string Name { get; set; }
        public required PriorityEnum Priority { get; set; }
        public required StatusTaskEnum Status { get; set; }

    }
}

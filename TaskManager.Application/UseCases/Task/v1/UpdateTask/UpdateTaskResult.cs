using TaskManager.Domain.DTOs;

namespace TaskManager.Application.UseCases.Task.v1.UpdateTask
{
    public class UpdateTaskResult
    {
        public Guid ModifiedBy { get; set; }
        public DateTime Date { get; set; }
        public required TaskDTO EntityOld { get; set; }
        public required TaskDTO EntityNew { get; set; }
    }
}

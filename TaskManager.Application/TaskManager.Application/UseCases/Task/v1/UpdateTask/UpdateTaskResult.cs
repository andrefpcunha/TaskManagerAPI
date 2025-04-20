using TaskManager.Domain.Entities;

namespace TaskManager.Application.UseCases.Task.v1.UpdateTask
{
    public class UpdateTaskResult
    {
        public Guid ModifiedBy { get; set; }
        public DateTime Date { get; set; }
        public TaskEntity EntityOld { get; set; }
        public TaskEntity EntityNew { get; set; }
    }
}

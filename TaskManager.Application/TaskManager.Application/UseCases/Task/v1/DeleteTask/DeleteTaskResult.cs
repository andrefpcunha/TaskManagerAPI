using TaskManager.Domain.Entities;

namespace TaskManager.Application.UseCases.Task.v1.DeleteTask
{
    public class DeleteTaskResult
    {
        public Guid ModifiedBy { get; set; }
        public DateTime Date { get; set; }
        public TaskEntity EntityOld { get; set; }
        public TaskEntity EntityNew { get; set; }
    }
}

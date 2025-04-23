namespace TaskManager.Application.UseCases.Task.v1.DeleteTask
{
    public class DeleteTaskResult
    {
        public Guid ModifiedBy { get; set; }
        public DateTime Date { get; set; }
        public Domain.Entities.Tasks EntityOld { get; set; }
        public Domain.Entities.Tasks EntityNew { get; set; }
    }
}

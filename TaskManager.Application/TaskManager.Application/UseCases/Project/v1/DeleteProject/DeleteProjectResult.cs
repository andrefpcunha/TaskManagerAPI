using TaskManager.Domain.Entities;

namespace TaskManager.Application.UseCases.Project.v1.DeleteProject
{
    public class DeleteProjectResult
    {
        public Guid ModifiedBy { get; set; }
        public DateTime Date { get; set; }
        public ProjectEntity EntityOld { get; set; }
        public ProjectEntity EntityNew { get; set; }
    }
}

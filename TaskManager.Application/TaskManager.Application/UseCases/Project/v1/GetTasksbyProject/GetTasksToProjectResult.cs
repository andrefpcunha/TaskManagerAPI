using TaskManager.Domain.Entities;

namespace TaskManager.Application.UseCases.Project.v1.GetTasksbyProject
{
    public class GetTasksToProjectResult
    {
        public Guid ProjectId { get; set; }
        public IEnumerable<TaskEntity> Tasks { get; set; }
    }
}

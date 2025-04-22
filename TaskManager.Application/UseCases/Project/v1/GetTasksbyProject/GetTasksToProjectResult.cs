using TaskManager.Domain.DTOs;

namespace TaskManager.Application.UseCases.Project.v1.GetTasksbyProject
{
    public class GetTasksToProjectResult
    {
        public IEnumerable<TaskDTO> Tasks { get; set; }
    }
}

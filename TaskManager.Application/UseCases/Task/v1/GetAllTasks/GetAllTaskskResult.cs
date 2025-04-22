using TaskManager.Domain.DTOs;

namespace TaskManager.Application.UseCases.Task.v1.GetAllTasks
{
    public class GetAllTaskskResult
    {
        public IEnumerable<TaskDTO>? Tasks { get; set; }
    }
}

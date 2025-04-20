using TaskManager.Application.UseCases.Task.v1.UpdateTask;
using TaskManager.Domain.Entities;
using TaskManager.Infra.Interfaces.Services;

namespace TaskManager.Application.Services
{
    public class TaskService : ITaskService
    {
        public async Task<TaskEntity> GetTaskById(Guid id)
        {
            return new TaskEntity
            {
                TaskId = id,
                Name = "Task 1",
                Description = "Description Task 1",
                Priority = Domain.Enums.PriorityEnum.Medium,
                Status = StatusTaskEnum.ToDo
            };
        }

        public async Task<bool> UpdateTask(TaskEntity entity)
        {
            return true;
        }
    }
}

using TaskManager.Application.UseCases.Task.v1.UpdateTask;
using TaskManager.Domain.Entities;

namespace TaskManager.Infra.Interfaces.Services
{
    public interface ITaskService
    {
        Task<TaskEntity> GetTaskById(Guid id);
        Task<bool> UpdateTask(TaskEntity entity);
    }
}

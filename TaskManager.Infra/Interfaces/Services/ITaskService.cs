using TaskManager.Domain.DTOs;

namespace TaskManager.Infra.Interfaces.Services
{
    public interface ITaskService
    {
        Task<bool> GetLimitTaskToproject(int ProjectId);
        Task<Domain.Entities.Tasks> AddTask(Domain.Entities.Tasks entity);
        Task<Domain.Entities.Tasks> GetTaskById(int taskId);
        Task<string> UpdateTaskWithHistoric(Guid UserId, Domain.Entities.Tasks entity, TaskDTO old, TaskDTO modfied);
        Task<string> DeleteTask(int taskId, int projectId, Guid UserId, Domain.Entities.Tasks? old, Domain.Entities.Tasks? modfied);
    }
}

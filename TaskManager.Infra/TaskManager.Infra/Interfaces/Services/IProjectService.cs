using TaskManager.Domain.Entities;

namespace TaskManager.Infra.Interfaces.Services
{
    public interface IProjectService
    {
        Task<bool> GetLimitTaskToproject(Guid ProjectId);
        Task<IEnumerable<ProjectEntity>> GetAllProjectsByUserId(String userId);
        Task<IEnumerable<TaskEntity>> GetTasksByProjectId(Guid projectId);
        Task<ProjectEntity> AddProject(ProjectEntity entity);
        Task<bool> DeleteTaskToProject(Guid projectId, Guid taskId);
        Task<ProjectEntity> GetProjectById(Guid projectId);
        Task<bool> DeleteProject(Guid projectId);

    }
}

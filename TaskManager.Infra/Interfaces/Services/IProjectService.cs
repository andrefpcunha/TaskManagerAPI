using TaskManager.Domain.Entities;

namespace TaskManager.Infra.Interfaces.Services
{
    public interface IProjectService
    {
        Task<IEnumerable<Projects>> GetAllProjectsByUserId(String userId);
        Task<IEnumerable<Domain.Entities.Tasks>?> GetTasksByProjectId(int projectId);
        Task<Projects> AddProject(Projects entity);
        Task<Projects?> GetProjectById(int projectId);
        Task<bool> DeleteProject(Projects entity);

    }
}

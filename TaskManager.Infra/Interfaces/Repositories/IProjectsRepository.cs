using TaskManager.Domain.Entities;
using TaskManager.Domain.Models;

namespace TaskManager.Infra.Interfaces.Repositories
{
    public interface IProjectsRepository : IRepositoryBase<Projects>
    {
        Task<Projects?> GetProjectById(int projectId);
        Task<Projects> GetTasksByProjectId(int projectId);
        Task<List<Projects>> GetListByFilterAsync(ProjectFilter filter);
    }
}

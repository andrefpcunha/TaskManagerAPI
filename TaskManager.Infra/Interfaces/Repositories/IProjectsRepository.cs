using TaskManager.Domain.Entities;
using TaskManager.Domain.Models;

namespace TaskManager.Infra.Interfaces.Repositories
{
    public interface IProjectsRepository : IRepositoryBase<Projects>
    {
        Task<Projects> GetTasksByProjectId(int projectId);
        Task<List<Projects>> GetListByFilterAsync(ProjectFilter filter);
    }
}

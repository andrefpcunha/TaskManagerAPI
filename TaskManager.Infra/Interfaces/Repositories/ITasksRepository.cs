using TaskManager.Domain.Entities;
using TaskManager.Domain.Models;

namespace TaskManager.Infra.Interfaces.Repositories
{
    public interface ITasksRepository : IRepositoryBase<Tasks>
    {
        Task<List<Tasks>> GetListByFilterAsync(TaskFilter filter);
    }
}

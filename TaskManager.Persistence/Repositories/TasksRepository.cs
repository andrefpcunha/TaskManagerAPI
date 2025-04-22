using Microsoft.EntityFrameworkCore;
using TaskManager.Domain.Entities;
using TaskManager.Domain.Enums;
using TaskManager.Domain.Models;
using TaskManager.Infra.Interfaces.Repositories;
using TaskManager.Persistence.Contexts;

namespace TaskManager.Persistence.Repositories
{
    public class TasksRepository(AppDbContext context)
    : RepositoryBase<Domain.Entities.Tasks>(context), ITasksRepository
    {
        public async Task<int> CountByFilterAsync(TaskFilter filter, CancellationToken cancellationToken)
        {
            var query = DbContext.Tasks.AsQueryable();

            query = ApplyFilter(filter, query);

            return await query.CountAsync(cancellationToken);
        }


        public async Task<List<Tasks>> GetListByFilterAsync(TaskFilter filter)
        {
            var query = DbContext.Tasks.AsQueryable();

            query = ApplyFilter(filter, query);

            return await query.ToListAsync(default);
        }


        private static IQueryable<Tasks> ApplyFilter(TaskFilter filter, IQueryable<Tasks> query)
        {
            if (filter.Id > 0)
                query = query.Where(x => x.Id == filter.Id);

            if (filter.ProjectId > 0)
                query = query.Where(x => x.ProjectId == filter.ProjectId);

            if (!string.IsNullOrWhiteSpace(filter.Name))
                query = query.Where(x => EF.Functions.Like(x.Name, $"%{filter.Name}%"));

            if (filter.Priority.HasValue && Enum.IsDefined(typeof(PriorityEnum), filter.Priority))
                query = query.Where(x => x.Priority == filter.Priority);

            if (filter.Status.HasValue && Enum.IsDefined(typeof(StatusTaskEnum), filter.Status))
                query = query.Where(x => x.Status == filter.Status);

            return query;
        }
    }
}

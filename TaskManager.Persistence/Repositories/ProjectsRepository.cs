using Microsoft.EntityFrameworkCore;
using TaskManager.Domain.Entities;
using TaskManager.Domain.Models;
using TaskManager.Infra.Interfaces.Repositories;
using TaskManager.Persistence.Contexts;

namespace TaskManager.Persistence.Repositories
{
    public class ProjectsRepository(AppDbContext context)
    : RepositoryBase<Domain.Entities.Projects>(context), IProjectsRepository
    {
        public async Task<Projects?> GetTasksByProjectId(int projectId)
        {
            return await context.Projects.Where(x => x.Id == projectId)
                        .Include(p => p.Tasks)
                        .FirstOrDefaultAsync();
        }

        public async Task<List<Projects>> GetListByFilterAsync(ProjectFilter filter)
        {
            var query = DbContext.Projects.AsQueryable();

            query = ApplyFilter(filter, query);
            
            if (filter.CurrentPage > 0)
                query = query.Skip((filter.CurrentPage - 1) * filter.PageSize).Take(filter.PageSize);

            return await query.OrderBy(x => x.Id).Include(p => p.Tasks).ToListAsync(default);
        }


        private static IQueryable<Projects> ApplyFilter(ProjectFilter filter, IQueryable<Projects> query)
        {
            if (filter.Id > 0)
                query = query.Where(x => x.Id == filter.Id);

            if (!string.IsNullOrWhiteSpace(filter.UserId))
                query = query.Where(x => x.OwnerUser == filter.UserId);

            return query;
        }
    }
}

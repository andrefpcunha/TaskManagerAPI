using System.Threading;
using TaskManager.Domain.Entities;
using TaskManager.Domain.Models;
using TaskManager.Infra.Interfaces.Repositories;
using TaskManager.Infra.Interfaces.Services;

namespace TaskManager.Application.Services
{
    public class ProjectService : IProjectService
    {
        private readonly IProjectsRepository _projectsRepository;

        public ProjectService(IProjectsRepository projectsRepository)
        {
            _projectsRepository = projectsRepository;
        }

        

        public async Task<IEnumerable<Projects>> GetAllProjectsByUserId(string userId)
        {
            var filter = new ProjectFilter { UserId = userId };
            return await _projectsRepository.GetListByFilterAsync(filter);
        }

        public async Task<IEnumerable<Domain.Entities.Tasks>?> GetTasksByProjectId(int projectId)
        {
            var project = await _projectsRepository.GetTasksByProjectId(projectId);
            var listTasks = new List<Domain.Entities.Tasks>();

            if (project is not null && (project.Tasks is not null && project.Tasks.Any()))
            {
                foreach (var item in project.Tasks)
                {
                    listTasks.Add(item);
                }
                return listTasks;
            }
            else 
                return null;
        }

        public async Task<Projects> AddProject(Projects entity)
        {
            _projectsRepository.Add(entity);
            await _projectsRepository.SaveChangesAsync(default);

            return entity;
        }

        public async Task<Projects?> GetProjectById(int projectId)
        {
            return await _projectsRepository.GetProjectById(projectId);
        }

        public async Task<bool> DeleteProject(Projects entity)
        {
            _projectsRepository.Remove(entity);
            return await _projectsRepository.SaveChangesAsync(default) > 0;
        }
    }
}

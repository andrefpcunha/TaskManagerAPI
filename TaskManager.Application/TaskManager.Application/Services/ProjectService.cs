using TaskManager.Domain.Entities;
using TaskManager.Infra.Interfaces.Services;

namespace TaskManager.Application.Services
{
    public class ProjectService : IProjectService
    {

        public async Task<bool> GetLimitTaskToproject(Guid ProjectId)
        {
            return true;
        }

        public async Task<IEnumerable<ProjectEntity>> GetAllProjectsByUserId(String userId)
        {
            return new List<ProjectEntity>() { new ProjectEntity { Active = true, Name = "Project 1", ProjectId = Guid.NewGuid() } };
        }

        public async Task<IEnumerable<TaskEntity>> GetTasksByProjectId(Guid projectId)
        {
            return new List<TaskEntity>() 
            { 
                new TaskEntity { 
                    TaskId = Guid.NewGuid(), 
                    Name = "Task1", 
                    Description = "Description Task 1",
                    Priority = Domain.Enums.PriorityEnum.Medium,
                    Status = StatusTaskEnum.Doing
                } 
            };
        }

        public async Task<ProjectEntity> AddProject(ProjectEntity entity)
        {
            return new ProjectEntity { ProjectId = Guid.NewGuid(), Name = "Project 11", Active = true, Description = entity.Description, Tasks = null };
        }

        public async Task<bool> DeleteTaskToProject(Guid projectId, Guid taskId)
        {
            return true;
        }

        public async Task<ProjectEntity> GetProjectById(Guid projectId)
        {
            return new ProjectEntity 
            { 
                ProjectId = projectId, 
                Name = "Project 11", 
                Active = true, 
                Description = "Project Description", 
                Tasks =
                [
                    new TaskEntity
                    {
                        TaskId = Guid.NewGuid(),
                        Name = "Task 2",
                        Description = "Description Task 2",
                        Priority= Domain.Enums.PriorityEnum.Medium,
                        Status = StatusTaskEnum.Doing
                    }
                ]
            };
        }

        public async Task<bool> DeleteProject(Guid projectId)
        {
            return true;
        }
    }
}

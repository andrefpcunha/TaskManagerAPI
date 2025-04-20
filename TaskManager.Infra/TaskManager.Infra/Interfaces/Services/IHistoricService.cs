using TaskManager.Domain.Entities;

namespace TaskManager.Infra.Interfaces.Services
{
    public interface IHistoricService
    {
        Task<HistoricEntity<TaskEntity>> RegisterTaskHistoric(Guid UserId, TaskEntity old, TaskEntity modfied);
        Task<HistoricEntity<ProjectEntity>> RegisterProjectHistoric(Guid UserId, ProjectEntity old, ProjectEntity modfied);
    }
}

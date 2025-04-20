using TaskManager.Domain.Entities;
using TaskManager.Infra.Interfaces.Services;

namespace TaskManager.Application.Services
{
    public class HistoricService : IHistoricService 
    {
        public async Task<HistoricEntity<ProjectEntity>> RegisterProjectHistoric(Guid UserId, ProjectEntity old, ProjectEntity modfied)
        {
            return new HistoricEntity<ProjectEntity> { ModifiedBy = UserId, Date = DateTime.Now, EntityOld = old, EntityNew = modfied };
        }

        public async Task<HistoricEntity<TaskEntity>> RegisterTaskHistoric(Guid UserId, TaskEntity old, TaskEntity modfied)
        {
            return new HistoricEntity<TaskEntity> { ModifiedBy = UserId, Date = DateTime.Now, EntityOld = old, EntityNew = modfied };
        }
    }
}

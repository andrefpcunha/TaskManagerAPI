using System.Text.Json;
using TaskManager.Application.UseCases.Task.v1.UpdateTask;
using TaskManager.Domain.DTOs;
using TaskManager.Domain.Models;
using TaskManager.Infra.Interfaces.Repositories;
using TaskManager.Infra.Interfaces.Services;

namespace TaskManager.Application.Services
{
    public class TaskService : ITaskService
    {
        private readonly ITasksRepository _tasksRepository;

        public TaskService(ITasksRepository tasksRepository)
        {
            _tasksRepository = tasksRepository;
        }


        public async Task<bool> GetLimitTaskToproject(int ProjectId)
        {
            var filter = new TaskFilter { ProjectId = ProjectId };
            var total = await _tasksRepository.GetListByFilterAsync(filter);

            if (total is null)
                return false;
            else if (total.Count > 0 && total.Count == filter.LimitTaskInProject)
                return true;
            else
                return false;
        }

        public async Task<Domain.Entities.Tasks> AddTask(Domain.Entities.Tasks entity)
        {
            _tasksRepository.Add(entity);
            await _tasksRepository.SaveChangesAsync(default);

            return entity;
        }

        public async Task<Domain.Entities.Tasks> GetTaskById(int taskId)
        {
            return await _tasksRepository.GetByIdAsync(taskId, default);
        }

        public async Task<string> UpdateTaskWithHistoric(Guid UserId, Domain.Entities.Tasks entity, TaskDTO old, TaskDTO modfied)
        {
            //Cada vez que uma tarefa for atualizada (status, detalhes, etc.), a API deve registrar um histórico de alterações para a tarefa.
            //O histórico de alterações deve incluir informações sobre o que foi modificado, a data da modificação e o usuário que fez a modificação.
            var obj = new HistoricDTO<TaskDTO> { ModifiedBy = UserId, Date = DateTime.Now, EntityOld = old, EntityNew = modfied };
            var historic = JsonSerializer.Serialize(obj);
            entity.Historic = historic;

            _tasksRepository.Update(entity);
            await _tasksRepository.SaveChangesAsync(default);

            return historic;
        }

        public async Task<string> DeleteTask(int taskId, int projectId, Guid UserId, Domain.Entities.Tasks? old, Domain.Entities.Tasks? modfied)
        {
            //Cada vez que uma tarefa for atualizada (status, detalhes, etc.), a API deve registrar um histórico de alterações para a tarefa.
            //O histórico de alterações deve incluir informações sobre o que foi modificado, a data da modificação e o usuário que fez a modificação.
            var obj = new HistoricDTO<Domain.Entities.Tasks> { ModifiedBy = UserId, Date = DateTime.Now, EntityOld = old, EntityNew = modfied };

            return JsonSerializer.Serialize(obj);
        }
    }
}

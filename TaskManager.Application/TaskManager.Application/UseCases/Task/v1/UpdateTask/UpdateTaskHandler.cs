using AutoMapper;
using MediatR;
using TaskManager.Application.Commons.Bases;
using TaskManager.Application.UseCases.Task.v1.NewTask;
using TaskManager.Domain.Entities;
using TaskManager.Infra.Interfaces.Services;

namespace TaskManager.Application.UseCases.Task.v1.UpdateTask
{
    public class UpdateTaskHandler : IRequestHandler<UpdateTaskComand, BaseResponse<UpdateTaskResult>>
    {
        private readonly IMapper _mapper;
        private readonly ITaskService _taskService;
        private readonly IHistoricService _historicService;

        public UpdateTaskHandler(IMapper mapper, ITaskService taskService, IHistoricService historicService)
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _taskService = taskService ?? throw new ArgumentNullException(nameof(ITaskService));
            _historicService = historicService;
        }

        public async Task<BaseResponse<UpdateTaskResult>> Handle(UpdateTaskComand command, CancellationToken cancellationToken)
        {
            var response = new BaseResponse<UpdateTaskResult>() { Succcess = true };
            try
            {
                var entity = await _taskService.GetTaskById(command.TaskId);
                var toUpdate = ConvertToEntity(entity, command);
                if (await _taskService.UpdateTask(toUpdate))
                {
                    //Cada vez que uma tarefa for atualizada (status, detalhes, etc.), a API deve registrar um histórico de alterações para a tarefa.
                    //O histórico de alterações deve incluir informações sobre o que foi modificado, a data da modificação e o usuário que fez a modificação.
                    var result = await _historicService.RegisterTaskHistoric(command.UserId, entity, toUpdate);
                    response.Data = _mapper.Map<UpdateTaskResult>(Adapter(result));
                    response.Message = "Updated successfully!";
                }
                else
                {
                    response.Succcess = false;
                    response.Message = $"Could not update TaskId {command.TaskId}";
                }
            }
            catch (Exception ex)
            {
                response.Succcess = false;
                response.Message  = ex.Message;
            }
            return response;
        }

        private TaskEntity ConvertToEntity(TaskEntity entity, UpdateTaskComand command)
        {
            return new TaskEntity
            {
                TaskId = command.TaskId,
                Name = command.Name,
                Description = command.Comment,
                Priority = entity.Priority,
                Status = command.Status
            };
        }

        private UpdateTaskResult Adapter(HistoricEntity<TaskEntity> entity)
        {
            return new UpdateTaskResult
            {
                ModifiedBy = entity.ModifiedBy,
                Date = entity.Date,
                EntityNew = entity.EntityNew,
                EntityOld = entity.EntityOld
            };
        }
    }
}

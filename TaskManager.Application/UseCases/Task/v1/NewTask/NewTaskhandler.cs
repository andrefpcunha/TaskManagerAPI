using AutoMapper;
using MediatR;
using System.Text.Json;
using TaskManager.Application.Commons.Bases;
using TaskManager.Domain.DTOs;
using TaskManager.Domain.Entities;
using TaskManager.Infra.Interfaces.Services;

namespace TaskManager.Application.UseCases.Task.v1.NewTask
{
    public class NewTaskhandler : IRequestHandler<NewTaskCommand, BaseResponse<NewTaskResult>>
    {
        private readonly IMapper _mapper;
        private readonly ITaskService _taskService;

        public NewTaskhandler(IMapper mapper, ITaskService taskService)
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _taskService = taskService ?? throw new ArgumentNullException(nameof(taskService));
        }

        public async Task<BaseResponse<NewTaskResult>> Handle(NewTaskCommand request, CancellationToken cancellationToken)
        {
            var response = new BaseResponse<NewTaskResult>() { Succcess = true };

            try
            {
                //Cada projeto tem um limite máximo de 20 tarefas.
                var reachedLimitTaskProject = await _taskService.GetLimitTaskToproject(request.ProjectId);

                if (reachedLimitTaskProject)
                {
                    //Tentar adicionar mais tarefas do que o limite deve resultar em um erro.
                    response.Succcess = false;
                    response.Message = $"Limit of Tasks reacehd to the Project {request.ProjectId}!";
                    response.Errors = [new BaseError { ErrorMessage = "Limit reached", PropertyMessage = "Task" }];
                }
                else
                {
                    var entity = new Tasks
                    {
                        ProjectId = request.ProjectId,
                        Name = request.Name,
                        Priority = request.Priority,
                        Comments = request.Comment != null ? [request.Comment] : null,
                        Status = StatusTaskEnum.ToDo
                    };

                    var obj = new HistoricDTO<Domain.Entities.Tasks> { ModifiedBy = request.UserId.Value, Date = DateTime.Now, EntityOld = null, EntityNew = entity };
                    entity.Historic = JsonSerializer.Serialize(obj);

                    var result = await _taskService.AddTask(entity);
                    response.Data = _mapper.Map<NewTaskResult>(Adapter(result));
                    response.Message = "Creadted successfully!";
                }
            }
            catch (Exception ex)
            {
                response.Succcess = false;
                response.Message = ex.Message;
            }
            return response;
        }

        private NewTaskResult Adapter(Domain.Entities.Tasks entity)
        {
            return new NewTaskResult() { TaskId = entity.Id, ProjectId = entity.ProjectId, Name = entity.Name, Priority = entity.Priority, Status = entity.Status };
        }
    }
}

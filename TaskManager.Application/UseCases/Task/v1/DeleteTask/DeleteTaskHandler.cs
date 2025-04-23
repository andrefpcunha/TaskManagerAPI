using AutoMapper;
using MediatR;
using System.Text.Json;
using TaskManager.Application.Commons.Bases;
using TaskManager.Domain.DTOs;
using TaskManager.Infra.Interfaces.Services;

namespace TaskManager.Application.UseCases.Task.v1.DeleteTask
{
    public class DeleteTaskHandler : IRequestHandler<DeleteTaskComand, BaseResponse<DeleteTaskResult>>
    {
        private readonly IMapper _mapper;
        private readonly ITaskService _taskService;

        public DeleteTaskHandler(IMapper mapper, ITaskService taskService, IProjectService projectService)
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _taskService = taskService ?? throw new ArgumentNullException(nameof(ITaskService));
        }

        public async Task<BaseResponse<DeleteTaskResult>> Handle(DeleteTaskComand command, CancellationToken cancellationToken)
        {
            var response = new BaseResponse<DeleteTaskResult>() { Succcess = true };
            try
            {
                var entityOld = await _taskService.GetTaskById(command.TaskId);
                var result = await _taskService.DeleteTask(command.TaskId, command.ProjectId, command.UserId, entityOld, null);

                if (result is not null)
                {
                    response.Data = _mapper.Map<DeleteTaskResult>(Adapter(result));
                    response.Message = "Deleted successfully!";
                }
                else
                {
                    response.Succcess = false;
                    response.Message = $"Could not to delete TaskId {command.TaskId}";
                }
            }
            catch (Exception ex)
            {
                response.Succcess = false;
                response.Message  = ex.Message;
            }
            return response;
        }

        private DeleteTaskResult Adapter(string json)
        {
            var entity = JsonSerializer.Deserialize<HistoricDTO<Domain.Entities.Tasks>>(json); 
            return new DeleteTaskResult
            {
                ModifiedBy = entity.ModifiedBy,
                Date = entity.Date,
                EntityNew = entity.EntityNew,
                EntityOld = entity.EntityOld
            };
        }
    }
}

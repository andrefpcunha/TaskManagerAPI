using AutoMapper;
using MediatR;
using System.Text.Json;
using TaskManager.Application.Commons.Bases;
using TaskManager.Domain.DTOs;
using TaskManager.Infra.Interfaces.Services;

namespace TaskManager.Application.UseCases.Task.v1.UpdateTask
{
    public class UpdateTaskHandler : IRequestHandler<UpdateTaskComand, BaseResponse<UpdateTaskResult>>
    {
        private readonly IMapper _mapper;
        private readonly ITaskService _taskService;

        public UpdateTaskHandler(IMapper mapper, ITaskService taskService)
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _taskService = taskService ?? throw new ArgumentNullException(nameof(ITaskService));
        }

        public async Task<BaseResponse<UpdateTaskResult>> Handle(UpdateTaskComand command, CancellationToken cancellationToken)
        {
            var response = new BaseResponse<UpdateTaskResult>() { Succcess = true };
            try
            {
                var entity = await _taskService.GetTaskById(command.TaskId);

                if (entity == null)
                {
                    response.Succcess = false;
                    response.Message = $"TaskId {command.TaskId} not found";
                }
                else
                {
                    var result = ConvertToDTO(entity, command);
                    var historic = await _taskService.UpdateTaskWithHistoric(command.UserId!.Value, entity, result.Item1, result.Item2);

                    if (historic is not null)
                    {

                        response.Data = _mapper.Map<UpdateTaskResult>(Adapter(historic));
                        response.Message = "Updated successfully!";
                    }
                    else
                    {
                        response.Succcess = false;
                        response.Message = $"Could not to update TaskId {command.TaskId}";
                    }
                }
            }
            catch (Exception ex)
            {
                response.Succcess = false;
                response.Message = ex.Message;
            }
            return response;
        }

        private static (TaskDTO, TaskDTO) ConvertToDTO(Domain.Entities.Tasks entity, UpdateTaskComand command)
        {
            var oldObj = new TaskDTO
            {
                TaskId = entity.Id,
                ProjectId = entity.ProjectId,
                Name = entity.Name,
                Comments = entity.Comments,
                Priority = entity.Priority,
                Status = entity.Status
            };

            var newComent = new List<string>();

            if (entity.Comments == null)
            {
                if (command.Comment is not null)
                {
                    entity.Comments = [];
                    entity.Comments = [.. entity.Comments, command.Comment];
                }
            }
            else if(command.Comment is not null)
                newComent = entity.Comments.Append(command.Comment).ToList();

            
            var newObj = new TaskDTO
                                {
                                    TaskId = entity.Id,
                                    ProjectId = entity.ProjectId,
                                    Name = command.Name,
                                    Comments = newComent,
                                    Priority = entity.Priority,
                                    Status = command.Status
                                };
            entity.Name = command.Name;
            entity.Comments = newComent;
            entity.Status = command.Status;

            return (oldObj, newObj);
        }

        private UpdateTaskResult Adapter(string json)
        {
            var entity = JsonSerializer.Deserialize<HistoricDTO<TaskDTO>>(json);
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

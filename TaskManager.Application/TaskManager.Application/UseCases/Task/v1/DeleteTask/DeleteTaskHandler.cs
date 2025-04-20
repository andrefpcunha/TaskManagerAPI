using AutoMapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManager.Application.Commons.Bases;
using TaskManager.Application.UseCases.Task.v1.UpdateTask;
using TaskManager.Domain.Entities;
using TaskManager.Infra.Interfaces.Services;

namespace TaskManager.Application.UseCases.Task.v1.DeleteTask
{
    public class DeleteTaskHandler : IRequestHandler<DeleteTaskComand, BaseResponse<DeleteTaskResult>>
    {
        private readonly IMapper _mapper;
        private readonly ITaskService _taskService;
        private readonly IProjectService _projectService;
        private readonly IHistoricService _historicService;

        public DeleteTaskHandler(IMapper mapper, ITaskService taskService, IProjectService projectService, IHistoricService historicService)
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _taskService = taskService ?? throw new ArgumentNullException(nameof(ITaskService));
            _projectService = projectService ?? throw new ArgumentNullException(nameof(IProjectService));
            _historicService = historicService;
        }

        public async Task<BaseResponse<DeleteTaskResult>> Handle(DeleteTaskComand command, CancellationToken cancellationToken)
        {
            var response = new BaseResponse<DeleteTaskResult>() { Succcess = true };
            try
            {
                var task = await _taskService.GetTaskById(command.TaskId);

                if (await _projectService.DeleteTaskToProject(command.ProjectId, command.TaskId))
                {
                    //Cada vez que uma tarefa for atualizada (status, detalhes, etc.), a API deve registrar um histórico de alterações para a tarefa.
                    //O histórico de alterações deve incluir informações sobre o que foi modificado, a data da modificação e o usuário que fez a modificação.
                    var result = await _historicService.RegisterTaskHistoric(command.UserId, task, null);
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

        private DeleteTaskResult Adapter(HistoricEntity<TaskEntity> entity)
        {
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

using AutoMapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Text;
using System.Threading.Tasks;
using TaskManager.Application.Commons.Bases;
using TaskManager.Application.UseCases.Task.v1.DeleteTask;
using TaskManager.Domain.Entities;
using TaskManager.Infra.Interfaces.Services;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace TaskManager.Application.UseCases.Project.v1.DeleteProject
{
    public class DeleteProjectHandler : IRequestHandler<DeleteProjectComand, BaseResponse<DeleteProjectResult>>
    {
        private readonly IMapper _mapper;
        private readonly ITaskService _taskService;
        private readonly IProjectService _projectService;
        private readonly IHistoricService _historicService;

        public DeleteProjectHandler(IMapper mapper, ITaskService taskService, IProjectService projectService, IHistoricService historicService)
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _taskService = taskService ?? throw new ArgumentNullException(nameof(ITaskService));
            _projectService = projectService ?? throw new ArgumentNullException(nameof(IProjectService));
            _historicService = historicService;
        }

        public async Task<BaseResponse<DeleteProjectResult>> Handle(DeleteProjectComand command, CancellationToken cancellationToken)
        {
            var response = new BaseResponse<DeleteProjectResult>() { Succcess = true };
            try
            {
                var project = await _projectService.GetProjectById(command.ProjectId);

                if (project.Tasks.Any(x => x.Status.Equals(StatusTaskEnum.ToDo)) || project.Tasks.Any(x => x.Status.Equals(StatusTaskEnum.Doing)))
                {
                    //Um projeto não pode ser removido se ainda houver tarefas pendentes associadas a ele.
                    response.Succcess = false;
                    //Caso o usuário tente remover um projeto com tarefas pendentes, a API deve retornar um erro e sugerir a conclusão ou remoção das tarefas primeiro.
                    response.Message = $"Could not to delete ProjectId {command.ProjectId}, because there is some Pending Task";
                }
                else if (await _projectService.DeleteProject(command.ProjectId))
                {
                    //Cada vez que uma tarefa for atualizada (status, detalhes, etc.), a API deve registrar um histórico de alterações para a tarefa.
                    //O histórico de alterações deve incluir informações sobre o que foi modificado, a data da modificação e o usuário que fez a modificação.
                    var result = await _historicService.RegisterProjectHistoric(command.UserId, project, null);
                    response.Data = _mapper.Map<DeleteProjectResult>(Adapter(result));
                    response.Message = "Deleted successfully!";
                }
                else
                {
                    response.Succcess = false;
                    response.Message = $"Could not to delete ProjectId {command.ProjectId}";
                }
            }
            catch (Exception ex)
            {
                response.Succcess = false;
                response.Message  = ex.Message;
            }
            return response;
        }

        private DeleteProjectResult Adapter(HistoricEntity<ProjectEntity> entity)
        {
            return new DeleteProjectResult
            {
                ModifiedBy = entity.ModifiedBy,
                Date = entity.Date,
                EntityNew = entity.EntityNew,
                EntityOld = entity.EntityOld
            };
        }
    }
}

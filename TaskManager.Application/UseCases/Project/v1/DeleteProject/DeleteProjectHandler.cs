using AutoMapper;
using MediatR;
using TaskManager.Application.Commons.Bases;
using TaskManager.Domain.DTOs;
using TaskManager.Infra.Interfaces.Services;

namespace TaskManager.Application.UseCases.Project.v1.DeleteProject
{
    public class DeleteProjectHandler : IRequestHandler<DeleteProjectComand, BaseResponse<DeleteProjectResult>>
    {
        private readonly IMapper _mapper;
        private readonly ITaskService _taskService;
        private readonly IProjectService _projectService;

        public DeleteProjectHandler(IMapper mapper, ITaskService taskService, IProjectService projectService)
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _taskService = taskService ?? throw new ArgumentNullException(nameof(ITaskService));
            _projectService = projectService ?? throw new ArgumentNullException(nameof(IProjectService));
        }

        public async Task<BaseResponse<DeleteProjectResult>> Handle(DeleteProjectComand command, CancellationToken cancellationToken)
        {
            var response = new BaseResponse<DeleteProjectResult>() { Succcess = true };
            try
            {
                var project = await _projectService.GetProjectById(command.ProjectId);

                if (project is null)
                {
                    response.Errors = [new() { PropertyMessage = "ProjectId", ErrorMessage = "Not found" }];
                    response.Succcess = false;
                    response.Message = $"Could not to delete ProjectId {command.ProjectId}, its not exists";
                }
                else if (project.Tasks != null && 
                    project.Tasks.Any(x => x.Status == StatusTaskEnum.ToDo 
                                        || x.Status == StatusTaskEnum.Doing))
                {
                    //Um projeto não pode ser removido se ainda houver tarefas pendentes associadas a ele.
                    response.Succcess = false;
                    //Caso o usuário tente remover um projeto com tarefas pendentes, a API deve retornar um erro e sugerir a conclusão ou remoção das tarefas primeiro.
                    response.Message = $"Could not to delete ProjectId {command.ProjectId}, because there is some Pending Task";
                }
                else if (await _projectService.DeleteProject(project))
                {
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

        private DeleteProjectResult Adapter(HistoricDTO<Domain.Entities.Projects> entity)
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

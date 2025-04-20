using AutoMapper;
using MediatR;
using TaskManager.Application.Commons.Bases;
using TaskManager.Infra.Interfaces.Services;

namespace TaskManager.Application.UseCases.Task.v1.NewTask
{
    public class NewTaskhandler : IRequestHandler<NewTaskCommand, BaseResponse<NewTaskResult>>
    {
        private readonly IMapper _mapper;
        private readonly IProjectService _projectService;

        public NewTaskhandler(IMapper mapper, IProjectService projectService)
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _projectService = projectService ?? throw new ArgumentNullException(nameof(projectService));
        }

        public async Task<BaseResponse<NewTaskResult>> Handle(NewTaskCommand request, CancellationToken cancellationToken)
        {
            var response = new BaseResponse<NewTaskResult>() { Succcess = true };

            try
            {
                //Cada projeto tem um limite máximo de 20 tarefas.
                var reachedLimitTaskProject = await _projectService.GetLimitTaskToproject(request.ProjectId);

                if (reachedLimitTaskProject)
                {
                    //Tentar adicionar mais tarefas do que o limite deve resultar em um erro.
                    response.Succcess = false;
                    response.Message = $"Limit of Tasks reacehd to the Project {request.ProjectId}!";
                    response.Errors = [new BaseError { ErrorMessage = "Limit reached", PropertyMessage = "Task" }];
                }
                else
                {
                    response.Data = _mapper.Map<NewTaskResult>(new NewTaskResult { TaskId = Guid.NewGuid() });
                    response.Message = "Creadted successfully!";
                }
            }
            catch (Exception ex)
            {
                response.Succcess = false;
                response.Message  = ex.Message;
            }
            return response;
        }
    }
}

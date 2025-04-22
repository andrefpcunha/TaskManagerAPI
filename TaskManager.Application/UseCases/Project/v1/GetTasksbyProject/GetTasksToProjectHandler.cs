using AutoMapper;
using MediatR;
using TaskManager.Application.Commons.Bases;
using TaskManager.Domain.DTOs;
using TaskManager.Infra.Interfaces.Services;

namespace TaskManager.Application.UseCases.Project.v1.GetTasksbyProject
{
    public class GetTasksToProjectHandler : IRequestHandler<GetTasksToProjectQuery, BaseResponse<GetTasksToProjectResult>>
    {
        private readonly IProjectService _projectService;
        private readonly IMapper _mapper;

        public GetTasksToProjectHandler(IProjectService projectService, IMapper mapper)
        {
            _projectService = projectService ?? throw new ArgumentNullException(nameof(projectService));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<BaseResponse<GetTasksToProjectResult>> Handle(GetTasksToProjectQuery request, CancellationToken cancellationToken)
        {
            var response = new BaseResponse<GetTasksToProjectResult>() { Succcess = true };

            try
            {
                var tasks = await _projectService.GetTasksByProjectId(request.ProjectId);

                if (tasks is null)
                {
                    response.Succcess = false;
                    response.Message = $"ProjectId {request.ProjectId} not found or there are no Tasks";
                }
                else
                {
                    response.Data = _mapper.Map<GetTasksToProjectResult>(Adapter(tasks));
                    response.Message = "Returned successfully!";
                }
            }
            catch (Exception ex)
            {
                response.Succcess = false;
                response.Message  = ex.Message;
            }
            return response;
        }


        protected GetTasksToProjectResult Adapter(IEnumerable<Domain.Entities.Tasks> entity)
        {
            var result = new GetTasksToProjectResult { Tasks = [] };
            var list = new List<TaskDTO>();
            foreach (var entityItem in entity)
            {
                var item = new TaskDTO
                {
                    TaskId = entityItem.Id,
                    ProjectId = entityItem.ProjectId,
                    Name = entityItem.Name,
                    Priority = entityItem.Priority,
                    Status = entityItem.Status,
                    Comments = entityItem.Comments
                };

                list.Add(item);
            }
            result.Tasks = list;
            return result;
        }
    }
}

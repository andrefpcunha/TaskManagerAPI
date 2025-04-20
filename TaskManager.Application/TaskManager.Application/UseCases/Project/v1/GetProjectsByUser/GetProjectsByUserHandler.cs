using AutoMapper;
using MediatR;
using TaskManager.Application.Commons.Bases;
using TaskManager.Application.UseCases.Task.v1.NewTask;
using TaskManager.Domain.Entities;
using TaskManager.Infra.Interfaces.Services;

namespace TaskManager.Application.UseCases.Project.v1.GetProjectsByUser
{
    public class GetProjectsByUserHandler : IRequestHandler<GetProjectsByUserQuery, BaseResponse<GetProjectsByUserResult>>
    {
        private readonly IProjectService _projectService;
        private readonly IMapper _mapper;

        public GetProjectsByUserHandler(IProjectService projectService, IMapper mapper)
        {
            _projectService = projectService ?? throw new ArgumentNullException(nameof(projectService));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<BaseResponse<GetProjectsByUserResult>> Handle(GetProjectsByUserQuery request, CancellationToken cancellationToken)
        {
            var response = new BaseResponse<GetProjectsByUserResult>() { Succcess = true };

            try
            {
                var projects = await _projectService.GetAllProjectsByUserId(request.UserId);

                response.Data = _mapper.Map<GetProjectsByUserResult>(Adapter(projects, request.UserId));
                response.Message = "Returned successfully!";
            }
            catch (Exception ex)
            {
                response.Succcess = false;
                response.Message  = ex.Message;
            }
            return response;
        }

        protected GetProjectsByUserResult Adapter(IEnumerable<ProjectEntity> entity, String userId)
        {
            var result = new GetProjectsByUserResult { Projects = [], UserId = userId };
            var list = new List<ProjectDTO>();
            foreach (var entityItem in entity)
            {
                var item = new ProjectDTO
                {
                    ProjectId = entityItem.ProjectId,
                    Name = entityItem.Name,
                    Active = entityItem.Active
                };
                list.Add(item);
            }
            result.Projects = list;
            return result;
        }
    }
}

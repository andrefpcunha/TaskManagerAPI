using MediatR;
using TaskManager.Application.Commons.Bases;

namespace TaskManager.Application.UseCases.Project.v1.GetTasksbyProject
{
    public class GetTasksToProjectQuery : IRequest<BaseResponse<GetTasksToProjectResult>>
    {
        public Guid ProjectId { get; set; }
        public GetTasksToProjectQuery(Guid projectId)
        {
            ProjectId = projectId;
        }
    }
}

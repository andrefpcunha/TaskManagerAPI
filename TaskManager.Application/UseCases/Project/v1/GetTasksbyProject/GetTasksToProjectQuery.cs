using MediatR;
using TaskManager.Application.Commons.Bases;

namespace TaskManager.Application.UseCases.Project.v1.GetTasksbyProject
{
    public class GetTasksToProjectQuery : IRequest<BaseResponse<GetTasksToProjectResult>>
    {
        public int ProjectId { get; set; }
        public GetTasksToProjectQuery(int projectId)
        {
            ProjectId = projectId;
        }
    }
}

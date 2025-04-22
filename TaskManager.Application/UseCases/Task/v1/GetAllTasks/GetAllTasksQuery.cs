using MediatR;
using TaskManager.Application.Commons.Bases;

namespace TaskManager.Application.UseCases.Task.v1.GetAllTasks
{
    public class GetAllTasksQuery : IRequest<BaseResponse<GetAllTaskskResult>>
    {
    }
}

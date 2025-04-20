using MediatR;
using TaskManager.Application.Commons.Bases;

namespace TaskManager.Application.UseCases.Project.v1.GetProjectsByUser
{
    public class GetProjectsByUserQuery : IRequest<BaseResponse<GetProjectsByUserResult>>
    {
        public String UserId { get; set; }
        public GetProjectsByUserQuery(String userId) 
        {
            UserId = userId;
        }
    }
}

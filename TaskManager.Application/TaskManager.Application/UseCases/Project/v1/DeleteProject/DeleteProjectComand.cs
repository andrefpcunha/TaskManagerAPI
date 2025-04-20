using MediatR;
using System.Text.Json.Serialization;
using TaskManager.Application.Commons.Bases;

namespace TaskManager.Application.UseCases.Project.v1.DeleteProject
{
    public class DeleteProjectComand : IRequest<BaseResponse<DeleteProjectResult>>
    {
        [JsonIgnore]
        public Guid UserId { get; set; }
        
        public Guid ProjectId { get; set; }

        public DeleteProjectComand(Guid userId, Guid projectId)
        {
            UserId = userId;
            ProjectId = projectId;
        }
    }
}

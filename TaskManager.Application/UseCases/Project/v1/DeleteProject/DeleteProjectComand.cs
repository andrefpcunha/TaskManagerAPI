using MediatR;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;
using TaskManager.Application.Commons.Bases;

namespace TaskManager.Application.UseCases.Project.v1.DeleteProject
{
    [ExcludeFromCodeCoverage]
    public class DeleteProjectComand : IRequest<BaseResponse<DeleteProjectResult>>
    {
        [JsonIgnore]
        public Guid UserId { get; set; }
        
        public int ProjectId { get; set; }

        public DeleteProjectComand(Guid userId, int projectId)
        {
            UserId = userId;
            ProjectId = projectId;
        }
    }
}

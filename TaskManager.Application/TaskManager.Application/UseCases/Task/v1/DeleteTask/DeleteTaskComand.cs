using MediatR;
using System.Text.Json.Serialization;
using TaskManager.Application.Commons.Bases;

namespace TaskManager.Application.UseCases.Task.v1.DeleteTask
{
    public class DeleteTaskComand : IRequest<BaseResponse<DeleteTaskResult>>
    {
        [JsonIgnore]
        public Guid UserId { get; set; }
        public Guid TaskId { get; set; }
        public Guid ProjectId { get; set; }

        public DeleteTaskComand(Guid userId, Guid taskId, Guid projectId)
        {
            UserId = userId;
            TaskId = taskId;
            ProjectId = projectId;
        }
    }
}

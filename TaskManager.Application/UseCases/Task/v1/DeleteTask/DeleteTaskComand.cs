using MediatR;
using System.Text.Json.Serialization;
using TaskManager.Application.Commons.Bases;

namespace TaskManager.Application.UseCases.Task.v1.DeleteTask
{
    public class DeleteTaskComand : IRequest<BaseResponse<DeleteTaskResult>>
    {
        [JsonIgnore]
        public Guid UserId { get; set; }
        public int TaskId { get; set; }
        public int ProjectId { get; set; }

        public DeleteTaskComand(Guid userId, int taskId, int projectId)
        {
            UserId = userId;
            TaskId = taskId;
            ProjectId = projectId;
        }
    }
}

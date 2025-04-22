using MediatR;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using TaskManager.Application.Commons.Bases;
using TaskManager.Domain.Enums;

namespace TaskManager.Application.UseCases.Task.v1.NewTask
{
    public class NewTaskCommand : IRequest<BaseResponse<NewTaskResult>>
    {
        [JsonIgnore]
        public Guid? UserId { get; set; }

        [MaxLength(20)]
        public required string Name { get; set; }

        public required PriorityEnum Priority { get; set; }

        public required int ProjectId { get; set; }

        //Os usuários podem adicionar comentários a uma tarefa para fornecer informações adicionais.
        [MaxLength(100)]
        public string? Comment { get; set; }
    }
}

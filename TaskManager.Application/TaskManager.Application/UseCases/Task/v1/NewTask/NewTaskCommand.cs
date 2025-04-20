using MediatR;
using System.ComponentModel.DataAnnotations;
using TaskManager.Application.Commons.Bases;
using TaskManager.Domain.Enums;

namespace TaskManager.Application.UseCases.Task.v1.NewTask
{
    public class NewTaskCommand : IRequest<BaseResponse<NewTaskResult>>
    {
        [MaxLength(20)]
        public required string Name { get; set; }

        public required PriorityEnum Priority { get; set; }

        public required Guid ProjectId { get; set; }

        //Os usuários podem adicionar comentários a uma tarefa para fornecer informações adicionais.
        [MaxLength(100)]
        public string? Comment { get; set; }
    }
}

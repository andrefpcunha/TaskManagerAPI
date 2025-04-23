using MediatR;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using TaskManager.Application.Commons.Bases;

namespace TaskManager.Application.UseCases.Task.v1.UpdateTask
{
    public class UpdateTaskComand : IRequest<BaseResponse<UpdateTaskResult>>
    {
        [JsonIgnore]
        public int TaskId {  get; set; }

        [JsonIgnore]
        public Guid? UserId { get; set; }

        [MaxLength(20)]
        [MinLength(5)]
        public string Name { get; set; }

        //Os usuários podem adicionar comentários a uma tarefa para fornecer informações adicionais.
        [MaxLength(100)]
        public string? Comment { get; set; }

        public StatusTaskEnum Status { get; set; }

        //Não é permitido alterar a prioridade de uma tarefa depois que ela foi criada.


        public UpdateTaskComand(Guid? userId, int taskId, string name, string? comment, StatusTaskEnum status)
        {
            UserId = userId;
            TaskId = taskId;
            Name = name;
            Comment = comment;
            Status = status;
        }
    }
}

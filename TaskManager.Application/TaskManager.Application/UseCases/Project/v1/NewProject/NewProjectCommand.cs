using MediatR;
using System.ComponentModel.DataAnnotations;
using TaskManager.Application.Commons.Bases;

namespace TaskManager.Application.UseCases.Project.v1.NewProject
{
    public class NewProjectCommand : IRequest<BaseResponse<NewProjectResult>>
    {

        [MaxLength(20)]
        public required string Name { get; set; }

        public required bool Active { get; set; }

        [MaxLength(100)]
        public required string Description { get; set; }
    }
}

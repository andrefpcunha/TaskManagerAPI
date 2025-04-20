using AutoMapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManager.Application.Commons.Bases;
using TaskManager.Application.UseCases.Task.v1.NewTask;
using TaskManager.Domain.Entities;
using TaskManager.Infra.Interfaces.Services;

namespace TaskManager.Application.UseCases.Project.v1.NewProject
{
    public class NewProjectHandler : IRequestHandler<NewProjectCommand, BaseResponse<NewProjectResult>>
    {
        private readonly IMapper _mapper;
        private readonly IProjectService _projectService;

        public NewProjectHandler(IMapper mapper, IProjectService projectService)
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _projectService = projectService ?? throw new ArgumentNullException(nameof(projectService));
        }

        public async Task<BaseResponse<NewProjectResult>> Handle(NewProjectCommand request, CancellationToken cancellationToken)
        {
            var response = new BaseResponse<NewProjectResult>() { Succcess = true };

            try
            {
                var project = new ProjectEntity() { ProjectId = Guid.NewGuid(), Name = request.Name, Active = request.Active, Description = request.Description };
                var result = await _projectService.AddProject(project);


                response.Data = _mapper.Map<NewProjectResult>(Adapter(result));
                response.Message = "Creadted successfully!";

            }
            catch (Exception ex)
            {
                response.Succcess = false;
                response.Message  = ex.Message;
            }
            return response;
        }

        protected NewProjectResult Adapter(ProjectEntity entity)
        {
            return new NewProjectResult() { ProjectId = entity.ProjectId, Name = entity.Name, Active = entity.Active };
        }
    }
}

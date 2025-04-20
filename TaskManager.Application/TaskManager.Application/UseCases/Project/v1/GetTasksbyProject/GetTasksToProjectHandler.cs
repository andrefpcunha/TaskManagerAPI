using AutoMapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManager.Application.Commons.Bases;
using TaskManager.Application.UseCases.Project.v1.GetProjectsByUser;
using TaskManager.Domain.Entities;
using TaskManager.Infra.Interfaces.Services;

namespace TaskManager.Application.UseCases.Project.v1.GetTasksbyProject
{
    public class GetTasksToProjectHandler : IRequestHandler<GetTasksToProjectQuery, BaseResponse<GetTasksToProjectResult>>
    {
        private readonly IProjectService _projectService;
        private readonly IMapper _mapper;

        public GetTasksToProjectHandler(IProjectService projectService, IMapper mapper)
        {
            _projectService = projectService ?? throw new ArgumentNullException(nameof(projectService));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<BaseResponse<GetTasksToProjectResult>> Handle(GetTasksToProjectQuery request, CancellationToken cancellationToken)
        {
            var response = new BaseResponse<GetTasksToProjectResult>() { Succcess = true };

            try
            {
                var tasks = await _projectService.GetTasksByProjectId(request.ProjectId);

                response.Data = _mapper.Map<GetTasksToProjectResult>(Adapter(tasks, request.ProjectId));
                response.Message = "Returned successfully!";
            }
            catch (Exception ex)
            {
                response.Succcess = false;
                response.Message  = ex.Message;
            }
            return response;
        }


        protected GetTasksToProjectResult Adapter(IEnumerable<TaskEntity> entity, Guid projectId)
        {
            var result = new GetTasksToProjectResult { ProjectId = projectId, Tasks = [] };
            var list = new List<TaskEntity>();
            foreach (var entityItem in entity)
            {
                var item = new TaskEntity
                {
                    TaskId = entityItem.TaskId,
                    Name = entityItem.Name,
                    Description = entityItem.Description,
                    Priority = entityItem.Priority,
                    Status = entityItem.Status
                };
                list.Add(item);
            }
            result.Tasks = list;
            return result;
        }
    }
}

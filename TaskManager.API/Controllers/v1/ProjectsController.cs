using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using TaskManager.Application.UseCases.Project.v1.DeleteProject;
using TaskManager.Application.UseCases.Project.v1.GetProjectsByUser;
using TaskManager.Application.UseCases.Project.v1.GetTasksbyProject;
using TaskManager.Application.UseCases.Project.v1.NewProject;
using TaskManager.Application.UseCases.Task.v1.DeleteTask;


namespace TaskManager.API.Controllers.v1
{
    [ApiController]
    [ApiVersion(1)]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class ProjectsController : ControllerBase
    {
        #region Properties
        private readonly IMediator _mediator;
        private readonly ILogger<ProjectsController> _logger;
        #endregion

        #region Constructor
        public ProjectsController(IMediator mediator, ILogger<ProjectsController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }
        #endregion



        //listar todos os projetos do usuário
        [HttpGet]
        public async Task<ActionResult> GetProjectsByUser([FromQuery] Guid userId)
        {
            _logger.LogInformation("Getting Projects");

            var command = new GetProjectsByUserQuery(userId.ToString());
            var response = await _mediator.Send(command);
            if (response.Succcess)
            {
                _logger.LogInformation($"List of Projects returned to UserId {userId}");
                return Ok(response);
            }

            _logger.LogInformation($"Error to return projects to UserId {userId}");
            return BadRequest(response);
        }



        //visualizar todas as tarefas de um projeto específico
        [HttpGet("{projectId}/tasks")]
        public async Task<ActionResult> GetTasksByProject(int projectId)
        {
            _logger.LogInformation($"Getting Tasks from ProjectId  {projectId}");

            var command = new GetTasksToProjectQuery(projectId);
            var response = await _mediator.Send(command);
            if (response.Succcess)
            {
                _logger.LogInformation($"List of Tasks returned to ProjectId {projectId}");
                return Ok(response);
            }

            _logger.LogInformation($"Error to return projects to ProjectId {projectId}");
            return BadRequest(response);
        }



        //criar um novo projeto
        [HttpPost]
        public async Task<ActionResult> NewProject([FromBody] NewProjectCommand command)
        {
            _logger.LogInformation("Creating a new Task to a Project");

            var response = await _mediator.Send(command);
            if (response.Succcess)
            {
                _logger.LogInformation($"New Project created with ProjectId {response?.Data?.ProjectId}");
                return Ok(response);
            }

            _logger.LogInformation($"Could not create Project!");
            return BadRequest(response);
        }



        //remover um projeto
        [HttpDelete("{projectId}")]
        public async Task<ActionResult> DeleteProject(int projectId)
        {
            _logger.LogInformation($"Deleting ProjectId {projectId}");
            var userId = Guid.NewGuid();

            var command = new DeleteProjectComand(userId, projectId);
            var response = await _mediator.Send(command);
            if (response.Succcess)
            {
                _logger.LogInformation($"ProjectId {projectId} deleted successfully");
                return Ok(response);
            }

            _logger.LogInformation($"Could not deleted ProjectId {projectId}");
            return BadRequest(response);
        }
    }
}

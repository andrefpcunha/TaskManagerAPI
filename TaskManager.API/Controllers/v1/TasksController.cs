using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics.CodeAnalysis;
using TaskManager.Application.UseCases.Task.v1.DeleteTask;
using TaskManager.Application.UseCases.Task.v1.GetAllTasks;
using TaskManager.Application.UseCases.Task.v1.NewTask;
using TaskManager.Application.UseCases.Task.v1.UpdateTask;

namespace TaskManager.API.Controllers.v1
{
    [ApiController]
    [ApiVersion(1)]
    [Produces("application/json")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ExcludeFromCodeCoverage]
    public class TasksController : ControllerBase
    {
        #region Properties
        private readonly IMediator _mediator;
        private readonly ILogger<TasksController> _logger;
        #endregion

        #region Constructor
        public TasksController(IMediator mediator, ILogger<TasksController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }
        #endregion



        // adicionar uma nova tarefa a um projeto
        [HttpPost]
        [ProducesResponseType(typeof(NewTaskResult), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> NewTask([FromBody] NewTaskCommand command)
        {
            _logger.LogInformation("Creating new Task to a Project");
            var userId = Guid.NewGuid();
            command.UserId = userId;
            var response = await _mediator.Send(command);
            if (response.Succcess)
            {
                _logger.LogInformation($"New Task created to ProjectId {command.ProjectId}");
                return Ok(response);
            }

            _logger.LogInformation($"Could not create task for ProjectId {command.ProjectId}");
            return BadRequest(response);
        }


        //atualizar o status ou detalhes de uma tarefa
        [HttpPut("{taskId}")]
        public async Task<ActionResult> UpdateTask(int taskId, [FromBody] UpdateTaskComand request)
        {
            _logger.LogInformation($"Updating TaskId {taskId}");
            var userId = Guid.NewGuid();

            var command = new UpdateTaskComand(userId, taskId, request.Name, request.Comment, request.Status);
            var response = await _mediator.Send(command);
            if (response.Succcess)
            {
                _logger.LogInformation($"TaskId {taskId} updated successfully");
                return Ok(response);
            }

            _logger.LogInformation($"Could not update TaskId {taskId}");
            return BadRequest(response);
        }


        //remover uma tarefa de um projeto
        [HttpDelete("{taskId}/projects/{projectId}")]
        public async Task<ActionResult> DeleteTask(int taskId, int projectId)
        {
            _logger.LogInformation($"Deleting TaskId {taskId} from ProjectId {projectId}");
            var userId = Guid.NewGuid();

            var command = new DeleteTaskComand(userId, taskId, projectId);
            var response = await _mediator.Send(command);
            if (response.Succcess)
            {
                _logger.LogInformation($"TaskId {taskId} deleted successfully");
                return Ok(response);
            }

            _logger.LogInformation($"Could not deleted TaskId {taskId}");
            return BadRequest(response);
        }

        // ocultado no Swagger
        [ApiExplorerSettings(IgnoreApi = true)]
        [HttpGet("getAll")]
        public async Task<ActionResult> GetAllTasks()
        {
            _logger.LogInformation($"Getting all Tasks");

            var command = new GetAllTasksQuery();
            var response = await _mediator.Send(command);
            if (response.Succcess)
            {
                _logger.LogInformation($"Getting all Tasks returned successfully");
                return Ok(response);
            }

            _logger.LogInformation($"Could not Getting all Tasks");
            return BadRequest(response);
        }

        //A API deve fornecer endpoints para gerar relatórios de desempenho, como o número médio de tarefas concluídas por usuário nos últimos 30 dias.
        //Os relatórios devem ser acessíveis apenas por usuários com uma função específica de "gerente"
        // ocultado no Swagger
        [ApiExplorerSettings(IgnoreApi = true)]
        [Authorize(Policy = "gerente")]
        [HttpGet("/Last30d/users/{userId}")]
        public string GetReportTasksLastDays(int userId)
        {
            //
            return "value";
        }
    }
}

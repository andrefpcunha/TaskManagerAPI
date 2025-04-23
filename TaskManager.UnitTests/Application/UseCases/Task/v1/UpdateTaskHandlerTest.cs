using AutoMapper;
using Moq;
using System.Text.Json;
using TaskManager.Application.UseCases.Task.v1.UpdateTask;
using TaskManager.Domain.DTOs;
using TaskManager.Infra.Interfaces.Services;

namespace TaskManager.UnitTests.Application.UseCases.Tasks.v1;

public class UpdateTaskHandlerTest
{
    private readonly Mock<IMapper> _mockMapper;
    private readonly Mock<ITaskService> _mockTaskService;
    private readonly UpdateTaskHandler _handler;

    public UpdateTaskHandlerTest()
    {
        _mockMapper = new Mock<IMapper>();
        _mockTaskService = new Mock<ITaskService>();
        _handler = new UpdateTaskHandler(_mockMapper.Object, _mockTaskService.Object);
    }

    [Fact]
    public async Task Handle_ShouldReturnError_WhenTaskNotFound()
    {
        // Arrange
        var command = new UpdateTaskComand (userId:Guid.NewGuid(), taskId:1, name:"Updated Task", comment:null, status:StatusTaskEnum.Doing);
        _mockTaskService.Setup(service => service.GetTaskById(command.TaskId))
            .ReturnsAsync((Domain.Entities.Tasks)null);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.False(result.Succcess);
        Assert.Equal($"TaskId {command.TaskId} not found", result.Message);
    }

    [Fact]
    public async Task Handle_ShouldUpdateTask_WhenTaskExists()
    {
        // Arrange
        var command = new UpdateTaskComand(userId: Guid.NewGuid(), taskId: 1, name: "Updated Task", comment: null, status: StatusTaskEnum.Doing);
        var taskEntity = new Domain.Entities.Tasks { Id = 1, ProjectId = 1, Name = "Task1", Priority = Domain.Enums.PriorityEnum.High, Status = StatusTaskEnum.ToDo };
        var updatedTaskDTO = new TaskDTO { TaskId = taskEntity.Id, ProjectId = taskEntity.ProjectId, Name = command.Name, Comments = new List<string> { "Old Comment", "New Comment" }, Priority = taskEntity.Priority, Status = command.Status };

        _mockTaskService.Setup(service => service.GetTaskById(command.TaskId))
            .ReturnsAsync(taskEntity);
        _mockTaskService.Setup(service => service.UpdateTaskWithHistoric(command.UserId.Value, taskEntity, It.IsAny<TaskDTO>(), It.IsAny<TaskDTO>()))
            .ReturnsAsync(JsonSerializer.Serialize(new HistoricDTO<TaskDTO> { ModifiedBy = command.UserId.Value, Date = DateTime.Now, EntityOld = updatedTaskDTO, EntityNew = updatedTaskDTO }));
        _mockMapper.Setup(mapper => mapper.Map<UpdateTaskResult>(It.IsAny<object>()))
            .Returns(new UpdateTaskResult { ModifiedBy = command.UserId.Value, Date = DateTime.Now, EntityOld = updatedTaskDTO, EntityNew = updatedTaskDTO });

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.Succcess);
        Assert.Equal("Updated successfully!", result.Message);
        Assert.NotNull(result.Data);
        Assert.Equal(updatedTaskDTO.TaskId, result.Data.EntityNew.TaskId);
    }
}
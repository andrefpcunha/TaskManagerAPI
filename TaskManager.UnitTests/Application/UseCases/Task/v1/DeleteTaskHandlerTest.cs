using AutoMapper;
using Moq;
using System.Text.Json;
using TaskManager.Application.UseCases.Task.v1.DeleteTask;
using TaskManager.Domain.DTOs;
using TaskManager.Domain.Entities;
using TaskManager.Infra.Interfaces.Services;

namespace TaskManager.UnitTests.Application.UseCases.Tasks.v1;

public class DeleteTaskHandlerTest
{
    private readonly Mock<IMapper> _mockMapper;
    private readonly Mock<ITaskService> _mockTaskService;
    private readonly Mock<IProjectService> _mockProjectService;
    private readonly DeleteTaskHandler _handler;

    public DeleteTaskHandlerTest()
    {
        _mockMapper = new Mock<IMapper>();
        _mockTaskService = new Mock<ITaskService>();
        _mockProjectService = new Mock<IProjectService>();
        _handler = new DeleteTaskHandler(_mockMapper.Object, _mockTaskService.Object, _mockProjectService.Object);
    }

    [Fact]
    public async Task Handle_ShouldDeleteTask_WhenTaskExists()
    {
        // Arrange
        var command = new DeleteTaskComand(Guid.NewGuid(), 1, 1);
        var task = new Domain.Entities.Tasks { Id = command.TaskId, ProjectId = command.ProjectId, Name = "abc", Priority = Domain.Enums.PriorityEnum.High, Status = StatusTaskEnum.ToDo };
        var jsonResult = JsonSerializer.Serialize(new HistoricDTO<Domain.Entities.Tasks> { ModifiedBy = command.UserId, Date = DateTime.UtcNow, EntityOld = task });

        _mockTaskService.Setup(service => service.GetTaskById(command.TaskId))
            .ReturnsAsync(task);
        _mockTaskService.Setup(service => service.DeleteTask(command.TaskId, command.ProjectId, command.UserId, task, null))
            .ReturnsAsync(jsonResult);

        var mappedResult = new DeleteTaskResult { ModifiedBy = command.UserId, Date = DateTime.UtcNow, EntityOld = task };
        _mockMapper.Setup(mapper => mapper.Map<DeleteTaskResult>(It.IsAny<object>()))
            .Returns(mappedResult);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.Succcess);
        Assert.Equal("Deleted successfully!", result.Message);
        Assert.NotNull(result.Data);
    }

    [Fact]
    public async Task Handle_ShouldReturnError_WhenTaskDoesNotExist()
    {
        // Arrange
        var command = new DeleteTaskComand(Guid.NewGuid(), 1, 1);

        _mockTaskService.Setup(service => service.GetTaskById(command.TaskId))
            .ReturnsAsync((Domain.Entities.Tasks)null);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.False(result.Succcess);
        Assert.Equal($"Could not to delete TaskId {command.TaskId}", result.Message);
        Assert.Null(result.Data);
    }
}
using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Moq;
using TaskManager.Application.Commons.Bases;
using TaskManager.Application.UseCases.Task.v1.NewTask;
using TaskManager.Domain.DTOs;
using TaskManager.Domain.Entities;
using TaskManager.Infra.Interfaces.Services;
using Xunit;

namespace TaskManager.UnitTests.Application.UseCases.Tasks.v1;
public class NewTaskHandlerTest
{
    private readonly Mock<IMapper> _mockMapper;
    private readonly Mock<ITaskService> _mockTaskService;
    private readonly NewTaskhandler _handler;

    public NewTaskHandlerTest()
    {
        _mockMapper = new Mock<IMapper>();
        _mockTaskService = new Mock<ITaskService>();
        _handler = new NewTaskhandler(_mockMapper.Object, _mockTaskService.Object);
    }

    [Fact]
    public async Task Handle_ShouldReturnError_WhenTaskLimitReached()
    {
        // Arrange
        var command = new NewTaskCommand { ProjectId = 1, Name = "New Task", Priority = Domain.Enums.PriorityEnum.Low, UserId = Guid.NewGuid() };
        _mockTaskService.Setup(service => service.GetLimitTaskToproject(command.ProjectId))
            .ReturnsAsync(true);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.False(result.Succcess);
        Assert.Equal($"Limit of Tasks reacehd to the Project {command.ProjectId}!", result.Message);
    }

    [Fact]
    public async Task Handle_ShouldCreateTask_WhenLimitNotReached()
    {
        // Arrange
        var command = new NewTaskCommand { ProjectId = 1, Name = "New Task", Priority = Domain.Enums.PriorityEnum.Low, UserId = Guid.NewGuid() };
        var taskEntity = new Domain.Entities.Tasks { Id =1, ProjectId = command.ProjectId, Name = command.Name, Priority = command.Priority, Status = StatusTaskEnum.ToDo };
        _mockTaskService.Setup(service => service.GetLimitTaskToproject(command.ProjectId))
            .ReturnsAsync(false);
        _mockTaskService.Setup(service => service.AddTask(It.IsAny<Domain.Entities.Tasks>()))
            .ReturnsAsync(taskEntity);
        _mockMapper.Setup(mapper => mapper.Map<NewTaskResult>(It.IsAny<object>()))
            .Returns(new NewTaskResult { TaskId = taskEntity.Id, ProjectId = taskEntity.ProjectId, Name = taskEntity.Name, Priority = taskEntity.Priority, Status = taskEntity.Status });

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.Succcess);
        Assert.Equal("Creadted successfully!", result.Message);
        Assert.NotNull(result.Data);
        Assert.Equal(taskEntity.Id, result.Data.TaskId);
    }
}
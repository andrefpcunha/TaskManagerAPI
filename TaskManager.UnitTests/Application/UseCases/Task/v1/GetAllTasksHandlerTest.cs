using AutoMapper;
using Moq;
using TaskManager.Application.UseCases.Task.v1.GetAllTasks;
using TaskManager.Domain.DTOs;
using TaskManager.Infra.Interfaces.Repositories;

namespace TaskManager.UnitTests.Application.UseCases.Tasks.v1;

public class GetAllTasksHandlerTest
{
    private readonly Mock<IMapper> _mockMapper;
    private readonly Mock<ITasksRepository> _mockTasksRepository;
    private readonly GetAllTasksHandler _handler;

    public GetAllTasksHandlerTest()
    {
        _mockMapper = new Mock<IMapper>();
        _mockTasksRepository = new Mock<ITasksRepository>();
        _handler = new GetAllTasksHandler(_mockMapper.Object, _mockTasksRepository.Object);
    }

    [Fact]
    public async Task Handle_ShouldReturnAllTasks_WhenTasksExist()
    {
        // Arrange
        var tasks = new List<Domain.Entities.Tasks>
        {
            new() { Id = 1, ProjectId = 1, Name = "Task 1", Priority = Domain.Enums.PriorityEnum.Medium, Status = StatusTaskEnum.ToDo },
            new() { Id = 2, ProjectId = 1, Name = "Task 2", Priority = Domain.Enums.PriorityEnum.Medium, Status = StatusTaskEnum.Done }
        };

        _mockTasksRepository.Setup(repo => repo.GetAllAsync(default))
            .ReturnsAsync(tasks);

        var mappedResult = new GetAllTaskskResult
        {
            Tasks =
            [
                new TaskDTO { TaskId = tasks[0].Id, ProjectId = tasks[0].ProjectId, Name = "Task 1", Priority = Domain.Enums.PriorityEnum.Medium, Status = StatusTaskEnum.ToDo },
                new TaskDTO { TaskId = tasks[1].Id, ProjectId = tasks[1].ProjectId, Name = "Task 2", Priority = Domain.Enums.PriorityEnum.Medium, Status = StatusTaskEnum.Done }
            ]
        };

        _mockMapper.Setup(mapper => mapper.Map<GetAllTaskskResult>(It.IsAny<object>()))
            .Returns(mappedResult);

        var query = new GetAllTasksQuery();

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.True(result.Succcess);
        Assert.Equal("Get successfully!", result.Message);
        Assert.NotNull(result.Data);
    }

    [Fact]
    public async Task Handle_ShouldReturnError_WhenExceptionIsThrown()
    {
        // Arrange
        _mockTasksRepository.Setup(repo => repo.GetAllAsync(default))
            .ThrowsAsync(new Exception("Database error"));

        var query = new GetAllTasksQuery();

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.False(result.Succcess);
        Assert.Equal("Database error", result.Message);
        Assert.Null(result.Data);
    }
}
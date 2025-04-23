using AutoMapper;
using Moq;
using TaskManager.Application.UseCases.Project.v1.GetTasksbyProject;
using TaskManager.Domain.DTOs;
using TaskManager.Domain.Entities;
using TaskManager.Infra.Interfaces.Services;

namespace TaskManager.UnitTests.Application.UseCases.Project.v1;

public class GetTasksToProjectHandlerTest
{
    private readonly Mock<IProjectService> _mockProjectService;
    private readonly Mock<IMapper> _mockMapper;
    private readonly GetTasksToProjectHandler _handler;

    public GetTasksToProjectHandlerTest()
    {
        _mockProjectService = new Mock<IProjectService>();
        _mockMapper = new Mock<IMapper>();
        _handler = new GetTasksToProjectHandler(_mockProjectService.Object, _mockMapper.Object);
    }

    [Fact]
    public async Task Handle_ShouldReturnTasks_WhenProjectExists()
    {
        // Arrange
        var projectId = 1;
        var tasks = new List<Domain.Entities.Tasks>
        {
            new Domain.Entities.Tasks { Id = 1, ProjectId = projectId, Name = "Task 1", Priority = Domain.Enums.PriorityEnum.High, Status = StatusTaskEnum.Doing }
        };

        _mockProjectService.Setup(service => service.GetTasksByProjectId(projectId))
            .ReturnsAsync(tasks);

        var mappedResult = new GetTasksToProjectResult
        {
            Tasks = new List<TaskDTO>
            {
                new TaskDTO { TaskId = tasks[0].Id, ProjectId = projectId, Name = "Task 1", Priority = Domain.Enums.PriorityEnum.High, Status = StatusTaskEnum.Doing }
            }
        };

        _mockMapper.Setup(mapper => mapper.Map<GetTasksToProjectResult>(It.IsAny<object>()))
            .Returns(mappedResult);

        var query = new GetTasksToProjectQuery(projectId);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.True(result.Succcess);
        Assert.Equal("Returned successfully!", result.Message);
        Assert.NotNull(result.Data);
        Assert.Single(result.Data.Tasks);
    }

    [Fact]
    public async Task Handle_ShouldReturnError_WhenProjectDoesNotExist()
    {
        // Arrange
        var projectId = 1;

        _mockProjectService.Setup(service => service.GetTasksByProjectId(projectId))
            .ReturnsAsync((IEnumerable<Domain.Entities.Tasks>)null);

        var query = new GetTasksToProjectQuery(projectId);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.False(result.Succcess);
        Assert.Equal($"ProjectId {projectId} not found or there are no Tasks", result.Message);
        Assert.Null(result.Data);
    }
}
using AutoMapper;
using Moq;
using TaskManager.Application.UseCases.Project.v1.DeleteProject;
using TaskManager.Domain.Entities;
using TaskManager.Infra.Interfaces.Services;

namespace TaskManager.UnitTests.Application.UseCases.Project.v1;
public class DeleteProjectHandlerTest
{
    private readonly Mock<IMapper> _mockMapper;
    private readonly Mock<ITaskService> _mockTaskService;
    private readonly Mock<IProjectService> _mockProjectService;
    private readonly DeleteProjectHandler _deleteProjectHandler;

    public DeleteProjectHandlerTest()
    {
        _mockMapper = new Mock<IMapper>();
        _mockTaskService = new Mock<ITaskService>();
        _mockProjectService = new Mock<IProjectService>();
        _deleteProjectHandler = new DeleteProjectHandler(_mockMapper.Object, _mockTaskService.Object, _mockProjectService.Object);
    }

    [Fact]
    public async Task Handle_ShouldReturnSuccessResponse_WhenProjectIsDeleted()
    {
        // Arrange
        var projectId = 1;
        var project = new Projects { Id = projectId, Name = "Project 1", Description = "xxx", OwnerUser = "7f93b273-8581-4e21-b10b-ca82a26b40a8",  Tasks = null };
        _mockProjectService.Setup(service => service.GetProjectById(projectId))
            .ReturnsAsync(project);
        _mockProjectService.Setup(service => service.DeleteProject(project))
            .ReturnsAsync(true);

        var command = new DeleteProjectComand(Guid.NewGuid(), projectId);

        // Act
        var result = await _deleteProjectHandler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.Succcess);
        Assert.Equal("Deleted successfully!", result.Message);
    }

    [Fact]
    public async Task Handle_ShouldReturnFailureResponse_WhenProjectDoesNotExist()
    {
        // Arrange
        var projectId = 1;
        _mockProjectService.Setup(service => service.GetProjectById(projectId))
            .ReturnsAsync((Projects)null);

        var command = new DeleteProjectComand(Guid.NewGuid(), projectId);

        // Act
        var result = await _deleteProjectHandler.Handle(command, CancellationToken.None);

        // Assert
        Assert.False(result.Succcess);
        Assert.Contains("its not exists", result.Message);
    }

    [Fact]
    public async Task Handle_ShouldReturnFailureResponse_WhenProjectHasPendingTasks()
    {
        // Arrange
        var projectId = 1;
        var project = new Projects
        {
            Id = projectId,
            Name = "Test",
            Description = "Test",
            OwnerUser = "7f93b273-8581-4e21-b10b-ca82a26b40a8",
            Tasks = new List<Tasks>
            {
                new Tasks { Id = 1, ProjectId = 1, Name = "abc", Priority = Domain.Enums.PriorityEnum.Medium, Status = StatusTaskEnum.ToDo }
            }
        };
        _mockProjectService.Setup(service => service.GetProjectById(projectId))
            .ReturnsAsync(project);

        var command = new DeleteProjectComand(Guid.NewGuid(), projectId);

        // Act
        var result = await _deleteProjectHandler.Handle(command, CancellationToken.None);

        // Assert
        Assert.False(result.Succcess);
        Assert.Contains("Pending Task", result.Message);
    }
}

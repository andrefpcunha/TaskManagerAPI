using Moq;
using TaskManager.Application.Services;
using TaskManager.Domain.Entities;
using TaskManager.Domain.Models;
using TaskManager.Infra.Interfaces.Repositories;
using TaskManager.Infra.Interfaces.Services;

namespace TaskManager.UnitTests.Application.Services;
public class ProjectServiceTests
{
    private readonly Mock<IProjectsRepository> _mockProjectsRepository;
    private readonly IProjectService _projectService;

    public ProjectServiceTests()
    {
        _mockProjectsRepository = new Mock<IProjectsRepository>();
        _projectService = new ProjectService(_mockProjectsRepository.Object);
    }

    [Fact]
    public async Task GetAllProjectsByUserId_ShouldReturnProjects_WhenProjectsExist()
    {
        // Arrange
        var userId = "user123";
        var projects = new List<Projects> { new Projects { Id = 1, Name = "Project1", Description = "xxx", OwnerUser = "7f93b273-8581-4e21-b10b-ca82a26b40a8" } };
        _mockProjectsRepository.Setup(repo => repo.GetListByFilterAsync(It.IsAny<ProjectFilter>()))
            .ReturnsAsync(projects);

        // Act
        var result = await _projectService.GetAllProjectsByUserId(userId);

        // Assert
        Assert.NotNull(result);
        Assert.Single(result);
        Assert.Equal("Project1", result.First().Name);
    }

    [Fact]
    public async Task GetTasksByProjectId_ShouldReturnTasks_WhenTasksExist()
    {
        // Arrange
        var projectId = 1;
        var tasks = new List<Tasks> { new Tasks { Id = 1, Name = "Task1" } };
        var project = new Projects { Id = projectId, Name = "Project1", Description = "xxx", OwnerUser = "7f93b273-8581-4e21-b10b-ca82a26b40a8", Tasks = tasks };
        _mockProjectsRepository.Setup(repo => repo.GetTasksByProjectId(projectId))
            .ReturnsAsync(project);

        // Act
        var result = await _projectService.GetTasksByProjectId(projectId);

        // Assert
        Assert.NotNull(result);
        Assert.Single(result);
        Assert.Equal("Task1", result.First().Name);
    }

    [Fact]
    public async Task AddProject_ShouldAddProject()
    {
        // Arrange
        var project = new Projects {Id = 1, Name = "New Project", Description = "xxx", OwnerUser = "7f93b273-8581-4e21-b10b-ca82a26b40a8" };
        _mockProjectsRepository.Setup(repo => repo.Add(It.IsAny<Projects>()));
        _mockProjectsRepository.Setup(repo => repo.SaveChangesAsync(default))
            .ReturnsAsync(1);

        // Act
        var result = await _projectService.AddProject(project);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("New Project", result.Name);
        _mockProjectsRepository.Verify(repo => repo.Add(It.IsAny<Projects>()), Times.Once);
        _mockProjectsRepository.Verify(repo => repo.SaveChangesAsync(default), Times.Once);
    }
}
using AutoMapper;
using Moq;
using TaskManager.Application.UseCases.Project.v1.NewProject;
using TaskManager.Domain.Entities;
using TaskManager.Infra.Interfaces.Services;

namespace TaskManager.UnitTests.Application.UseCases.Project.v1;

public class NewProjectHandlerTest
{
    private readonly Mock<IMapper> _mockMapper;
    private readonly Mock<IProjectService> _mockProjectService;
    private readonly NewProjectHandler _handler;

    public NewProjectHandlerTest()
    {
        _mockMapper = new Mock<IMapper>();
        _mockProjectService = new Mock<IProjectService>();
        _handler = new NewProjectHandler(_mockMapper.Object, _mockProjectService.Object);
    }

    [Fact]
    public async Task Handle_ShouldCreateProject_WhenValidRequest()
    {
        // Arrange
        var command = new NewProjectCommand { Name = "New Project", Active = true, Description = "Description", UserId = Guid.NewGuid() };
        var project = new Projects { Id = 1, Name = command.Name, Active = command.Active, Description = command.Description, OwnerUser = command.UserId.ToString() };

        _mockProjectService.Setup(service => service.AddProject(It.IsAny<Projects>()))
            .ReturnsAsync(project);

        var mappedResult = new NewProjectResult { ProjectId = project.Id, Name = project.Name, Active = project.Active, OwnerUser = project.OwnerUser };
        _mockMapper.Setup(mapper => mapper.Map<NewProjectResult>(It.IsAny<object>()))
            .Returns(mappedResult);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.Succcess);
        Assert.Equal("Creadted successfully!", result.Message);
        Assert.NotNull(result.Data);
        Assert.Equal(project.Id, result.Data.ProjectId);
    }

    [Fact]
    public async Task Handle_ShouldReturnError_WhenExceptionThrown()
    {
        // Arrange
        var command = new NewProjectCommand { Name = "New Project", Active = true, Description = "Description", UserId = Guid.NewGuid() };

        _mockProjectService.Setup(service => service.AddProject(It.IsAny<Projects>()))
            .ThrowsAsync(new Exception("Error creating project"));

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.False(result.Succcess);
        Assert.Equal("Error creating project", result.Message);
        Assert.Null(result.Data);
    }
}
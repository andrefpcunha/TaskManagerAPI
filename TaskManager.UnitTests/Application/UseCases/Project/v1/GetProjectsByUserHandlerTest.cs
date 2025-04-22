using AutoMapper;
using global::TaskManager.Application.UseCases.Project.v1.GetProjectsByUser;
using global::TaskManager.Domain.Entities;
using global::TaskManager.Infra.Interfaces.Services;
using Moq;

namespace TaskManager.UnitTests.Application.UseCases.Project.v1;

public class GetProjectsByUserHandlerTest
{
    private readonly Mock<IProjectService> _mockProjectService;
    private readonly Mock<IMapper> _mockMapper;
    private readonly GetProjectsByUserHandler _handler;

    public GetProjectsByUserHandlerTest()
    {
        _mockProjectService = new Mock<IProjectService>();
        _mockMapper = new Mock<IMapper>();
        _handler = new GetProjectsByUserHandler(_mockProjectService.Object, _mockMapper.Object);
    }

    [Fact]
    public async Task Handle_ShouldReturnProjects_WhenProjectsExist()
    {
        // Arrange
        var userId = "7f93b273-8581-4e21-b10b-ca82a26b40a8";
        var projects = new List<Projects>
        {
            new Projects { Id = 1, Name = "Project1", Active = true, Description = "xxx", OwnerUser = userId }
        };
        var expectedResult = new GetProjectsByUserResult
        {
            UserId = userId,
            Projects = new List<ProjectDTO>
            {
                new ProjectDTO { ProjectId = 1, Name = "Project1", Active = true }
            }
        };

        _mockProjectService.Setup(service => service.GetAllProjectsByUserId(userId))
            .ReturnsAsync(projects);

        _mockMapper.Setup(mapper => mapper.Map<GetProjectsByUserResult>(It.IsAny<GetProjectsByUserResult>()))
            .Returns(expectedResult);

        var query = new GetProjectsByUserQuery(userId);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.True(result.Succcess);
        Assert.Equal("Returned successfully!", result.Message);
        Assert.Equal(expectedResult, result.Data);
    }

    [Fact]
    public async Task Handle_ShouldReturnFailure_WhenExceptionIsThrown()
    {
        // Arrange
        var userId = "user123";
        _mockProjectService.Setup(service => service.GetAllProjectsByUserId(userId))
            .ThrowsAsync(new Exception("Service error"));

        var query = new GetProjectsByUserQuery(userId);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.False(result.Succcess);
        Assert.Equal("Service error", result.Message);
    }
}
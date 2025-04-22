using System.Text.Json;
using Moq;
using TaskManager.Application.Services;
using TaskManager.Domain.DTOs;
using TaskManager.Domain.Entities;
using TaskManager.Domain.Enums;
using TaskManager.Domain.Models;
using TaskManager.Infra.Interfaces.Repositories;

namespace TaskManager.UnitTests.Application.Services;
public class TaskServiceTests
{
    private readonly Mock<ITasksRepository> _mockTasksRepository;
    private readonly TaskService _taskService;

    public TaskServiceTests()
    {
        _mockTasksRepository = new Mock<ITasksRepository>();
        _taskService = new TaskService(_mockTasksRepository.Object);
    }

    [Fact]
    public async Task GetLimitTaskToproject_ShouldReturnTrue_WhenLimitReached()
    {
        // Arrange
        var projectId = 1;
        var tasks = new List<Tasks> { new () { Id = 1, Name = "Task1" },
                                      new () { Id = 2, Name = "Task2" },
                                      new () { Id = 3, Name = "Task3" },
                                      new () { Id = 4, Name = "Task4" },
                                      new () { Id = 5, Name = "Task5" },
                                      new () { Id = 6, Name = "Task6" },
                                      new () { Id = 7, Name = "Task7" },
                                      new () { Id = 8, Name = "Task8" },
                                      new () { Id = 9, Name = "Task9" },
                                      new () { Id = 10, Name = "Task10" },
                                      new () { Id = 11, Name = "Task11" },
                                      new () { Id = 12, Name = "Task12" },
                                      new () { Id = 13, Name = "Task13" },
                                      new () { Id = 14, Name = "Task14" },
                                      new () { Id = 15, Name = "Task15" },
                                      new () { Id = 16, Name = "Task16" },
                                      new () { Id = 17, Name = "Task17" },
                                      new () { Id = 18, Name = "Task18" },
                                      new () { Id = 19, Name = "Task19" },
                                      new () { Id = 20, Name = "Task20" }};
        _mockTasksRepository.Setup(repo => repo.GetListByFilterAsync(It.IsAny<TaskFilter>()))
            .ReturnsAsync(tasks);

        // Act
        var result = await _taskService.GetLimitTaskToproject(projectId);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public async Task AddTask_ShouldAddTask()
    {
        // Arrange
        var task = new Tasks { Id = 1, Name = "New Task" };
        _mockTasksRepository.Setup(repo => repo.Add(It.IsAny<Tasks>()));
        _mockTasksRepository.Setup(repo => repo.SaveChangesAsync(default))
            .ReturnsAsync(1);

        // Act
        var result = await _taskService.AddTask(task);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("New Task", result.Name);
        _mockTasksRepository.Verify(repo => repo.Add(It.IsAny<Tasks>()), Times.Once);
        _mockTasksRepository.Verify(repo => repo.SaveChangesAsync(default), Times.Once);
    }

    [Fact]
    public async Task UpdateTaskWithHistoric_ShouldReturnHistoricString()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var task = new Tasks { Id = 1, Name = "Task1" };
        var oldDto = new TaskDTO { TaskId = 1, ProjectId = 1, Name = "Old Task", Priority = PriorityEnum.Low, Status = StatusTaskEnum.ToDo };
        var modifiedDto = new TaskDTO { TaskId = 1, ProjectId = 1, Name = "Modified Task", Priority = PriorityEnum.Low, Status = StatusTaskEnum.Doing };

        // Act
        var result = await _taskService.UpdateTaskWithHistoric(userId, task, oldDto, modifiedDto);

        // Assert
        Assert.NotNull(result);
        var historic = JsonSerializer.Deserialize<HistoricDTO<TaskDTO>>(result);
        Assert.Equal(userId, historic.ModifiedBy);
        Assert.Equal("Old Task", historic.EntityOld.Name);
        Assert.Equal("Modified Task", historic.EntityNew.Name);
    }
}
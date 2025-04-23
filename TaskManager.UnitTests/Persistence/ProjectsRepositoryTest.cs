using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using TaskManager.Domain.Entities;
using TaskManager.Domain.Models;
using TaskManager.Persistence.Contexts;
using TaskManager.Persistence.Repositories;

namespace TaskManager.UnitTests.Persistence;

public class ProjectsRepositoryTest
{
    private DbContextOptions<AppDbContext> CreateNewContextOptions()
    {
        var serviceProvider = new ServiceCollection()
            .AddEntityFrameworkInMemoryDatabase()
            .BuildServiceProvider();

        var builder = new DbContextOptionsBuilder<AppDbContext>();
        builder.UseInMemoryDatabase("TestDatabase")
               .UseInternalServiceProvider(serviceProvider);

        return builder.Options;
    }

    [Fact]
    public async Task CanGetProjectById()
    {
        // Arrange
        var options = CreateNewContextOptions();
        using (var context = new AppDbContext(options))
        {
            var project = new Projects
            {
                Id = 1,
                Name = "Test Project",
                Description = "bla",
                Active = true,
                OwnerUser = Guid.NewGuid().ToString(),
                Tasks =
                [
                    new () { Id = 1, Name = "Task 1", ProjectId = 1, Priority = Domain.Enums.PriorityEnum.Medium, Status = StatusTaskEnum.ToDo, Historic = "sasa" },
                    new () { Id = 2, Name = "Task 2", ProjectId = 1, Priority = Domain.Enums.PriorityEnum.Low, Status = StatusTaskEnum.Done, Historic = "sasa" },

                ]
            };
            context.Projects.Add(project);
            context.SaveChanges();
        }

        // Act & Assert
        using (var context = new AppDbContext(options))
        {
            var repository = new ProjectsRepository(context);
            var projectFromDb = await repository.GetProjectById(1);

            Assert.NotNull(projectFromDb);
            Assert.Equal(1, projectFromDb.Id);
            Assert.Equal(2, projectFromDb.Tasks.Count);
        }
    }

    [Fact]
    public async Task CanGetListByFilterAsync()
    {
        // Arrange
        var options = CreateNewContextOptions();
        using (var context = new AppDbContext(options))
        {
            context.Projects.AddRange(
                new Projects { Id = 1, Name = "Project 1", OwnerUser = "User1", Description = "BLA" },
                new Projects { Id = 2, Name = "Project 2", OwnerUser = "User2", Description = "BLA2" }
            );
            context.SaveChanges();
        }

        // Act & Assert
        using (var context = new AppDbContext(options))
        {
            var repository = new ProjectsRepository(context);
            var filter = new ProjectFilter { UserId = "User1" };
            var projects = await repository.GetListByFilterAsync(filter);

            Assert.Single(projects);
            Assert.Equal("Project 1", projects.First().Name);
        }
    }
}
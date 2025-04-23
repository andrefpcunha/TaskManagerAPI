using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using TaskManager.Domain.Entities;
using TaskManager.Domain.Enums;
using TaskManager.Domain.Models;
using TaskManager.Persistence.Contexts;
using TaskManager.Persistence.Repositories;

namespace TaskManager.UnitTests.Persistence
{
    public class TasksRepositoryTest
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
        public async Task CanCountByFilterAsync()
        {
            var options = CreateNewContextOptions();
            using (var context = new AppDbContext(options))
            {
                var repository = new TasksRepository(context);
                var tasks = new List<Tasks>
                {
                    new () { Id = 1, Name = "Task 1", ProjectId = 1, Priority = PriorityEnum.High, Status = StatusTaskEnum.Done, Historic = "das" },
                    new () { Id = 2, Name = "Task 2", ProjectId = 1, Priority = PriorityEnum.Low, Status = StatusTaskEnum.ToDo, Historic = "dsd" }
                };

                await repository.AddRangeAsync(tasks);
                await repository.SaveChangesAsync(CancellationToken.None);

                var filter = new TaskFilter { ProjectId = 1 };
                var count = await repository.CountByFilterAsync(filter, CancellationToken.None);

                Assert.Equal(2, count);
            }
        }

        [Fact]
        public async Task CanGetListByFilterAsync()
        {
            var options = CreateNewContextOptions();
            using (var context = new AppDbContext(options))
            {
                var repository = new TasksRepository(context);
                var tasks = new List<Tasks>
                {
                    new () { Id = 1, Name = "Task 1", ProjectId = 1, Priority = PriorityEnum.High, Status = StatusTaskEnum.Done, Historic = "das" },
                    new () { Id = 2, Name = "Task 2", ProjectId = 1, Priority = PriorityEnum.Low, Status = StatusTaskEnum.ToDo, Historic = "fsa" }
                };

                await repository.AddRangeAsync(tasks);
                await repository.SaveChangesAsync(CancellationToken.None);

                var filter = new TaskFilter { ProjectId = 1 };
                var taskList = await repository.GetListByFilterAsync(filter);

                Assert.Equal(2, taskList.Count);
            }
        }
    }
}
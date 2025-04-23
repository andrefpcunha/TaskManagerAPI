using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using TaskManager.Domain.Entities;
using TaskManager.Persistence.Contexts;
using TaskManager.Persistence.Repositories;

namespace TaskManager.UnitTests.Persistence
{
    public class RepositoryBaseTest
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
        public async Task CanAddEntityAsync()
        {
            var options = CreateNewContextOptions();
            using (var context = new AppDbContext(options))
            {
                var repository = new RepositoryBase<Projects>(context);
                var entity = new Projects { Id = 1, Name = "P 1", Active = true, Description = "bdal", OwnerUser = Guid.NewGuid().ToString() };

                await repository.AddAsync(entity, CancellationToken.None);
                await repository.SaveChangesAsync(CancellationToken.None);

                Assert.Equal(1, await context.Set<Projects>().CountAsync());
            }
        }

        [Fact]
        public async Task CanGetEntityByIdAsync()
        {
            var options = CreateNewContextOptions();
            using (var context = new AppDbContext(options))
            {
                var repository = new RepositoryBase<Projects>(context);
                var entity = new Projects { Id = 1, Name = "P 1", Active = true, Description = "bdal", OwnerUser = Guid.NewGuid().ToString() };
                await repository.AddAsync(entity, CancellationToken.None);
                await repository.SaveChangesAsync(CancellationToken.None);

                var retrievedEntity = await repository.GetByIdAsync(1, CancellationToken.None);

                Assert.NotNull(retrievedEntity);
                Assert.Equal(1, retrievedEntity.Id);
            }
        }

        [Fact]
        public async Task CanGetAllEntitiesAsync()
        {
            var options = CreateNewContextOptions();
            using (var context = new AppDbContext(options))
            {
                var repository = new RepositoryBase<Projects>(context);
                var entities = new List<Projects>
                {
                    new Projects { Id = 1, Name = "P 1", Active = true, Description = "bdal", OwnerUser = Guid.NewGuid().ToString() },
                    new Projects { Id = 2, Name = "P 2", Active = false, Description = "bdal2", OwnerUser = Guid.NewGuid().ToString() }
                };

                await repository.AddRangeAsync(entities);
                await repository.SaveChangesAsync(CancellationToken.None);

                var allEntities = await repository.GetAllAsync(CancellationToken.None);

                Assert.Equal(2, allEntities.Count);
            }
        }

        [Fact]
        public async Task CanUpdateEntity()
        {
            var options = CreateNewContextOptions();
            using (var context = new AppDbContext(options))
            {
                var repository = new RepositoryBase<Projects>(context);
                var entity = new Projects { Id = 1, Name = "P 1", Active = true, Description = "bdal", OwnerUser = Guid.NewGuid().ToString() };
                await repository.AddAsync(entity, CancellationToken.None);
                await repository.SaveChangesAsync(CancellationToken.None);

                entity.Name = "Updated Name";
                repository.Update(entity);
                await repository.SaveChangesAsync(CancellationToken.None);

                var updatedEntity = await repository.GetByIdAsync(1, CancellationToken.None);

                Assert.Equal("Updated Name", updatedEntity.Name);
            }
        }

        [Fact]
        public async Task CanRemoveEntity()
        {
            var options = CreateNewContextOptions();
            using (var context = new AppDbContext(options))
            {
                var repository = new RepositoryBase<Projects>(context);
                var entity = new Projects { Id = 1, Name = "P 1", Active = true, Description = "bdal", OwnerUser = Guid.NewGuid().ToString() };
                await repository.AddAsync(entity, CancellationToken.None);
                await repository.SaveChangesAsync(CancellationToken.None);

                repository.Remove(entity);
                await repository.SaveChangesAsync(CancellationToken.None);

                var allEntities = await repository.GetAllAsync(CancellationToken.None);

                Assert.Empty(allEntities);
            }
        }
    }
}
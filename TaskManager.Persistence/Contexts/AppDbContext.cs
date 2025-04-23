using Microsoft.EntityFrameworkCore;
using System.Diagnostics.CodeAnalysis;
using TaskManager.Domain.Entities;

namespace TaskManager.Persistence.Contexts
{
    [ExcludeFromCodeCoverage]
    public class AppDbContext : DbContext
    {
        public DbSet<Projects> Projects => Set<Projects>();
        public DbSet<Domain.Entities.Tasks> Tasks => base.Set<Domain.Entities.Tasks>();

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Projects>(entity =>
            {
                entity.HasKey(p => p.Id);
                entity.HasMany(p => p.Tasks)
                      .WithOne()
                      .HasForeignKey(t => t.ProjectId);
            });


            modelBuilder.Entity<Domain.Entities.Tasks>(entity =>
            {
                entity.HasKey(p => p.Id);
                entity.HasOne<Projects>()
                      .WithMany(p => p.Tasks)
                      .HasForeignKey(t => t.ProjectId);
            });
        }
    }
}
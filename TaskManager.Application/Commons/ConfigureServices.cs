using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using TaskManager.Application.Services;
using TaskManager.Infra.Interfaces.Repositories;
using TaskManager.Infra.Interfaces.Services;
using TaskManager.Persistence.Contexts;
using TaskManager.Persistence.Repositories;

namespace TaskManager.Application.Commons
{
    public static class ConfigureServices
    {
        public static void AddInjectionServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<AppDbContext>(options =>
            options.UseNpgsql(configuration.GetConnectionString("taskmanager"), npgsqlOptions =>
            {
                npgsqlOptions.EnableRetryOnFailure(
                    maxRetryCount: 3, // Número máximo de tentativas
                    maxRetryDelay: TimeSpan.FromSeconds(30), // Tempo máximo de espera entre tentativas
                    errorCodesToAdd: null); // Códigos de erro adicionais para considerar como transitórios
            }));
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(AppDomain.CurrentDomain.GetAssemblies()));
            services.AddAutoMapper(Assembly.GetExecutingAssembly());
            services.AddScoped<IProjectService, ProjectService>();
            services.AddScoped<ITaskService, TaskService>();
        }

        public static void AddInjectionRepositories(this IServiceCollection services)
        {
            services.AddScoped<IProjectsRepository, ProjectsRepository>();
            services.AddScoped<ITasksRepository, TasksRepository>();
        }
    }
}

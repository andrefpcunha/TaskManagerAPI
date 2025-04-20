using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using TaskManager.Application.Services;
using TaskManager.Infra.Interfaces.Services;

namespace TaskManager.Application.Commons
{
    public static class ConfigureServices
    {
        public static void AddInjectionApplication(this IServiceCollection services)
        {
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(AppDomain.CurrentDomain.GetAssemblies()));
            services.AddAutoMapper(Assembly.GetExecutingAssembly());
            services.AddScoped<IProjectService, ProjectService>();
            services.AddScoped<ITaskService, TaskService>();
            services.AddScoped<IHistoricService, HistoricService>();

            //services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

            //services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehaviour<,>));
            //services.AddSingleton(typeof(IPipelineBehavior<,>), typeof(LoggingBehaviour<,>));
            //services.AddSingleton(typeof(IPipelineBehavior<,>), typeof(PerformanceBehaviour<,>));
        }
    }
}

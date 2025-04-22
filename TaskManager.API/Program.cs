using Asp.Versioning;
using Microsoft.EntityFrameworkCore;
using TaskManager.API.Extensions.Middleware;
using TaskManager.Application.Commons;
using TaskManager.Persistence.Contexts;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("taskmanager"), b => b.MigrationsAssembly("TaskManager.Persistence")));

builder.Services.AddApiVersioning(options =>
{
    options.ApiVersionReader = new UrlSegmentApiVersionReader();
}).AddMvc();


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddInjectionServices(builder.Configuration);
builder.Services.AddInjectionRepositories();

var app = builder.Build();


if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.AddMiddleware();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    
    await db.Database.MigrateAsync();
}

app.MapControllers();

app.Run();
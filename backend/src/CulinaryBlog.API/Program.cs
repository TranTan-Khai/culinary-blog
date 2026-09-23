<<<<<<< HEAD
using CulinaryBlog.API.Endpoints;
using CulinaryBlog.Application;
using CulinaryBlog.Infrastructure.Storage;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddApplicationServices();
builder.Services.AddMinioStorage(builder.Configuration);
=======
using CulinaryBlog.Application;
using CulinaryBlog.Infrastructure;
using CulinaryBlog.Infrastructure.Persistence;
using CulinaryBlog.Infrastructure.Persistence.Seed;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Keep local development logs on the console; Windows EventLog may require
// administrator permissions and can mask the original database exception.
builder.Logging.ClearProviders();
builder.Logging.AddConsole();

builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);
>>>>>>> e9fae1ea82f2c42284d778f15aa7220511269534

var app = builder.Build();

// Apply schema migrations on startup, but seed only when explicitly requested.
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<CulinaryBlogDbContext>();
    await dbContext.Database.MigrateAsync();
}

if (args.Contains("--seed", StringComparer.OrdinalIgnoreCase))
{
    await app.Services.SeedCulinaryBlogDataAsync();
    return;
}

app.MapGet("/", () => "Hello World!");

app.MapRecipesEndpoints();

app.Run();

using CulinaryBlog.API.Endpoints;
using CulinaryBlog.API.Middleware;
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

// RFC 7807: mọi lỗi trả về application/problem+json kèm traceId và instance.
builder.Services.AddProblemDetails(options =>
{
    options.CustomizeProblemDetails = context =>
    {
        context.ProblemDetails.Instance ??=
            $"{context.HttpContext.Request.Method} {context.HttpContext.Request.Path}";
    };
});

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

// Đặt đầu pipeline để bắt exception từ mọi middleware/endpoint phía sau.
app.UseMiddleware<GlobalExceptionMiddleware>();
// Response lỗi không có body (vd. 404 do sai route, 405) cũng trả về ProblemDetails.
app.UseStatusCodePages();

app.MapGet("/", () => "Hello World!");
app.MapRecipeEndpoints();
app.MapRecipeIngredientEndpoints();

app.Run();

using CulinaryBlog.API.Endpoints;
using CulinaryBlog.Application;
using CulinaryBlog.Infrastructure.Storage;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddApplicationServices();
builder.Services.AddMinioStorage(builder.Configuration);

var app = builder.Build();

app.MapGet("/", () => "Hello World!");

app.MapRecipesEndpoints();

app.Run();

using System.Reflection;
using CulinaryBlog.Application.Common.Behaviors;
using FluentValidation;
using MediatR;
using CulinaryBlog.Application.Categories.Services;
using CulinaryBlog.Application.Recipes.Services;
using Microsoft.Extensions.DependencyInjection;

namespace CulinaryBlog.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
            cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
        });

        return services;
    }
}

    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<ICategoryService, CategoryService>();
        services.AddScoped<IRecipeService, RecipeService>();
        return services;
    }
}

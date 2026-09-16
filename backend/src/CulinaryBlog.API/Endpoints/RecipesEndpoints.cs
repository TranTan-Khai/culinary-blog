using System.Linq;
using CulinaryBlog.Application.Recipes.Queries.SearchRecipes;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

namespace CulinaryBlog.API.Endpoints;

public static class RecipesEndpoints
{
    public static void MapRecipesEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/v1/recipes");

        group.MapGet("/search", async ([FromQuery(Name = "q")] string? q, 
            [FromQuery] int? page, 
            [FromQuery] int? pageSize, 
            ISender sender) =>
        {
            try
            {
                var query = new SearchRecipesQuery(q ?? string.Empty, page ?? 1, pageSize ?? 10);
                var result = await sender.Send(query);
                return Results.Ok(result);
            }
            catch (ValidationException ex)
            {
                var errors = ex.Errors
                    .GroupBy(e => e.PropertyName, e => e.ErrorMessage)
                    .ToDictionary(g => g.Key, g => g.ToArray());

                return Results.ValidationProblem(errors, statusCode: 422);
            }
        });
    }
}


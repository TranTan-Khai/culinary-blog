using CulinaryBlog.Application.Recipes.Services;

namespace CulinaryBlog.API.Endpoints;

/// <summary>
/// FR-RCP-002: /api/v1/recipes/{slug}
/// </summary>
public static class RecipeEndpoints
{
    public static IEndpointRouteBuilder MapRecipeEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/v1/recipes")
            .WithTags("Recipes");

        group.MapGet("/{slug}", GetBySlugAsync);

        return app;
    }

    private static async Task<IResult> GetBySlugAsync(
        string slug,
        IRecipeService service,
        CancellationToken cancellationToken)
    {
        var recipe = await service.GetBySlugAsync(slug, cancellationToken);
        if (recipe is null)
        {
            return Results.Problem(
                detail: $"Recipe with slug '{slug}' was not found.",
                statusCode: StatusCodes.Status404NotFound);
        }

        // TODO(FR-RCP-002): Draft/Archived chỉ cho Owner/Admin xem (403) khi module Auth hoàn thành.
        return Results.Ok(recipe);
    }
}

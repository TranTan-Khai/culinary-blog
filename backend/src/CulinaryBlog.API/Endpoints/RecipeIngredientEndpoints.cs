using CulinaryBlog.Application.Common.Exceptions;
using CulinaryBlog.Application.Recipes.Models;
using CulinaryBlog.Application.Recipes.Services;

namespace CulinaryBlog.API.Endpoints;

/// <summary>
/// FR-RCP-009: /api/v1/recipes/{recipeId}/ingredients
/// </summary>
public static class RecipeIngredientEndpoints
{
    public static IEndpointRouteBuilder MapRecipeIngredientEndpoints(this IEndpointRouteBuilder app)
    {
        // TODO(FR-RCP-009): RequireAuthorization (Owner/Admin) khi module Auth hoàn thành.
        var group = app.MapGroup("/api/v1/recipes/{recipeId:guid}/ingredients")
            .WithTags("Recipe Ingredients");

        group.MapPost("/", AddAsync);
        group.MapPut("/{ingredientId:guid}", UpdateAsync);
        group.MapDelete("/{ingredientId:guid}", DeleteAsync);

        return app;
    }

    private static Task<IResult> AddAsync(
        Guid recipeId,
        CreateIngredientRequest request,
        IRecipeIngredientService service,
        CancellationToken cancellationToken) =>
        HandleAsync(async () =>
        {
            var ingredient = await service.AddAsync(recipeId, request, cancellationToken);
            return Results.Created(
                $"/api/v1/recipes/{recipeId}/ingredients/{ingredient.Id}",
                ingredient);
        });

    private static Task<IResult> UpdateAsync(
        Guid recipeId,
        Guid ingredientId,
        UpdateIngredientRequest request,
        IRecipeIngredientService service,
        CancellationToken cancellationToken) =>
        HandleAsync(async () =>
            Results.Ok(await service.UpdateAsync(recipeId, ingredientId, request, cancellationToken)));

    private static Task<IResult> DeleteAsync(
        Guid recipeId,
        Guid ingredientId,
        IRecipeIngredientService service,
        CancellationToken cancellationToken) =>
        HandleAsync(async () =>
        {
            await service.DeleteAsync(recipeId, ingredientId, cancellationToken);
            return Results.NoContent();
        });

    /// <summary>
    /// Chuyển exception của Application sang RFC 7807. Tạm thời đặt ở đây cho tới khi
    /// có GlobalExceptionMiddleware dùng chung.
    /// </summary>
    private static async Task<IResult> HandleAsync(Func<Task<IResult>> action)
    {
        try
        {
            return await action();
        }
        catch (ValidationException ex)
        {
            return Results.ValidationProblem(
                ex.Errors,
                statusCode: StatusCodes.Status422UnprocessableEntity);
        }
        catch (NotFoundException ex)
        {
            return Results.Problem(
                detail: ex.Message,
                statusCode: StatusCodes.Status404NotFound);
        }
    }
}

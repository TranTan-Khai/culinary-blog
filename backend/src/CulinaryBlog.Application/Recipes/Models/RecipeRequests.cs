using CulinaryBlog.Domain.Modules.Recipes;

namespace CulinaryBlog.Application.Recipes.Models;

public sealed record RecipeQuery(
    int Page = 1,
    int PageSize = 20,
    Guid? CategoryId = null,
    RecipeDifficulty? Difficulty = null,
    int? MaxCookTime = null,
    string? Sort = null);

public sealed record CreateRecipeRequest(
    string Title,
    string Slug,
    string Description,
    string Instructions,
    Guid CategoryId,
    string AuthorId,
    int PrepTime,
    int CookTime,
    int Servings,
    RecipeDifficulty Difficulty,
    IReadOnlyList<CreateIngredientRequest> Ingredients,
    IReadOnlyList<CreateStepRequest> Steps,
    RecipeStatus Status = RecipeStatus.Draft);

public sealed record CreateIngredientRequest(
    string Name,
    decimal? Quantity,
    string? Unit,
    string? Notes = null);

public sealed record CreateStepRequest(
    string Title,
    string Description,
    int? TimerMinutes = null);

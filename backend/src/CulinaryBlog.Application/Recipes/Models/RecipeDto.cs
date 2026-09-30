using CulinaryBlog.Domain.Modules.Recipes;

namespace CulinaryBlog.Application.Recipes.Models;

public sealed record RecipeListItemDto(
    Guid Id,
    string Title,
    string Slug,
    Guid CategoryId,
    string AuthorId,
    int PrepTime,
    int CookTime,
    int Servings,
    RecipeDifficulty Difficulty,
    RecipeStatus Status);

public sealed record RecipeDetailDto(
    Guid Id,
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
    RecipeStatus Status,
    IReadOnlyList<RecipeIngredientDto> Ingredients,
    IReadOnlyList<RecipeStepDto> Steps);

public sealed record RecipeIngredientDto(
    Guid Id,
    string Name,
    decimal? Quantity,
    string? Unit,
    string? Notes,
    int OrderIndex);

public sealed record RecipeStepDto(
    int StepNumber,
    string Title,
    string Description,
    int? TimerMinutes);

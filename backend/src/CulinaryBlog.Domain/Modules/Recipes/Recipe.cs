using CulinaryBlog.Domain.Common;
using CulinaryBlog.Domain.Modules.Categories;
using CulinaryBlog.Domain.Modules.Identity;

namespace CulinaryBlog.Domain.Modules.Recipes;

/// <summary>
/// Aggregate root. Sở hữu RecipeStep, RecipeIngredient, RecipeImage (child entities)
/// và RecipeNutrition (Owned Entity).
/// </summary>
public class Recipe : BaseEntity
{
    public string Title { get; private set; } = default!;
    public string Slug { get; private set; } = default!;
    public string Description { get; private set; } = default!;
    public string Instructions { get; private set; } = default!;
    public int PrepTime { get; private set; }
    public int CookTime { get; private set; }
    public int Servings { get; private set; }
    public RecipeDifficulty Difficulty { get; private set; }
    public RecipeStatus Status { get; private set; }
    public DateTimeOffset? PublishedAt { get; private set; }

    public Guid CategoryId { get; private set; }
    public Category? Category { get; private set; }

    public string AuthorId { get; private set; } = default!;
    public ApplicationUser? Author { get; private set; }

    public RecipeNutrition Nutrition { get; private set; } = new();

    private readonly List<RecipeStep> _steps = new();
    public IReadOnlyCollection<RecipeStep> Steps => _steps.AsReadOnly();

    private readonly List<RecipeIngredient> _ingredients = new();
    public IReadOnlyCollection<RecipeIngredient> Ingredients => _ingredients.AsReadOnly();

    private readonly List<RecipeImage> _images = new();
    public IReadOnlyCollection<RecipeImage> Images => _images.AsReadOnly();

    private Recipe() { } // EF Core

    public static Recipe Create(
        string title,
        string slug,
        string description,
        string instructions,
        Guid categoryId,
        string authorId,
        int prepTime,
        int cookTime,
        int servings,
        RecipeDifficulty difficulty,
        RecipeStatus status = RecipeStatus.Draft)
    {
        var recipe = new Recipe
        {
            Title = title,
            Slug = slug,
            Description = description,
            Instructions = instructions,
            CategoryId = categoryId,
            AuthorId = authorId,
            PrepTime = prepTime,
            CookTime = cookTime,
            Servings = servings,
            Difficulty = difficulty,
            Status = status
        };

        if (status == RecipeStatus.Published)
        {
            recipe.PublishedAt = DateTimeOffset.UtcNow;
        }

        return recipe;
    }

    public void SetNutrition(RecipeNutrition nutrition) => Nutrition = nutrition;

    public void AddStep(string title, string description, int? timerMinutes = null)
    {
        var stepNumber = _steps.Count + 1;
        _steps.Add(RecipeStep.Create(Id, stepNumber, title, description, timerMinutes));
    }

    /// <summary>
    /// Thêm nguyên liệu. Không truyền orderIndex thì nguyên liệu được xếp cuối danh sách.
    /// </summary>
    public RecipeIngredient AddIngredient(
        string name,
        decimal? quantity,
        string? unit,
        string? notes = null,
        int? orderIndex = null)
    {
        var ingredient = RecipeIngredient.Create(
            Id, name, quantity, unit, orderIndex ?? _ingredients.Count, notes);
        _ingredients.Add(ingredient);
        return ingredient;
    }

    public RecipeIngredient? FindIngredient(Guid ingredientId) =>
        _ingredients.FirstOrDefault(i => i.Id == ingredientId);

    public bool RemoveIngredient(Guid ingredientId)
    {
        var ingredient = FindIngredient(ingredientId);
        return ingredient is not null && _ingredients.Remove(ingredient);
    }

    public void AddImage(string originalUrl, bool isPrimary = false, string? altText = null)
    {
        var orderIndex = _images.Count;
        _images.Add(RecipeImage.Create(Id, originalUrl, isPrimary, orderIndex, altText));
    }

    public void Publish()
    {
        Status = RecipeStatus.Published;
        PublishedAt = DateTimeOffset.UtcNow;
        UpdatedAt = DateTimeOffset.UtcNow;
    }
}

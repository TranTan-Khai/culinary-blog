using CulinaryBlog.Domain.Common;

namespace CulinaryBlog.Domain.Modules.Recipes;

public interface IRecipeRepository : IRepository<Recipe>
{
    Task<Recipe?> GetBySlugWithDetailsAsync(string slug, CancellationToken cancellationToken = default);

    Task<(IReadOnlyList<Recipe> Items, int TotalCount)> GetPagedAsync(
        int page,
        int pageSize,
        Guid? categoryId = null,
        RecipeDifficulty? difficulty = null,
        int? maxCookTime = null,
        string? sort = null,
        CancellationToken cancellationToken = default);

    Task<Recipe?> GetByIdWithIngredientsAsync(Guid id, CancellationToken cancellationToken = default);

    Task<bool> SlugExistsAsync(string slug, CancellationToken cancellationToken = default);
}

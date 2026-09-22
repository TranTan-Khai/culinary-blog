using CulinaryBlog.Application.Common.Models;
using CulinaryBlog.Application.Recipes.Models;

namespace CulinaryBlog.Application.Recipes.Services;

public interface IRecipeService
{
    Task<PagedResult<RecipeListItemDto>> GetPagedAsync(
        RecipeQuery query,
        CancellationToken cancellationToken = default);

    Task<RecipeDetailDto?> GetBySlugAsync(
        string slug,
        CancellationToken cancellationToken = default);

    Task<RecipeDetailDto> CreateAsync(
        CreateRecipeRequest request,
        CancellationToken cancellationToken = default);

    Task PublishAsync(Guid id, CancellationToken cancellationToken = default);
}

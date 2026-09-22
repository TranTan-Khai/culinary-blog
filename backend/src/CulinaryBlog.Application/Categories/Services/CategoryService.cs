using CulinaryBlog.Application.Categories.Models;
using CulinaryBlog.Domain.Common;
using CulinaryBlog.Domain.Modules.Categories;

namespace CulinaryBlog.Application.Categories.Services;

public sealed class CategoryService : ICategoryService
{
    private readonly ICategoryRepository _categories;
    private readonly IUnitOfWork _unitOfWork;

    public CategoryService(ICategoryRepository categories, IUnitOfWork unitOfWork)
    {
        _categories = categories;
        _unitOfWork = unitOfWork;
    }

    public async Task<IReadOnlyList<CategoryDto>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var categories = await _categories.GetAllWithRecipeCountAsync(cancellationToken);
        return categories.Select(Map).ToList();
    }

    public async Task<CategoryDto?> GetBySlugAsync(string slug, CancellationToken cancellationToken = default)
    {
        var category = await _categories.GetBySlugAsync(slug.Trim(), cancellationToken);
        return category is null ? null : Map(category);
    }

    public async Task<CategoryDto> CreateAsync(
        CreateCategoryRequest request,
        CancellationToken cancellationToken = default)
    {
        var name = Required(request.Name, nameof(request.Name));
        var slug = Required(request.Slug, nameof(request.Slug));

        if (await _categories.NameExistsAsync(name, cancellationToken))
            throw new InvalidOperationException($"Category '{name}' already exists.");

        if (await _categories.GetBySlugAsync(slug, cancellationToken) is not null)
            throw new InvalidOperationException($"Category slug '{slug}' already exists.");

        var category = Category.Create(name, slug, request.Description?.Trim(), request.OrderIndex);
        await _categories.AddAsync(category, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Map(category);
    }

    private static CategoryDto Map(Category category) => new(
        category.Id,
        category.Name,
        category.Slug,
        category.Description,
        category.ImageUrl,
        category.OrderIndex);

    private static string Required(string? value, string parameterName) =>
        string.IsNullOrWhiteSpace(value)
            ? throw new ArgumentException("Value is required.", parameterName)
            : value.Trim();
}

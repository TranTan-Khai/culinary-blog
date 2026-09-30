using CulinaryBlog.Domain.Modules.Recipes;

using CulinaryBlog.Domain.Common;

namespace CulinaryBlog.Domain.Modules.Categories;

public class Category : BaseEntity
{
    public string Name { get; private set; } = default!;
    public string Slug { get; private set; } = default!;
    public string? Description { get; private set; }
    public string? ImageUrl { get; private set; }
    public int OrderIndex { get; private set; }

    // Navigation
    private readonly List<Recipe> _recipes = new();
    public IReadOnlyCollection<Recipe> Recipes => _recipes.AsReadOnly();

    private Category() { } // EF Core

    public static Category Create(string name, string slug, string? description = null, int orderIndex = 0)
    {
        return new Category
        {
            Name = name,
            Slug = slug,
            Description = description,
            OrderIndex = orderIndex
        };
    }

    public void Update(string name, string? description)
    {
        Name = name;
        Description = description;
        UpdatedAt = DateTimeOffset.UtcNow;
    }
}

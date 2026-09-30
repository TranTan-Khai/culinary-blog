using CulinaryBlog.Domain.Common;

namespace CulinaryBlog.Domain.Modules.Recipes;

public class RecipeIngredient : BaseEntity
{
    public Guid RecipeId { get; private set; }
    public Recipe? Recipe { get; private set; }

    public string Name { get; private set; } = default!;
    public decimal? Quantity { get; private set; }
    public string? Unit { get; private set; }
    public string? Notes { get; private set; }
    public int OrderIndex { get; private set; }

    private RecipeIngredient() { }

    public static RecipeIngredient Create(Guid recipeId, string name, decimal? quantity, string? unit, int orderIndex, string? notes = null)
    {
        return new RecipeIngredient
        {
            RecipeId = recipeId,
            Name = name,
            Quantity = quantity,
            Unit = unit,
            OrderIndex = orderIndex,
            Notes = notes
        };
    }

    public void Update(string name, decimal? quantity, string? unit, int orderIndex, string? notes = null)
    {
        Name = name;
        Quantity = quantity;
        Unit = unit;
        OrderIndex = orderIndex;
        Notes = notes;
        UpdatedAt = DateTimeOffset.UtcNow;
    }
}

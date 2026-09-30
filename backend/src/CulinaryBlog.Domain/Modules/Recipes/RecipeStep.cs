using CulinaryBlog.Domain.Common;

namespace CulinaryBlog.Domain.Modules.Recipes;

public class RecipeStep : BaseEntity
{
    public Guid RecipeId { get; private set; }
    public Recipe? Recipe { get; private set; }

    public int StepNumber { get; private set; }
    public string Title { get; private set; } = default!;
    public string Description { get; private set; } = default!;
    public int? TimerMinutes { get; private set; }
    public string? ImageUrl { get; private set; }

    private RecipeStep() { }

    public static RecipeStep Create(Guid recipeId, int stepNumber, string title, string description, int? timerMinutes = null)
    {
        return new RecipeStep
        {
            RecipeId = recipeId,
            StepNumber = stepNumber,
            Title = title,
            Description = description,
            TimerMinutes = timerMinutes
        };
    }
}

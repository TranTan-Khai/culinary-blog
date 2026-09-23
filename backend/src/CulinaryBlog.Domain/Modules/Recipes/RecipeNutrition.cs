namespace CulinaryBlog.Domain.Modules.Recipes;

/// <summary>
/// Owned Entity — không có bảng riêng, được nhúng vào bảng Recipes
/// với tiền tố cột "Nutrition_".
/// </summary>
public class RecipeNutrition
{
    public decimal? Calories { get; set; }
    public decimal? Protein { get; set; }
    public decimal? Carbohydrates { get; set; }
    public decimal? Fat { get; set; }
    public decimal? Fiber { get; set; }
    public decimal? Sodium { get; set; }
}

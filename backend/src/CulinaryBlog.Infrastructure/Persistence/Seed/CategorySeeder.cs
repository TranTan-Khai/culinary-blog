using CulinaryBlog.Domain.Modules.Categories;
using Microsoft.Extensions.Logging;

namespace CulinaryBlog.Infrastructure.Persistence.Seed;

/// <summary>
/// Sinh dữ liệu mẫu cho module Categories.
/// Dùng danh sách tên cố định (thay vì Bogus random) vì tên danh mục món ăn
/// cần có nghĩa thực tế, không thể sinh ngẫu nhiên hợp lý được.
/// Yêu cầu Lab 2: tối thiểu 20 categories.
/// </summary>
public class CategorySeeder
{
    private static readonly string[] Names =
    {
        "Món khai vị", "Món chính", "Tráng miệng", "Đồ uống", "Món chay",
        "Món nướng", "Súp", "Salad", "Bánh mì", "Mì - Phở",
        "Hải sản", "Món Âu", "Món Á", "Ăn vặt", "Đồ ăn sáng",
        "Nước sốt & Gia vị", "Bánh ngọt", "Lẩu", "Món hấp", "Món kho",
        "Món chiên", "Đồ ăn nhanh", "Món cho bé", "Món giảm cân"
    };

    private readonly CulinaryBlogDbContext _context;
    private readonly ILogger<CategorySeeder> _logger;

    public CategorySeeder(CulinaryBlogDbContext context, ILogger<CategorySeeder> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<List<Category>> SeedAsync()
    {
        var categories = new List<Category>();
        var orderIndex = 0;

        foreach (var name in Names)
        {
            var slug = ToSlug(name);
            var category = Category.Create(name, slug, $"Các công thức thuộc danh mục {name}.", orderIndex++);
            categories.Add(category);
        }

        await _context.Categories.AddRangeAsync(categories);
        await _context.SaveChangesAsync();

        _logger.LogInformation("Category seed: đã tạo {Count} category.", categories.Count);
        return categories;
    }

    public static string ToSlug(string input)
    {
        var normalized = input.Trim().ToLowerInvariant();
        var noAccent = RemoveDiacritics(normalized);
        noAccent = noAccent.Replace(" - ", "-").Replace(" & ", "-");
        var slug = System.Text.RegularExpressions.Regex.Replace(noAccent, @"[^a-z0-9\s-]", "");
        slug = System.Text.RegularExpressions.Regex.Replace(slug, @"\s+", "-").Trim('-');
        return slug;
    }

    private static string RemoveDiacritics(string text)
    {
        var normalized = text.Normalize(System.Text.NormalizationForm.FormD);
        var sb = new System.Text.StringBuilder();
        foreach (var c in normalized)
        {
            var category = System.Globalization.CharUnicodeInfo.GetUnicodeCategory(c);
            if (category != System.Globalization.UnicodeCategory.NonSpacingMark)
                sb.Append(c);
        }
        return sb.ToString().Normalize(System.Text.NormalizationForm.FormC).Replace('đ', 'd').Replace('Đ', 'D');
    }
}

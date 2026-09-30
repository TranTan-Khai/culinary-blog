using Bogus;
using CulinaryBlog.Domain.Modules.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace CulinaryBlog.Infrastructure.Persistence.Seed;

/// <summary>
/// Sinh dữ liệu mẫu cho module Identity: 5 tác giả (Author) mẫu.
/// Dùng UserManager để password được hash đúng chuẩn (PBKDF2), không insert thẳng qua DbContext.
/// </summary>
public class IdentitySeeder
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly ILogger<IdentitySeeder> _logger;

    public IdentitySeeder(
        UserManager<ApplicationUser> userManager,
        RoleManager<IdentityRole> roleManager,
        ILogger<IdentitySeeder> logger)
    {
        _userManager = userManager;
        _roleManager = roleManager;
        _logger = logger;
    }

    public async Task<List<ApplicationUser>> SeedAuthorsAsync(int count = 6)
    {
        foreach (var role in new[] { "Admin", "Author" })
        {
            if (!await _roleManager.RoleExistsAsync(role))
            {
                await _roleManager.CreateAsync(new IdentityRole(role));
            }
        }

        var faker = new Faker("vi");
        var authors = new List<ApplicationUser>();

        for (var i = 0; i < count; i++)
        {
            var fullName = faker.Name.FullName();
            var userName = $"author{i + 1}";
            var email = $"author{i + 1}@culinaryblog.local";

            var user = new ApplicationUser
            {
                UserName = userName,
                Email = email,
                EmailConfirmed = true,
                DisplayName = fullName,
                Bio = faker.Lorem.Sentence(12),
                IsActive = true,
                CreatedAt = DateTimeOffset.UtcNow
            };

            var result = await _userManager.CreateAsync(user, "Author@123");
            if (!result.Succeeded)
            {
                _logger.LogWarning("Không tạo được user {Email}: {Errors}", email,
                    string.Join(", ", result.Errors.Select(e => e.Description)));
                continue;
            }

            var role = i == 0 ? "Admin" : "Author"; // user đầu tiên làm Admin
            await _userManager.AddToRoleAsync(user, role);

            authors.Add(user);
        }

        _logger.LogInformation("Identity seed: đã tạo {Count} author.", authors.Count);
        return authors;
    }
}

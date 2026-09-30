using CulinaryBlog.Domain.Modules.Identity;
using CulinaryBlog.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace CulinaryBlog.Infrastructure.Modules.Identity;

public class RefreshTokenRepository : IRefreshTokenRepository
{
    private readonly CulinaryBlogDbContext _context;

    public RefreshTokenRepository(CulinaryBlogDbContext context) => _context = context;

    public async Task<RefreshToken?> GetByTokenHashAsync(string tokenHash, CancellationToken cancellationToken = default) =>
        await _context.RefreshTokens.FirstOrDefaultAsync(t => t.TokenHash == tokenHash, cancellationToken);

    public async Task AddAsync(RefreshToken token, CancellationToken cancellationToken = default) =>
        await _context.RefreshTokens.AddAsync(token, cancellationToken);

    public void Update(RefreshToken token) => _context.RefreshTokens.Update(token);
}

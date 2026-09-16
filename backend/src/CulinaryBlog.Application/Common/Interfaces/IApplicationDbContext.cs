using System.Threading;
using System.Threading.Tasks;
using CulinaryBlog.Domain.Recipes.Entities;
using Microsoft.EntityFrameworkCore;

namespace CulinaryBlog.Application.Common.Interfaces;

public interface IApplicationDbContext
{
    DbSet<Recipe> Recipes { get; }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}


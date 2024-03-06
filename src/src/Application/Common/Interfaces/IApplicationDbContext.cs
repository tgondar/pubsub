using src.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace src.Application.Common.Interfaces;

public interface IApplicationDbContext
{
    DbSet<src.Domain.Entities.Products> Product { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}

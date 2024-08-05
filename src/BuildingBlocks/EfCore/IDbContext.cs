using Microsoft.EntityFrameworkCore.Infrastructure;

namespace Argo.MD.BuildingBlocks.EfCore
{
    /// <summary>
    /// Abstraction of DbContext to be used in BuildingBlocks
    /// </summary>
    public interface IDbContext
    {
        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
        DatabaseFacade Database { get; }
    }
}

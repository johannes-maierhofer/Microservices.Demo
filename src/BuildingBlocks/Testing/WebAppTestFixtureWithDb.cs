using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Respawn;

namespace BuildingBlocks.Testing
{
    public class WebAppTestFixtureWithDb<TEntryPoint, TDbContext> : WebAppTestFixture<TEntryPoint>
        where TEntryPoint : class
        where TDbContext : DbContext
    {
        private Respawner? _respawner;
        private string _dbConnectionString = string.Empty;

        public override async Task InitializeAsync()
        {
            await base.InitializeAsync();
            
            await ExecuteScopeAsync(async provider =>
            {
                var dbContext = provider.GetRequiredService<TDbContext>();
                await dbContext.Database.MigrateAsync();
                _dbConnectionString = dbContext.Database.GetConnectionString() ?? string.Empty;
                _respawner = await Respawner.CreateAsync(_dbConnectionString);
                await SeedData(dbContext);
            });
        }

        public override async Task DisposeAsync()
        {
            if (_respawner != null)
                await _respawner.ResetAsync(_dbConnectionString);
            await ReseedData();
        }

        public async Task ReseedData()
        {
            if (_respawner != null)
                await _respawner.ResetAsync(_dbConnectionString);

            await ExecuteScopeAsync(async provider =>
            {
                var dbContext = provider.GetRequiredService<TDbContext>();
                await SeedData(dbContext);
            });
        }

        protected virtual Task SeedData(TDbContext dbContext)
        {
            return Task.CompletedTask;
        }
    }
}

using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace BuildingBlocks.Testing
{
    public class DbContextTestFixture<TEntryPoint, TDbContext> : IAsyncLifetime
        where TEntryPoint : class
        where TDbContext : DbContext
    {
        private readonly WebApplicationFactory<TEntryPoint> _factory;

        public DbContextTestFixture()
        {
            _factory = new WebApplicationFactory<TEntryPoint>();
        }

        public async Task InitializeAsync()
        {
            using var scope = _factory.Services.CreateScope();

            var dbContext = scope.ServiceProvider.GetRequiredService<TDbContext>();
            await dbContext.Database.EnsureDeletedAsync();
            await dbContext.Database.EnsureCreatedAsync();

            await SeedDatabase(dbContext);
        }

        public Task DisposeAsync()
        {
            return Task.CompletedTask;
        }

        protected virtual Task SeedDatabase(TDbContext dbContext)
        {
            return Task.CompletedTask;
        }
    }
}
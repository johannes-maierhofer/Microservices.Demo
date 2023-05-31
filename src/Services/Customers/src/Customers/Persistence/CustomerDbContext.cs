using BuildingBlocks.EfCore.Conventions;
using Customers.Model;
using Microsoft.EntityFrameworkCore;

namespace Customers.Persistence
{
    public class CustomerDbContext : DbContext
    {
        public CustomerDbContext(DbContextOptions<CustomerDbContext> options) 
            : base(options)
        {
        }

        public DbSet<Customer> Customers => Set<Customer>();

        protected override void ConfigureConventions(
            ModelConfigurationBuilder configurationBuilder)
        {
            // per default string fields will have a length of 200 chars
            configurationBuilder.Conventions
                .Add(_ => new MaxStringLength200Convention());

            base.ConfigureConventions(configurationBuilder);
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.ApplyConfigurationsFromAssembly(typeof(CustomerDbContext).Assembly);
        }
    }
}

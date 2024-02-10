using BuildingBlocks.EfCore;
using Customers.Domain.CustomerAggregate;
using MassTransit;
using Microsoft.EntityFrameworkCore;

namespace Customers.Persistence
{
    public class CustomerDbContext : DbContext, IDbContext
    {
        public CustomerDbContext(DbContextOptions<CustomerDbContext> options) 
            : base(options)
        {
        }

        public DbSet<Customer> Customers => Set<Customer>();

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.ApplyConfigurationsFromAssembly(typeof(CustomerDbContext).Assembly);

            // MassTransit Outbox
            builder.AddInboxStateEntity(b =>
            {
                b.ToTable("InboxState", "MassTransit");
            });
            builder.AddOutboxMessageEntity(b =>
            {
                b.ToTable("OutboxMessage", "MassTransit");
            });
            builder.AddOutboxStateEntity(b =>
            {
                b.ToTable("OutboxState", "MassTransit");
            });
        }
    }
}

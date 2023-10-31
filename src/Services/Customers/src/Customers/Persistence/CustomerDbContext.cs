using BuildingBlocks.EfCore.Conventions;
using Customers.Domain.Customers;
using MassTransit;
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

            // MassTransit Outbox
            builder.AddInboxStateEntity(b =>
            {
                b.ToTable("InboxState", "MassTransit");
            });
            builder.AddOutboxMessageEntity(b =>
            {
                b.ToTable("OutboxMessage", "MassTransit");
                b.Property(p => p.Body).HasMaxLength(int.MaxValue);
                b.Property(p => p.Headers).HasMaxLength(int.MaxValue);
                b.Property(p => p.Properties).HasMaxLength(int.MaxValue);
                b.Property(p => p.MessageType).HasMaxLength(512);
            });
            builder.AddOutboxStateEntity(b =>
            {
                b.ToTable("OutboxState", "MassTransit");
            });
        }
    }
}

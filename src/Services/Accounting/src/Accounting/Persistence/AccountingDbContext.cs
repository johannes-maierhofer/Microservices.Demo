using Argo.MD.Accounting.Domain.CustomerAggregate;
using Argo.MD.BuildingBlocks.EfCore;
using MassTransit;
using Microsoft.EntityFrameworkCore;

namespace Argo.MD.Accounting.Persistence;

public class AccountingDbContext : DbContext, IDbContext
{
    public AccountingDbContext(DbContextOptions<AccountingDbContext> options) 
        : base(options)
    {
    }

    public DbSet<Customer> Customers => Set<Customer>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.ApplyConfigurationsFromAssembly(typeof(AccountingDbContext).Assembly);

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
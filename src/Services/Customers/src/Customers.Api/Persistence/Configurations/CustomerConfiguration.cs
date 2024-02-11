using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Customers.Api.Persistence.Configurations;

public class CustomerConfiguration : IEntityTypeConfiguration<Domain.CustomerAggregate.Customer>
{
    public void Configure(EntityTypeBuilder<Domain.CustomerAggregate.Customer> builder)
    {
        builder.ToTable("Customers");
        builder.HasKey(c => c.Id);

        builder.Property(c => c.FirstName).HasMaxLength(200);
        builder.Property(c => c.LastName).HasMaxLength(200);
        builder.Property(c => c.EmailAddress).HasMaxLength(320);
    }
}
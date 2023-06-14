using BuildingBlocks.Core.Domain;

namespace Customers.Domain
{
    public class Customer : Entity<Guid>
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string EmailAddress { get; set; } = string.Empty;
    }
}

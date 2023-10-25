using BuildingBlocks.Core.Domain;

namespace Customers.Domain.Customers
{
    public class Customer : Entity<Guid>
    {
        public string FirstName { get; private set; }
        public string LastName { get; private set; }
        public string EmailAddress { get; private set; }

        private Customer(
            Guid id,
            string firstName,
            string lastName,
            string emailAddress)
        {
            Id = id;
            FirstName = firstName;
            LastName = lastName;
            EmailAddress = emailAddress;
        }

        public static Customer Create(
            string firstName,
            string lastName,
            string emailAddress)
        {
            var customer = new Customer(
                Guid.NewGuid(),
                firstName,
                lastName,
                emailAddress);

            customer.AddDomainEvent(new CustomerCreatedEvent(customer));

            return customer;
        }

#pragma warning disable CS8618
        private Customer()
        {
        }
#pragma warning restore CS8618
    }
}

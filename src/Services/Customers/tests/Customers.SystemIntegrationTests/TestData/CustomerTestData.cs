using Customers.Domain.CustomerAggregate;

namespace Customers.SystemIntegrationTests.TestData
{
    public static class CustomerTestData
    {
        public static Customer TestCustomer { get; } = CreateTestCustomer();

        private static Customer CreateTestCustomer()
        {
            var customer = Customer.Create(
                "Luciano",
                "Pavarotti",
                "luciano.pavarotto@scala.it"
            );
            customer.ClearDomainEvents();
            return customer;
        }
    }
}

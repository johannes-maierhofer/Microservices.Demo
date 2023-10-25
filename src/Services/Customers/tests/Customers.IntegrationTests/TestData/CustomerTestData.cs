using Customers.Domain.Customers;

namespace Customers.IntegrationTests.TestData
{
    public static class CustomerTestData
    {
        public static Customer TestCustomer => CreateTestCustomer();

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

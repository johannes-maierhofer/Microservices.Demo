using Customers.Model;

namespace Customers.IntegrationTests.TestData
{
    public static class CustomerTestData
    {
        public static Customer TestCustomer => new()
        {
            Id = new Guid("A1DF1FE0-7431-4585-A7B6-15DE49B8F5CC"),
            FirstName = "Luciano",
            LastName = "Pavarotti",
            EmailAddress = "luciano.pavarotto@scala.it"
        };
    }
}

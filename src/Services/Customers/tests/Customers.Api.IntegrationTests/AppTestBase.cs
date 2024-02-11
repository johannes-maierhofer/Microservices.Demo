using Customers.SystemIntegrationTests.Clients;

namespace Customers.Api.IntegrationTests
{
    [Collection("App")]
    public class AppTestBase : IAsyncLifetime
    {
        protected AppTestFixture Fixture { get; }

        protected AppTestBase(AppTestFixture fixture)
        {
            Fixture = fixture;
        }

        protected CustomerClient CreateCustomerClient()
        {
            var client = Fixture.Factory.CreateClient();
            return new CustomerClient(client.BaseAddress?.ToString(), client);
        }

        public Task InitializeAsync()
        {
            // reset db before each test run
            return Fixture.ReseedData();
        }

        public Task DisposeAsync()
        {
            return Task.CompletedTask;
        }
    }
}

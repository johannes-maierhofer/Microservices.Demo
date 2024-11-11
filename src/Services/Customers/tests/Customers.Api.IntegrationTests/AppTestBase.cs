using Argo.MD.Customers.Api.Client;

namespace Argo.MD.Customers.Api.IntegrationTests
{
    [Collection("App")]
    public class AppTestBase : IAsyncLifetime
    {
        protected AppTestFixture Fixture { get; }

        protected AppTestBase(AppTestFixture fixture)
        {
            Fixture = fixture;
        }

        protected CustomerApiClient CreateCustomerApiClient()
        {
            var client = Fixture.Factory.CreateClient();
            return new CustomerApiClient(client);
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

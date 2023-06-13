namespace Customers.IntegrationTests
{
    [Collection("App")]
    public class AppReadTestBase
    {
        protected AppTestFixture Fixture { get; }

        protected AppReadTestBase(AppTestFixture fixture)
        {
            Fixture = fixture;
        }
    }

    [Collection("App")]
    public class AppWriteTestBase : IAsyncLifetime
    {
        protected AppTestFixture Fixture { get; }

        protected AppWriteTestBase(AppTestFixture fixture)
        {
            Fixture = fixture;
        }

        public Task InitializeAsync()
        {
            return Task.CompletedTask;
        }

        public Task DisposeAsync()
        {
            // reset db after test run
            return Fixture.ReseedData();
        }
    }
}

using Customers.Features.Queries.GetCustomer;
using Customers.IntegrationTests.TestData;
using FluentAssertions;

namespace Customers.IntegrationTests.Features
{
    public class GetCustomerTests : AppReadTestBase
    {
        public GetCustomerTests(AppTestFixture fixture) : base(fixture)
        {
        }

        [Fact]
        public async Task GetCustomer_ShouldBeAsExpected_ForExistingCustomer()
        {
            var expectedCustomer = CustomerTestData.TestCustomer;

            var customer = await Fixture.SendAsync(new GetCustomerQuery(expectedCustomer.Id));

            customer.Should().NotBeNull();
            customer.Id.Should().Be(expectedCustomer.Id);
        }
    }
}

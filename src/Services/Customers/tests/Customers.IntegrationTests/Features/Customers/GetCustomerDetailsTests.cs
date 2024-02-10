using Customers.Features.Customers.Queries.GetCustomerDetails;
using Customers.IntegrationTests.TestData;
using FluentAssertions;

namespace Customers.IntegrationTests.Features.Customers;

public class GetCustomerDetailsTests : AppReadTestBase
{
    public GetCustomerDetailsTests(AppTestFixture fixture) : base(fixture)
    {
    }

    [Fact]
    public async Task GetCustomer_ShouldBeAsExpected_ForExistingCustomer()
    {
        // Arrange
        var expectedCustomer = CustomerTestData.TestCustomer;

        // Act
        var customerDetails = await Fixture.SendAsync(new GetCustomerDetailsQuery(expectedCustomer.Id));

        // Assert
        customerDetails.Should().NotBeNull();
        customerDetails.Id.Should().Be(expectedCustomer.Id);
        customerDetails.FirstName.Should().Be(expectedCustomer.FirstName);
        customerDetails.LastName.Should().Be(expectedCustomer.LastName);
        customerDetails.EmailAddress.Should().Be(expectedCustomer.EmailAddress);
    }
}
using Customers.SystemIntegrationTests.TestData;
using FluentAssertions;

namespace Customers.SystemIntegrationTests.Features.Customers;

public class GetCustomerDetailsTests : AppTestBase
{
    public GetCustomerDetailsTests(AppTestFixture fixture) : base(fixture)
    {
    }

    [Fact]
    public async Task GetCustomer_ShouldBeAsExpected_ForExistingCustomer()
    {
        // Arrange
        var expectedCustomer = CustomerTestData.TestCustomer;
        var client = CreateCustomerClient();

        // Act
        var customerDetails = await client.GetCustomerDetailsAsync(expectedCustomer.Id.ToString());

        // Assert
        customerDetails.Should().NotBeNull();
        customerDetails.Id.Should().Be(expectedCustomer.Id);
        customerDetails.FirstName.Should().Be(expectedCustomer.FirstName);
        customerDetails.LastName.Should().Be(expectedCustomer.LastName);
        customerDetails.EmailAddress.Should().Be(expectedCustomer.EmailAddress);
    }
}
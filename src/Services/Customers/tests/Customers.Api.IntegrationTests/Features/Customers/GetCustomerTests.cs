using Argo.MD.Customers.Api.IntegrationTests.TestData;
using FluentAssertions;

namespace Argo.MD.Customers.Api.IntegrationTests.Features.Customers;

public class GetCustomerTests(AppTestFixture fixture) : AppTestBase(fixture)
{
    [Fact]
    public async Task GetCustomer_ShouldBeAsExpected_ForExistingCustomer()
    {
        // Arrange
        var expectedCustomer = CustomerTestData.TestCustomer;
        var client = CreateCustomerApiClient();

        // Act
        var response = await client.GetCustomerAsync(expectedCustomer.Id);

        // Assert
        response.Should().NotBeNull();
        response.Id.Should().Be(expectedCustomer.Id);
        response.FirstName.Should().Be(expectedCustomer.FirstName);
        response.LastName.Should().Be(expectedCustomer.LastName);
        response.EmailAddress.Should().Be(expectedCustomer.EmailAddress);
    }
}
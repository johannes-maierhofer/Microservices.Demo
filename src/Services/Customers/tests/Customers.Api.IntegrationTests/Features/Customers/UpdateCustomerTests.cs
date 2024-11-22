using FluentAssertions;
using ApiClient = Argo.MD.Customers.Api.Client;

namespace Argo.MD.Customers.Api.IntegrationTests.Features.Customers;

public class UpdateCustomerTests(AppTestFixture fixture) : AppTestBase(fixture)
{
    [Fact]
    public async Task UpdateCustomer_ShouldReturnNonEmptyGuid_WhenCommandIsValid()
    {
        // Arrange
        var client = CreateCustomerApiClient();

        // Act
        var request = new ApiClient.UpdateCustomerRequest
        {
            Id = TestData.CustomerTestData.TestCustomer.Id,
            FirstName = "Herbert (update)",
            LastName = "Hofer (update)",
            EmailAddress = Guid.NewGuid() + "@aol.com"
        };

        var response = await client.UpdateCustomerAsync(request);

        // Assert
        response.Id.Should().Be(request.Id);
        response.FirstName.Should().Be(request.FirstName);
        response.LastName.Should().Be(request.LastName);
        response.EmailAddress.Should().Be(request.EmailAddress);
    }
}
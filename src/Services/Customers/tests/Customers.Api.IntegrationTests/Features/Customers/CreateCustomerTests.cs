using FluentAssertions;
using ApiClient = Argo.MD.Customers.Api.Client;

namespace Argo.MD.Customers.Api.IntegrationTests.Features.Customers;

public class CreateCustomerTests : AppTestBase
{
    public CreateCustomerTests(AppTestFixture fixture) : base(fixture)
    {
    }

    [Fact]
    public async Task CreateCustomer_ShouldReturnNonEmptyGuid_WhenCommandIsValid()
    {
        // Arrange
        var client = CreateCustomerApiClient();

        // Act
        var response = await client.CreateCustomerAsync(new ApiClient.CreateCustomerRequest
        {
            FirstName = "Herbert",
            LastName = "Hofer",
            EmailAddress = Guid.NewGuid() + "@aol.com"
        });

        // Assert
        response.Id.Should().NotBeEmpty();
    }
}
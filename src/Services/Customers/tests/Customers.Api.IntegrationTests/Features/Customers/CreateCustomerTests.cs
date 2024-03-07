using Customers.Api.Client;
using FluentAssertions;

namespace Customers.Api.IntegrationTests.Features.Customers;

public class CreateCustomerTests : AppTestBase
{
    public CreateCustomerTests(AppTestFixture fixture) : base(fixture)
    {
    }

    [Fact]
    public async Task CreateCustomer_ShouldReturnNonEmptyGuid_WhenCommandIsValid()
    {
        // Arrange
        var client = CreateCustomerClient();

        // Act
        var response = await client.CreateCustomerAsync(new CreateCustomerRequest
        {
            FirstName = "Herbert",
            LastName = "Hofer",
            EmailAddress = Guid.NewGuid() + "@aol.com"
        });

        // Assert
        response.Id.Should().NotBeEmpty();
    }
}
using FluentAssertions;

namespace Argo.MD.Customers.Api.IntegrationTests.Features.Customers;

public class GetCustomerListTests : AppTestBase
{
    public GetCustomerListTests(AppTestFixture fixture) : base(fixture)
    {
    }

    [Fact]
    public async Task GetCustomerList_ShouldReturnListOfCustomers_ForPageNumberOne()
    {
        // Arrange
        var client = CreateCustomerApiClient();
        const int pageNumber = 1;
        const int pageSize = 5;

        // Act
        var response = await client.GetCustomerListAsync(pageNumber, pageSize);

        // Assert
        response.PageNumber.Should().Be(pageNumber);
        response.Items.Should().NotBeEmpty();
        response.Items.Count.Should().BeLessThanOrEqualTo(pageSize);
    }
}
using FluentAssertions;

namespace Customers.SystemIntegrationTests.Features.Customers;

public class GetCustomerListTests : AppTestBase
{
    public GetCustomerListTests(AppTestFixture fixture) : base(fixture)
    {
    }

    [Fact]
    public async Task GetCustomerList_ShouldReturnListOfCustomers_ForPageNumberOne()
    {
        // Arrange
        var client = CreateCustomerClient();
        const int pageNumber = 1;
        const int pageSize = 5;

        // Act
        var result = await client.GetCustomerListAsync(pageNumber, pageSize);

        // Assert
        result.PageNumber.Should().Be(pageNumber);
        result.Items.Should().NotBeEmpty();
        result.Items.Count.Should().BeLessThanOrEqualTo(pageSize);
    }
}
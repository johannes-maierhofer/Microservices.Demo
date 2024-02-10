using Customers.Features.Customers.Queries.GetCustomerList;
using FluentAssertions;

namespace Customers.IntegrationTests.Features.Customers;

public class GetCustomerListTests : AppReadTestBase
{
    public GetCustomerListTests(AppTestFixture fixture) : base(fixture)
    {
    }

    [Fact]
    public async Task GetCustomerList_ShouldReturnListOfCustomers_ForPageNumberOne()
    {
        // Arrange
        const int pageNumber = 1;
        const int pageSize = 5;

        // Act
        var result = await Fixture.SendAsync(new GetCustomerListQuery(pageNumber, pageSize));

        // Assert
        result.PageNumber.Should().Be(pageNumber);
        result.Items.Should().NotBeEmpty();
        result.Items.Count.Should().BeLessThanOrEqualTo(pageSize);
    }
}
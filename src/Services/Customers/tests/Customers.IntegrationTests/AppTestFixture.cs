using BuildingBlocks.Testing;
using Customers.Api;
using Customers.IntegrationTests.TestData;
using Customers.Persistence;

namespace Customers.IntegrationTests;

public class AppTestFixture : WebAppTestFixtureWithDb<Program, CustomerDbContext>
{
    private readonly MsSqlTestFixture _msSqlTestFixture = new();
    private readonly RabbitMqTestFixture _rabbitMqTestFixture = new();

    public override async Task InitializeAsync()
    {
        await _msSqlTestFixture.InitializeAsync();
        await _rabbitMqTestFixture.InitializeAsync();
        await base.InitializeAsync();
    }

    public override async Task DisposeAsync()
    {
        await base.DisposeAsync();
        await _msSqlTestFixture.DisposeAsync();
        await _rabbitMqTestFixture.DisposeAsync();
    }

    protected override async Task SeedData(CustomerDbContext dbContext)
    {
        dbContext.Customers.Add(CustomerTestData.TestCustomer);

        await dbContext.SaveChangesAsync();
    }
}
//using BuildingBlocks.Contracts.Messages;
//using BuildingBlocks.Testing;
//using Customers.Api;
//using Customers.Features.Commands.CreateCustomer;
//using Customers.Persistence;
//using FluentAssertions;
//using MassTransit.Testing;

//namespace Customers.IntegrationTests.Features
//{
//    [Collection("App")]
//    public class CreateCustomerTests2 : TestFixture<Program>, IClassFixture<DbContextTestFixture<Program, CustomerDbContext>>
//    {
//        [Fact]
//        public async Task CreateCustomer_ShouldReturnNonEmptyGuid_WhenCommandIsValid()
//        {
//            // Arrange
//            var command = CreateValidCommand();

//            // Act
//            var response = await SendAsync(command);

//            // Assert
//            response.Should().NotBeEmpty();
//        }

//        [Fact]
//        public async Task CreateCustomer_ShouldPublishCustomerCreatedEvent_WhenCommandIsValid()
//        {
//            // Arrange
//            var command = CreateValidCommand();

//            // Act
//            await SendAsync(command);

//            // Assert
//            var harness = ServiceProvider.GetTestHarness();
//            var eventWasPublished = await harness.Published.Any<CustomerContracts.CustomerCreated>();
//            eventWasPublished.Should().Be(true);
//        }

//        private static CreateCustomerCommand CreateValidCommand()
//        {
//            return new CreateCustomerCommand
//            {
//                FirstName = "First",
//                LastName = "Last",
//                EmailAddress = Guid.NewGuid() + "@test.com"
//            };
//        }
//    }
//}
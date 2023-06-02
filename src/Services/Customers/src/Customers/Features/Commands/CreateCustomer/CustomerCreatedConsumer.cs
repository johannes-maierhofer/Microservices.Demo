//using BuildingBlocks.Contracts.Messages;
//using MassTransit;
//using Microsoft.Extensions.Logging;

//namespace Customers.Features.Commands.CreateCustomer
//{
//    public class CustomerCreatedConsumer : IConsumer<CustomerContracts.CustomerCreated>
//    {
//        private readonly ILogger<CustomerCreatedConsumer> _logger;

//        public CustomerCreatedConsumer(ILogger<CustomerCreatedConsumer> logger)
//        {
//            _logger = logger;
//        }

//        public Task Consume(ConsumeContext<CustomerContracts.CustomerCreated> context)
//        {
//            _logger.LogInformation($"Consume message of type {nameof(CustomerContracts.CustomerCreated)} with customerId: {context.Message.Id}.");
//            return Task.CompletedTask;
//        }
//    }
//}

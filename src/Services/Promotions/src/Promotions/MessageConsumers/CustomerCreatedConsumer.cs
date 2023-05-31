using BuildingBlocks.Contracts.Messages;
using BuildingBlocks.Messaging;
using MassTransit;
using Microsoft.Extensions.Logging;
using Promotions.Services;

namespace Promotions.MessageConsumers
{
    public class CustomerCreatedConsumer : IConsumer<CustomerContracts.CustomerCreated>
    {
        private readonly ILogger<CustomerCreatedConsumer> _logger;
        private readonly ICustomerApiClient _apiClient;
        private readonly IMessageBus _bus;

        public CustomerCreatedConsumer(
            ILogger<CustomerCreatedConsumer> logger,
            ICustomerApiClient apiClient,
            IMessageBus bus)
        {
            _logger = logger;
            _apiClient = apiClient;
            _bus = bus;
        }

        public async Task Consume(ConsumeContext<CustomerContracts.CustomerCreated> context)
        {
            _logger.LogInformation($"Consume message of type {nameof(CustomerContracts.CustomerCreated)} with customerId: {context.Message.Id}.");

            var customerData = await _apiClient.GetCustomerById(context.Message.Id);

            var sendEmailCmd = new EmailSenderContracts.SendEmail
            {
                Recipient = customerData.EmailAddress,
                Subject = "Promo for new customers",
                MessageText = $"Dear {customerData.FirstName}, we have some really cool offer for you as a new customer."
            };

            await _bus.Send(sendEmailCmd);
        }
    }
}

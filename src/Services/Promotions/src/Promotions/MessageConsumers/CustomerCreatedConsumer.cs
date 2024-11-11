using Argo.MD.BuildingBlocks.Messaging;
using Argo.MD.Customers.Api.Client;
using Argo.MD.Customers.Messages.Events;
using Argo.MD.EmailSender.Messages.Commands;
using MassTransit;
using Microsoft.Extensions.Logging;

namespace Argo.MD.Promotions.MessageConsumers
{
    public class CustomerCreatedConsumer : IConsumer<CustomerCreated>
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

        public async Task Consume(ConsumeContext<CustomerCreated> context)
        {
            _logger.LogInformation(
                "Consume message of type {Consumer} with customerId: {CustomerId}.",
                nameof(CustomerCreated),
                context.Message.Id);

            var customer = await _apiClient.GetCustomerDetailsAsync(context.Message.Id);

            if (customer.EmailAddress == null)
            {
                return;
            }

            var sendEmailCmd = new SendEmail(
                customer.EmailAddress,
                "Promo for new customers",
                $"Dear {customer.FirstName}, we have some really cool offer for you as a new customer.");

            await _bus.Send(sendEmailCmd);
        }
    }
}

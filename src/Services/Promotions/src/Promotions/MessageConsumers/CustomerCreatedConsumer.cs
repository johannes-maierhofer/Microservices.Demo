using Argo.MD.BuildingBlocks.Messaging;
using Argo.MD.Customers.Api.Client;
using Argo.MD.Customers.Messages;
using Argo.MD.EmailSender.Messages.Commands;
using MassTransit;
using Microsoft.Extensions.Logging;

namespace Argo.MD.Promotions.MessageConsumers;

public class CustomerCreatedConsumer(
    ILogger<CustomerCreatedConsumer> logger,
    ICustomerApiClient apiClient,
    IMessageBus messageBus)
    : IConsumer<CustomerCreated>
{
    public async Task Consume(ConsumeContext<CustomerCreated> context)
    {
        logger.LogInformation(
            "Consume message of type {Consumer} with customerId: {CustomerId}.",
            nameof(CustomerCreated),
            context.Message.Id);

        var customer = await apiClient.GetCustomerAsync(context.Message.Id);

        var sendEmail = new SendEmail(
            customer.EmailAddress,
            "Promo for new customers",
            $"Dear {customer.FirstName}, we have some really cool offer for you as a new customer.");

        await messageBus.Send(sendEmail);
    }
}
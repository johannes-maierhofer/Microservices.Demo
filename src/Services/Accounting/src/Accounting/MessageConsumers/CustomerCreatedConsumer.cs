using Argo.MD.Accounting.Domain.CustomerAggregate;
using Argo.MD.Accounting.Persistence;
using Argo.MD.Customers.Api.Client;
using Argo.MD.Customers.Messages.Events;
using MassTransit;
using Microsoft.Extensions.Logging;

namespace Argo.MD.Accounting.MessageConsumers;

public class CustomerCreatedConsumer(
    ILogger<CustomerCreatedConsumer> logger,
    AccountingDbContext dbContext,
    ICustomerApiClient apiClient)
    : IConsumer<CustomerCreated>
{
    public async Task Consume(ConsumeContext<CustomerCreated> context)
    {
        logger.LogInformation(
            "Consume message of type {EventName} with customerId: {CustomerId}.",
            nameof(CustomerCreated),
            context.Message.Id);

        // handling the event is IDEMPOTENT!!!!!!!!!
        var customerData = await apiClient.GetCustomerDetailsAsync(context.Message.Id);

        var customer = dbContext.Customers.FirstOrDefault(c => c.Id == context.Message.Id);

        if (customer == null)
        {
            customer = new Customer(
                customerData.FirstName,
                customerData.LastName,
                customerData.EmailAddress);

            dbContext.Customers.Add(customer);
        }
        else
        {
            customer.Update(
                customerData.FirstName,
                customerData.LastName,
                customerData.EmailAddress);
        }

        await dbContext.SaveChangesAsync();
    }
}
using Argo.MD.BuildingBlocks.Core.Exceptions;
using Argo.MD.Customers.Api.Features.Customers.Common;
using Argo.MD.Customers.Api.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Argo.MD.Customers.Api.Features.Customers.Commands.UpdateCustomer;

public class UpdateCustomerCommandHandler(CustomerDbContext dbContext)
    : IRequestHandler<UpdateCustomerCommand, CustomerResponse>
{
    public async Task<CustomerResponse> Handle(UpdateCustomerCommand command, CancellationToken cancellationToken)
    {
        var customer = await dbContext.Customers
            .FirstOrDefaultAsync(c => c.Id == command.Id, cancellationToken);

        if (customer == null)
        {
            throw new NotFoundException($"Customer with Id '{command.Id}' not found.");
        }

        customer.Update(
            command.FirstName,
            command.LastName,
            command.EmailAddress);

        await dbContext.SaveChangesAsync(cancellationToken);

        return new CustomerResponse(
            customer.Id,
            customer.FirstName,
            customer.LastName,
            customer.EmailAddress);
    }
}

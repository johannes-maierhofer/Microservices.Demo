﻿using Argo.MD.Customers.Api.Persistence;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace Argo.MD.Customers.Api.Features.Customers.Commands.CreateCustomer;

public class CreateCustomerCommandValidator : AbstractValidator<CreateCustomerCommand>
{
    private readonly CustomerDbContext _dbContext;

    public CreateCustomerCommandValidator(CustomerDbContext dbContext)
    {
        _dbContext = dbContext;

        RuleFor(x => x.FirstName)
            .NotEmpty().WithMessage("First name may not be empty");
        RuleFor(x => x.LastName)
            .NotEmpty().WithMessage("Last name may not be empty");
        RuleFor(x => x.EmailAddress)
            .NotEmpty().WithMessage("Email may not be empty")
            .EmailAddress().WithMessage("Email address is invalid")
            .MustAsync(BeUniqueEmail).WithMessage("A customer with the specified email already exists.");
    }

    private async Task<bool> BeUniqueEmail(string email, CancellationToken cancellationToken)
    {
        var alreadyExists = await _dbContext.Customers
            .AnyAsync(c => c.EmailAddress == email, cancellationToken);

        return !alreadyExists;
    }
}
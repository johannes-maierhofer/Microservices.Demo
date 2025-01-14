﻿using Argo.MD.BuildingBlocks.Mediatr;
using Argo.MD.Customers.Api.Features.Customers.Common;

namespace Argo.MD.Customers.Api.Features.Customers.Commands.CreateCustomer;

public record CreateCustomerCommand(
    string FirstName,
    string LastName,
    string EmailAddress)
    : ICommand<CustomerResponse>;
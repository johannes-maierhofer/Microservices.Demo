﻿using Argo.MD.BuildingBlocks.Core.Domain;
using Argo.MD.Customers.Api.Domain.CustomerAggregate.Events;

namespace Argo.MD.Customers.Api.Domain.CustomerAggregate;

public class Customer : Entity<Guid>
{
    public string FirstName { get; private set; }
    public string LastName { get; private set; }
    public string EmailAddress { get; private set; }

    private Customer(
        Guid id,
        string firstName,
        string lastName,
        string emailAddress)
    {
        Id = id;
        FirstName = firstName;
        LastName = lastName;
        EmailAddress = emailAddress;
    }

    public static Customer Create(
        string firstName,
        string lastName,
        string emailAddress)
    {
        var customer = new Customer(
            Guid.NewGuid(),
            firstName,
            lastName,
            emailAddress);

        customer.AddDomainEvent(new CustomerCreated(customer));

        return customer;
    }

    public void Update(string firstName, string lastName, string emailAddress)
    {
        this.FirstName = firstName;
        this.LastName = lastName;
        this.EmailAddress = emailAddress;

        AddDomainEvent(new CustomerUpdated(this));
    }

#pragma warning disable CS8618
    private Customer()
    {
        // for EF
    }
#pragma warning restore CS8618
}
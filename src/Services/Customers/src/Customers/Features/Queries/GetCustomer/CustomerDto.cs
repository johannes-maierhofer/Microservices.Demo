﻿namespace Customers.Features.Queries.GetCustomer
{
    public record CustomerDto
    {
        public Guid Id { get; set; }
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public required string EmailAddress { get; set; }
    }
}
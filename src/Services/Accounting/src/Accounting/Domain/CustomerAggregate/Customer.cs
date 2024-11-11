using Argo.MD.BuildingBlocks.Core.Domain;

namespace Argo.MD.Accounting.Domain.CustomerAggregate;

public class Customer : Entity<Guid>
{
    public Customer(string firstName, string lastName, string emailAddress)
    {
        FirstName = firstName;
        LastName = lastName;
        EmailAddress = emailAddress;
    }

    public string FirstName { get; private set; }
    public string LastName { get; private set; }
    public string EmailAddress { get; private set; }

    public void Update(string firstName, string lastName, string emailAddress)
    {
        FirstName = firstName;
        LastName = lastName;
        EmailAddress = emailAddress;
    }
}
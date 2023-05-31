using Promotions.Models;

namespace Promotions.Services
{
    public interface ICustomerApiClient
    {
        Task<CustomerData> GetCustomerById(Guid customerId);
    }
}

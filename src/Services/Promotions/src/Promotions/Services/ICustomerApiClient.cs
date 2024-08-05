using Argo.MD.Promotions.Models;

namespace Argo.MD.Promotions.Services
{
    public interface ICustomerApiClient
    {
        Task<CustomerData> GetCustomerById(Guid customerId);
    }
}

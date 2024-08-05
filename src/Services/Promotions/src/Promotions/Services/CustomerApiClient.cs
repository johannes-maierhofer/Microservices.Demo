using System.Text.Json;
using Argo.MD.Promotions.Config;
using Argo.MD.Promotions.Models;
using Microsoft.Extensions.Options;

namespace Argo.MD.Promotions.Services
{
    public class CustomerApiClient : ICustomerApiClient
    {
        private readonly IOptions<UrlsConfig> _config;
        private readonly IHttpClientFactory _httpClientFactory;

        private readonly JsonSerializerOptions _jsonSerializerOptions = new()
        {
            PropertyNameCaseInsensitive = true
        };

        public CustomerApiClient(IOptions<UrlsConfig> config, IHttpClientFactory httpClientFactory)
        {
            _config = config;
            _httpClientFactory = httpClientFactory;
        }

        public async Task<CustomerData> GetCustomerById(Guid customerId)
        {
            using var client = _httpClientFactory.CreateClient("Customers");

            var uri = _config.Value.Customers + "/api/v1/customers/" + customerId;
            var response = await client.GetAsync(uri);

            // var request = response.RequestMessage;

            response.EnsureSuccessStatusCode();

            var responseContent = await response.Content.ReadAsStringAsync();

            return JsonSerializer.Deserialize<CustomerData>(responseContent, _jsonSerializerOptions)!;
        }
    }
}

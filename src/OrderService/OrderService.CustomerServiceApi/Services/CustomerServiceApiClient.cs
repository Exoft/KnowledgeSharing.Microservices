using System;
using System.Threading.Tasks;
using Newtonsoft.Json;
using OrderService.CustomerServiceApi.Interfaces;
using OrderService.CustomerServiceApi.Models.ResponseModels;
using RestEase;

namespace OrderService.CustomerServiceApi.Services
{
    public class CustomerServiceApiClient : ICustomerServiceApiClient
    {
        private const string CustomerServiceApiUrl = "http://localhost:5005";
        
        private readonly ICustomerServiceApi _client;

        public CustomerServiceApiClient()
        {
            _client = RestClient.For<ICustomerServiceApi>(CustomerServiceApiUrl);
        }
        
        public async Task<CustomerResponseModel> GetCustomerByIdAsync(long id, string correlationId)
        {
            _client.CorrelationId = correlationId;
            var response = await _client.GetCustomerById(id);
            if (!response.IsSuccess)
                throw new Exception(response.ErrorMessage);

            var model = JsonConvert.DeserializeObject<CustomerResponseModel>(response.Data.ToString());
            return model;
        }
    }
}
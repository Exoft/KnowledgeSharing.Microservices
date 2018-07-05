using System;
using System.Threading.Tasks;
using BookingService.CustomerServiceApi.Interfaces;
using BookingService.CustomerServiceApi.Models.ResponseModels;
using Newtonsoft.Json;
using RestEase;

namespace BookingService.CustomerServiceApi.Services
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
using System.Threading.Tasks;
using OrderService.CustomerServiceApi.Models.ResponseModels;

namespace OrderService.CustomerServiceApi.Interfaces
{
    public interface ICustomerServiceApiClient
    {
        Task<CustomerResponseModel> GetCustomerByIdAsync(long id, string correlationId);
    }
}
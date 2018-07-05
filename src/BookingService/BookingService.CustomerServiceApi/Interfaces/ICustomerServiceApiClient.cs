using System.Threading.Tasks;
using BookingService.CustomerServiceApi.Models.ResponseModels;

namespace BookingService.CustomerServiceApi.Interfaces
{
    public interface ICustomerServiceApiClient
    {
        Task<CustomerResponseModel> GetCustomerByIdAsync(long id, string correlationId);
    }
}
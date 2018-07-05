using System.Threading.Tasks;
using BookingService.CustomerServiceApi.Infrastructure;
using RestEase;

namespace BookingService.CustomerServiceApi
{
    [Header("User-Agent", "RestEase")]
    public interface ICustomerServiceApi
    {
        [Header("X-Correlation-ID")]
        string CorrelationId { get; set; }
        
        [Get("/papi/Customers/{id}")]
        Task<Message> GetCustomerById([Path] long id);
    }
}
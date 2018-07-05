using System.Threading.Tasks;
using OrderService.CustomerServiceApi.Infrastructure;
using RestEase;

namespace OrderService.CustomerServiceApi
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
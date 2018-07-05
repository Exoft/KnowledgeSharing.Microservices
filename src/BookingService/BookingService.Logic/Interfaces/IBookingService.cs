using System.Threading.Tasks;
using BookingService.Domain;

namespace BookingService.Logic.Interfaces
{
    public interface IBookingService
    {
        Task<Booking> GetAsync(long id);
        Task<Booking> CreateAsync(string startTime, string endTime, long customerId);
        Task<Booking> UpdateAsync(long id, string startTime, string endTime);
        Task<bool> DeleteAsync(long id);     
    }
}
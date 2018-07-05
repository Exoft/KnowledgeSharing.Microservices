using BookingService.Domain;

namespace BookingService.Infrastructure.Repositories
{
    public class BookingRepository : Repository<long, Booking>
    {
        public BookingRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
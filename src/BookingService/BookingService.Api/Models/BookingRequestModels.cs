namespace BookingService.Api.Models.RequestModels
{
    public class BaseBookingRequestModel
    {
        public string StartTime { get; set; }
        public string EndTime { get; set; }
    }

    public class CreateBookingRequestModel : BaseBookingRequestModel
    {
        public long CustomerId { get; set; }    
    }
    
    public class UpdateBookingRequestModel : BaseBookingRequestModel
    {
        public long Id { get; set; }
    }
}
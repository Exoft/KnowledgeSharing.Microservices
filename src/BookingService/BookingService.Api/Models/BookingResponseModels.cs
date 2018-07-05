namespace BookingService.Api.Models.ResponseModels
{
    public class BookingResponseModel
    {
        public string StartTime { get; set; }
        public string EndTime { get; set; }
        public long CustomerId { get; set; }
        public string NameOfCustomer { get; set; }
    }
}
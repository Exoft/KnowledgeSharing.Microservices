namespace BookingService.CustomerServiceApi.Infrastructure
{
    public class Message
    {
        public bool IsSuccess { get; set; }
        public object Data { get; set; }
        public string ErrorMessage { get; set; }
    }
}
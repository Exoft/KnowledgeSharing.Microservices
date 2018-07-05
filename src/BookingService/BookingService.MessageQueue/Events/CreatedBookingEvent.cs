namespace BookingService.MessageQueue.Events
{
    public class CreatedBookingEvent : BookingEvent
    {
        public long CustomerId { get; set; }
    }
}
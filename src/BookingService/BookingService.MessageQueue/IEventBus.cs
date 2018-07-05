using BookingService.MessageQueue.Events;

namespace BookingService.MessageQueue
{
    public interface IEventBus
    {
        void Publish(BookingEvent @event, string exchange);
    }
}
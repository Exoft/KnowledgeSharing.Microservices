namespace OrderService.MessageQueue.Events
{
    public class CreateOrderEvent : OrderEvent
    {
        public long Id { get; set; }
        public string Description { get; set; }
    }
}
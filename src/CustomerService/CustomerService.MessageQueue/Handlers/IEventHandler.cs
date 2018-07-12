using RabbitMQ.Client.Events;

namespace CustomerService.MessageQueue.Handlers
{
    public interface IEventHandler
    {
        void ConsumerOnReceived(object sender, BasicDeliverEventArgs ea);
    }
}
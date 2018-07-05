using System.Diagnostics;
using System.Text;
using RabbitMQ.Client.Events;

namespace CustomerService.MessageQueue.Handlers
{
    public static class BookingEventHandler
    {  
        public static void ConsumerOnReceived(object sender, BasicDeliverEventArgs ea)
        {
            var body = ea.Body;
            var message = Encoding.UTF8.GetString(body);
        }
    }
}
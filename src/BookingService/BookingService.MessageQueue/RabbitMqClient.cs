using System.Text;
using RabbitMQ.Client;

namespace BookingService.MessageQueue
{
    public class RabbitMqClient
    {
        private const string BookingExchange = "booking-exchange";
        
        private readonly IConnection _connection;
        private readonly IModel _channel;

        public RabbitMqClient(IConnection connection, IModel channel)
        {
            _connection = connection;
            _channel = channel;
            
            _channel.ExchangeDeclare(exchange: BookingExchange, 
                type: ExchangeType.Fanout, 
                durable: true);
        }
        
        public void Publish(object body)
        {
            var bytesBody = Encoding.UTF8.GetBytes("Hi from BookingService");
            
            _channel.BasicPublish(exchange: BookingExchange,
                routingKey: "",
                basicProperties: null,
                body: bytesBody);
        }
    }
}
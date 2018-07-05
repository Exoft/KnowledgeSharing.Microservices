using System.Text;
using RabbitMQ.Client;

namespace OrderService.MessageQueue
{
    public class RabbitMqClient
    {
        public const string OrderExchange = "order-exchange";

        private readonly IConnection _connection;
        private readonly IModel _channel;

        public RabbitMqClient(IConnection connection, IModel channel)
        {
            _connection = connection;
            _channel = channel;
            
            _channel.ExchangeDeclare(exchange: OrderExchange, 
                type: ExchangeType.Fanout, 
                durable: true);
        }
        
        public void Publish(object body)
        {
            var bytesBody = Encoding.UTF8.GetBytes("Hi from OrderService");
            
            _channel.BasicPublish(exchange: OrderExchange,
                routingKey: "",
                basicProperties: null,
                body: bytesBody);
        }
    }
}
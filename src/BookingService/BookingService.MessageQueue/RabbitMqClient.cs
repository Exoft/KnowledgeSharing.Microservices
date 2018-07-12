using System;
using System.Text;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;

namespace BookingService.MessageQueue
{
    public class RabbitMqClient
    {
        private const string BookingExchange = "booking-exchange";
        
        private readonly IConnection _connection;
        private readonly IModel _channel;
        private readonly ILogger _logger;
        
        public RabbitMqClient(
            IConnection connection, 
            IModel channel,
            ILoggerFactory loggerFactory)
        {
            _connection = connection;
            _channel = channel;
            _logger = loggerFactory.CreateLogger(typeof(RabbitMqClient).Name);
            
            _channel.ExchangeDeclare(exchange: BookingExchange, 
                type: ExchangeType.Fanout, 
                durable: true);
        }
        
        public void Publish(object body)
        {
            var bytesBody = Encoding.UTF8.GetBytes("Hi from BookingService");
            
            _logger.LogInformation($"Sending message {BitConverter.ToString(bytesBody)}");
            
            _channel.BasicPublish(exchange: BookingExchange,
                routingKey: "",
                basicProperties: null,
                body: bytesBody);
            
            _logger.LogInformation($"Sended message {BitConverter.ToString(bytesBody)}");
        }
    }
}
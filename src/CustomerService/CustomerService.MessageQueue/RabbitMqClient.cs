using System;
using System.Diagnostics;
using System.Text;
using CustomerService.MessageQueue.Handlers;
using CustomerService.MessageQueue.Helpers;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace CustomerService.MessageQueue
{
    public class MessageListener
    {
        private readonly IConnection _connection;
        private readonly IModel _channel;
        private readonly ILogger _logger;

        public MessageListener(
            IConnection connection,
            IModel channel,
            ILoggerFactory loggerFactory)
        {
            _connection = connection;
            _channel = channel;
            _logger = loggerFactory.CreateLogger(typeof(MessageListener).Name);
        }

        public void Subscribe(string exchange, IEventHandler eventHandler)
        {
            _channel.ExchangeDeclare(exchange: exchange, 
                type: ExchangeType.Fanout, 
                durable: true);
          
            var queueName = _channel.QueueDeclare().QueueName;

            _channel.QueueBind(queue: queueName,
                exchange: exchange,
                routingKey: "");

            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += eventHandler.ConsumerOnReceived;
                        
            _channel.BasicConsume(queue: queueName, 
                autoAck: true, 
                consumer: consumer); 
            
            _logger.LogInformation($"Consumed to exchange {exchange}");
            
        }
    }
}
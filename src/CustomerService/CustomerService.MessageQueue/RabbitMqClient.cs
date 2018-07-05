using System;
using System.Diagnostics;
using System.Text;
using CustomerService.MessageQueue.Handlers;
using CustomerService.MessageQueue.Helpers;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace CustomerService.MessageQueue
{
    public class MessageListener
    {
        private static IConnection _connection;
        private static IModel _channel;

        public MessageListener(IConnection connection, IModel channel)
        {
            _connection = connection;
            _channel = channel;
        }

        public void Subscribe(string exchange)
        {
            _channel.ExchangeDeclare(exchange: exchange, 
                type: ExchangeType.Fanout, 
                durable: true);
          
            var queueName = _channel.QueueDeclare().QueueName;

            _channel.QueueBind(queue: queueName,
                exchange: exchange,
                routingKey: "");

            var consumer = new EventingBasicConsumer(_channel);
            switch (exchange)
            {
                case Constants.MessageQueue.BookingExchange:
                {
                    consumer.Received += BookingEventHandler.ConsumerOnReceived;
                    break;
                }
                case Constants.MessageQueue.OrderExchange:
                {
                    consumer.Received += OrderEventHandler.ConsumerOnReceived;
                    break;
                }
                default:
                    throw new Exception("Incorrect exchange name");
            }
            
            _channel.BasicConsume(queue: queueName, 
                autoAck: true, 
                consumer: consumer); 
        }
    }
}
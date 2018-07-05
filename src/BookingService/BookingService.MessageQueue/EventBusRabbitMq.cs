using System;
using System.Net.Sockets;
using System.Text;
using BookingService.MessageQueue.Events;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Polly;
using Polly.Retry;
using RabbitMQ.Client.Exceptions;

namespace BookingService.MessageQueue
{
    public class EventBusRabbitMq : IEventBus
    {
        private const ushort RetryCount = 5;
        
        private readonly IRabbitMqPersistentConnection _persistentConnection;
        private readonly ILogger _logger;
        
        public EventBusRabbitMq(
            IRabbitMqPersistentConnection persistentConnection,
            ILoggerFactory loggerFactory)
        {
            _persistentConnection = persistentConnection;
            _logger = loggerFactory.CreateLogger<EventBusRabbitMq>();
        }
        
        public void Publish(BookingEvent @event, string exchange)
        {
            if (!_persistentConnection.IsConnected)
            {
                _persistentConnection.TryConnect();
            }

            var policy = RetryPolicy.Handle<BrokerUnreachableException>()
                .Or<SocketException>()
                .WaitAndRetry(RetryCount, retryAttempt => TimeSpan.FromSeconds(10), (ex, time) =>
                {
                    _logger.LogWarning(ex.ToString());
                });

            using (var channel = _persistentConnection.CreateModel())
            {
                var eventName = @event.GetType()
                    .Name;

                channel.ExchangeDeclare(exchange, "direct", true, false, null);

                var message = JsonConvert.SerializeObject(@event);
                var body = Encoding.UTF8.GetBytes(message);

                policy.Execute(() =>
                {
                    var properties = channel.CreateBasicProperties();
                    properties.DeliveryMode = 2; // persistent

                    channel.BasicPublish(exchange: exchange,
                        routingKey: eventName,
                        mandatory:true,
                        basicProperties: properties,
                        body: body);
                });
            }
        }
    }
}
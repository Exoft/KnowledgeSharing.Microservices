using System;
using System.Text;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client.Events;

namespace CustomerService.MessageQueue.Handlers
{
    public class OrderEventHandler : IEventHandler
    {
        private readonly ILogger _logger;

        public OrderEventHandler(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger(typeof(OrderEventHandler).Name);
        }
        
        public void ConsumerOnReceived(object sender, BasicDeliverEventArgs ea)
        {
            var body = ea.Body;
            var message = Encoding.UTF8.GetString(body);
            _logger.LogInformation(message);
        }
    }
}
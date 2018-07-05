using System;
using System.Threading;
using System.Threading.Tasks;
using CorrelationId;
using Mapster;
using MediatR;
using OrderService.CustomerServiceApi.Interfaces;
using OrderService.Domain.AggregatesModels.OrderAggregate;
using OrderService.MessageQueue;
using OrderService.MessageQueue.Events;
using OrderService.Ordering.Commands;
using OrderService.Ordering.Helpers;
using OrderService.Ordering.Models;

namespace OrderService.Ordering.CommandHandlers
{
    public class OrderCommandHandler : 
        IRequestHandler<CreateOrderCommand, OrderResponseModel>,
        IRequestHandler<UpdateOrderCommand, OrderResponseModel>,
        IRequestHandler<DeleteOrderCommand, bool>
    {
        private readonly IOrderRepository _orderRepository;
        private readonly ICustomerServiceApiClient _customerServiceApiClient;
        private readonly ICorrelationContextAccessor _correlationContext;
        private readonly RabbitMqClient _rabbitMqClient;
        
        public OrderCommandHandler(
            IOrderRepository orderRepository,
            ICustomerServiceApiClient customerServiceApiClient, 
            ICorrelationContextAccessor correlationContext,
            RabbitMqClient rabbitMqClient)
        {
            _orderRepository = orderRepository;
            _customerServiceApiClient = customerServiceApiClient;
            _correlationContext = correlationContext;
            _rabbitMqClient = rabbitMqClient;
        }
        
        public async Task<OrderResponseModel> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
        {
            var customer = await _customerServiceApiClient.GetCustomerByIdAsync(request.CustomerId,
                _correlationContext.CorrelationContext.CorrelationId);
                
            var order = new Order(request.Description, request.CustomerId, customer.FirstName);
            order = await _orderRepository.CreateAsync(order);
            await _orderRepository.SaveChangesAsync();

            var @event = order.Adapt<CreateOrderEvent>();
            _rabbitMqClient.Publish(@event);
            
            var response = order.Adapt<OrderResponseModel>();
            return response;
        }

        public async Task<OrderResponseModel> Handle(UpdateOrderCommand request, CancellationToken cancellationToken)
        {
            var existedOrder = await _orderRepository.GetByIdAsync(request.Id);
            if (existedOrder == null)
                throw new Exception($"Order with id {request.Id} does not exist");
            
            existedOrder.Update(request.Description);
            await _orderRepository.SaveChangesAsync();

            var response = existedOrder.Adapt<OrderResponseModel>();
            return response;
        }

        public async Task<bool> Handle(DeleteOrderCommand request, CancellationToken cancellationToken)
        {
            var existedOrder = await _orderRepository.GetByIdAsync(request.Id);
            if (existedOrder == null)
                throw new Exception($"Order with id {request.Id} does not exist");

            _orderRepository.Delete(existedOrder);
            return await _orderRepository.SaveChangesAsync();
        }
    }
}
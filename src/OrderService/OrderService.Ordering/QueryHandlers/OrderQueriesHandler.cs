using System;
using System.Threading;
using System.Threading.Tasks;
using Mapster;
using MediatR;
using OrderService.Domain.AggregatesModels.OrderAggregate;
using OrderService.Ordering.Models;
using OrderService.Ordering.Queries;

namespace OrderService.Ordering.QueryHandlers
{
    public class OrderQueryHandler : IRequestHandler<GetOrderByIdQuery, OrderResponseModel>
    {
        private readonly IOrderRepository _orderRepository;

        public OrderQueryHandler(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }
        
        public async Task<OrderResponseModel> Handle(GetOrderByIdQuery request, CancellationToken cancellationToken)
        {
            var order = await _orderRepository.GetByIdAsync(request.Id);
            if (order == null)
                throw new Exception($"Order by id {request.Id} does not exist");

            var response = order.Adapt<OrderResponseModel>();
            return response;
        }
    }
}
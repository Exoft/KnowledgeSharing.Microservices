using MediatR;
using OrderService.Ordering.Models;

namespace OrderService.Ordering.Commands
{
    public class CreateOrderCommand : IRequest<OrderResponseModel>
    {
        public string Description { get; set; }
        public long CustomerId { get; set; }
    }
}
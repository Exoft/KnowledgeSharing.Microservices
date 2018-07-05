using MediatR;
using OrderService.Ordering.Models;

namespace OrderService.Ordering.Commands
{
    public class UpdateOrderCommand : IRequest<OrderResponseModel>
    {
        public long Id { get; set; }
        public string Description { get; set; }
    }
}
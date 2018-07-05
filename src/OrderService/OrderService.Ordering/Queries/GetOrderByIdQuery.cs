using MediatR;
using OrderService.Ordering.Models;

namespace OrderService.Ordering.Queries
{
    public class GetOrderByIdQuery : IRequest<OrderResponseModel>
    {
        public long Id { get; set; }
    }
}
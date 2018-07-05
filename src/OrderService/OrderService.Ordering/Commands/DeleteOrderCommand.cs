using MediatR;

namespace OrderService.Ordering.Commands
{
    public class DeleteOrderCommand : IRequest<bool>
    {
        public long Id { get; set; }
    }
}
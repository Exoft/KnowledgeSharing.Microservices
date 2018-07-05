using System.Threading.Tasks;

namespace OrderService.Domain.AggregatesModels.OrderAggregate
{
    public interface IOrderRepository
    {
        Task<Order> GetByIdAsync(long id);
        Task<Order> CreateAsync(Order order);
        Order Update(Order order);
        void Delete(Order order);
        Task<bool> SaveChangesAsync();
    }
}
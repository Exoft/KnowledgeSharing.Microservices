using System;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace BookingService.Infrastructure.Interfaces
{
    public interface IRepository<TK, T>
    {
        Task<T> GetAsync(long id);
        Task<T> CreateAsync(T model);
        T Update(T model);
        bool Delete(T customer);
        Task<bool> SaveChangesAsync();
    }
}
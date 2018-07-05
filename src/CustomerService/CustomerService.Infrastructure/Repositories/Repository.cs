using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using CustomerService.Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CustomerService.Infrastructure.Repositories
{
    public class Repository<TK, T> : IRepository<TK, T> where T : class
    {
        private readonly ApplicationDbContext _context;

        public Repository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<T> GetAsync(long id)
        {
            var model = await _context.FindAsync<T>(id);
            return model;
        }

        public async Task<T> CreateAsync(T model)
        {
            await _context.AddAsync(model);

            return model;
        }

        public T Update(T model)
        {
            _context.Update(model);

            return model;
        }

        public bool Delete(T customer)
        {
            var entry = _context.Remove(customer);

            return entry.State == EntityState.Deleted;
        }

        public async Task<bool> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }
    }
}
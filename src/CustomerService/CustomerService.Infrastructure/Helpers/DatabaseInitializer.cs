using System.Collections.Generic;
using CustomerService.Domain;
using Microsoft.EntityFrameworkCore.Internal;

namespace CustomerService.Infrastructure.Helpers
{
    public class DatabaseInitializer
    {
        private readonly ApplicationDbContext _context;

        public DatabaseInitializer(ApplicationDbContext context)
        {
            _context = context;
        }

        public void Initialize()
        {
            if (!_context.Customers.Any())
            {
                SeedCustomers();
            }
        }

        private void SeedCustomers()
        {
            var customers = new List<Customer>
            {
                new Customer("Harry", "Potter"),
                new Customer("Martin", "Fowler")
            };

            _context.Customers.AddRange(customers);
            _context.SaveChanges();
        }
    }
}
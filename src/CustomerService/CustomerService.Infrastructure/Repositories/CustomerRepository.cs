using CustomerService.Domain;

namespace CustomerService.Infrastructure.Repositories
{
    public class CustomerRepository : Repository<long, Customer>
    {
        public CustomerRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
using CustomerService.Domain;
using CustomerService.Infrastructure.EntityConfigurations;
using Microsoft.EntityFrameworkCore;

namespace CustomerService.Infrastructure
{
    public class ApplicationDbContext : DbContext
    {    
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
            
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfiguration(new CustomerEntityTypeConfiguration());
        }

        public DbSet<Customer> Customers { get; set; }
    }
}
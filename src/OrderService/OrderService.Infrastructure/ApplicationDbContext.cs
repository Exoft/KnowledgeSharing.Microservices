using Microsoft.EntityFrameworkCore;
using OrderService.Domain.AggregatesModels.OrderAggregate;
using OrderService.Infrastructure.EntityConfigurations;

namespace OrderService.Infrastructure
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
            
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfiguration(new OrderTypeConfiguration());
        }
        
        public DbSet<Order> Orders { get; set; }
    }
}
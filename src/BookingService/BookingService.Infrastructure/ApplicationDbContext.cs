using BookingService.Domain;
using BookingService.Infrastructure.EntityConfigurations;
using Microsoft.EntityFrameworkCore;

namespace BookingService.Infrastructure
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            
            modelBuilder.ApplyConfiguration(new BookingEntityTypeConfiguration());
        }
    
        public DbSet<Booking> Bookings { get; set; } 
    }
}
using Microsoft.EntityFrameworkCore;
using OrdersApp.Models.Entities;

namespace OrdersApp.Models.Contexts
{
    public class AppDbContext : DbContext
    {
        public DbSet<Order> Orders { get; set; } = null!;

        public DbSet<OrderItem> OrderItems { get; set; } = null!;

        public DbSet<Provider> Providers { get; set; } = null!;

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
            //Database.EnsureCreated(); // async
        }

        /*protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.LogTo(message => System.Diagnostics.Debug.WriteLine(message));
        }*/
    }
}

using demo.Models;
using Microsoft.EntityFrameworkCore;

namespace demo.Context
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }

        public DbSet<Item> items { get; set; }
        public DbSet<Cart> carts { get; set; }
        public DbSet<CartItem> cartItems { get; set; }
            
    }
}
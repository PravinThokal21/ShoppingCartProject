using Microsoft.EntityFrameworkCore;

namespace ShoppingCart.API.Models
{
    public class ShoppingCartContext : DbContext
    {
        public ShoppingCartContext(DbContextOptions<ShoppingCartContext> options)
        : base(options)
        {
        }

        public DbSet<Entity.CartItem> CartItems { get; set; } = null!;
    }
}

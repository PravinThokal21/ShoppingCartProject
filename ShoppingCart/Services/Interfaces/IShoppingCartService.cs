using ShoppingCart.API.Entity;
using ShoppingCart.API.Models;

namespace ShoppingCart.API.Services.Interfaces
{
    public interface IShoppingCartService
    {
        public Task<bool> AddOrUpdateCart(CartItem cartItem);
        public CartState GetCartState();
    }
}

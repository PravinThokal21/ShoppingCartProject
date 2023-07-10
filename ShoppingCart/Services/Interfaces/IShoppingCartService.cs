using ShoppingCart.API.Entity;
using ShoppingCart.API.Models;

namespace ShoppingCart.API.Services.Interfaces
{
    public interface IShoppingCartService
    {
        public Task<bool> AddItem(CartItem cartItem);
        public CartState GetCartState();
    }
}

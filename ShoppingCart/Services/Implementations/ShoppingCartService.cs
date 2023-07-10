using ShoppingCart.API.Entity;
using ShoppingCart.API.Models;
using ShoppingCart.API.Services.Interfaces;

namespace ShoppingCart.API.Services.Implementations
{
    public class ShoppingCartService : IShoppingCartService
    {
        private const decimal TaxRate = 12.5M;
        ShoppingCartContext _DBContext ;
        public ShoppingCartService(ShoppingCartContext shoppingCartContext)
        {
            _DBContext = shoppingCartContext;
        }
        public async Task<bool> AddOrUpdateCart(CartItem cartItem)
        {
            bool result = true;
            int returnValue = 0;

            var existingItem = _DBContext.CartItems.Where(x => x.Name == cartItem.Name).FirstOrDefault();

            if (existingItem == null)
            {
                _DBContext.CartItems.Add(cartItem);
            }
            else
            {
                existingItem.Quantity += cartItem.Quantity;
                _DBContext.Update(existingItem);
            }

            try
            {
                returnValue = await _DBContext.SaveChangesAsync();
            }
            catch (Exception)
            {
                result = false;
            }    

            return result;
        }

        public CartState GetCartState()
        {
            CartState cartState = new CartState();

            cartState.cartItems = _DBContext.CartItems.ToList();

            cartState.cartItems.ForEach(cartItem => { Math.Round(cartState.Subtotal += cartItem.Quantity * cartItem.Price,2); });                

            cartState.Tax = Math.Round(cartState.Subtotal * TaxRate / 100,2 );

            cartState.Total = Math.Round(cartState.Tax + cartState.Subtotal,2);

            return cartState;
        }
    }
}

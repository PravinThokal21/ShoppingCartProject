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
        public async Task<bool> AddItem(CartItem cartItem)
        {
            bool result = true;
            int returnValue = 0;

            _DBContext.CartItems.Add(cartItem);

            try
            {
                returnValue = await _DBContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                result = false;
            }    

            return result;
        }

        public CartState GetCartState()
        {
            CartState cartState = new CartState();

            cartState.cartItems = _DBContext.CartItems.ToList();

            foreach(CartItem cartItem in cartState.cartItems) 
            {
                cartState.Subtotal += cartItem.Quantity * cartItem.Price;
            }

            cartState.Tax = (cartState.Subtotal * TaxRate / 100 );

            cartState.Total = cartState.Tax + cartState.Subtotal;

            return cartState;
        }
    }
}

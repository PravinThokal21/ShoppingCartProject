using ShoppingCart.API.Entity;

namespace ShoppingCart.API.Models
{   
    public class CartState
    {
        public List<CartItem>? cartItems { get; set; }
        public decimal Subtotal { get; set; }
        public decimal Tax { get; set; }
        public decimal Total { get; set; }
    }
}

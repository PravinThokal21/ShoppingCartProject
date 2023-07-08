namespace ShoppingCart.API.Entity
{
    public class CartItem
    {
        public long Id { get; set; }
        public string? Name { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }

        public CartItem()
        { 
        }
        public CartItem(long id,string name,int quantity,decimal price)
        {
            Id = id;
            Name = name;
            Quantity = quantity;
            Price = price;
        }
    }
}

using System.ComponentModel.DataAnnotations;

namespace ShoppingCart.API.Models
{
    public class CartItemDto
    {
        public CartItemDto(string name,int quantity)
        {
            Name = name;
            Quantity = quantity;
        }

        [Required]
        public string Name { get; set; }
        [Required]
        [Range(1,int.MaxValue)]
        public int Quantity { get; set; }
    }
}

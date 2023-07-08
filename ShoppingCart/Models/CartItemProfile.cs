using AutoMapper;
using ShoppingCart.API.Entity;

namespace ShoppingCart.API.Models
{
    public class CartItemProfile : Profile
    {
        public CartItemProfile() 
        {
            CreateMap<CartItemDto, CartItem>();
        }
    }
}

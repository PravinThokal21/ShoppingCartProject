﻿using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using ShoppingCart.API.Entity;
using ShoppingCart.API.Models;
using ShoppingCart.API.Services.Interfaces;

namespace ShoppingCart.API.Controller
{
    [Route("[controller]")]
    [ApiController]
    public class ShoppingCartController : ControllerBase
    {        
        private readonly IMapper _mapper;
        IProductService _productService;
        IShoppingCartService _shoppingCartService;

        public ShoppingCartController(IShoppingCartService shoppingCartService, IMapper mapper, IProductService productService)
        {
            _mapper = mapper;
            _productService = productService;
            _shoppingCartService = shoppingCartService;
        }


        [HttpPost]
        public async Task<IActionResult> AddItem(CartItemDto cartItemDto)
        {
            CartState cartState ;

            var priceTouple = await _productService.GetProductPrice(cartItemDto.Name);

            if (priceTouple.Item1)
            {
                var item = _mapper.Map<CartItem>(cartItemDto);
                item.Price = priceTouple.Item2;

                bool result = await _shoppingCartService.AddOrUpdateCart(item);

                if(result)
                    cartState = _shoppingCartService.GetCartState();
                else
                    return BadRequest("Something went wrong, please try again !!!");
            }
            else 
            {
                return BadRequest("Product not found !!!");
            }

            return Ok(cartState);
        }

        [HttpGet]
        public IActionResult GetCartState()
        {
            var state = _shoppingCartService.GetCartState();
            return Ok(state);
        }
    }
}

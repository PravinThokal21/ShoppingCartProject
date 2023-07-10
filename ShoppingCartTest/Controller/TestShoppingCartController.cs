using ShoppingCart.API.Controller;
using ShoppingCart.API.Services.Interfaces;
using Moq;
using AutoMapper;
using ShoppingCart.API.Models;
using ShoppingCart.API.Entity;
using Microsoft.AspNetCore.Mvc;
using FluentAssertions;
using Xunit;

namespace ShoppingCart.UnitTests.Controller
{
    
    public class TestShoppingCartController
    {
        private static IMapper _mapper;
        private Mock<IShoppingCartService> mockShoppingCartService;
        private Mock<IProductService> mockproductService;

        public TestShoppingCartController()
        {           
            var mappingConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new CartItemProfile());
            });

            _mapper =  mappingConfig.CreateMapper();

            mockShoppingCartService = new Mock<IShoppingCartService>();
            mockproductService = new Mock<IProductService>();

        }

        [Fact]
        public  async void Post_OnSuccess_ReturnStatusCode200()
        {
            //Arrange            

            CartItem cartItem = GetCartItem();
           
            CartState cartState = GetCartState();
            mockShoppingCartService.Setup(x => x.AddOrUpdateCart(It.IsAny<CartItem>()).Result).Returns(true);
            mockShoppingCartService.Setup(x => x.GetCartState()).Returns(cartState);

            mockproductService.Setup(x => x.GetProductPrice(cartItem.Name)).ReturnsAsync(Tuple.Create<bool, decimal>(true, 2.45M));

            var shoppingCartController = new ShoppingCartController(mockShoppingCartService.Object, _mapper, mockproductService.Object);


            //Act
            CartItemDto cartItemDto = new CartItemDto("cheerios", 1);
            var result = await shoppingCartController.AddItem(cartItemDto);

            //Assert           
            Assert.IsType<OkObjectResult>(result);
            OkObjectResult okObjectResult = (OkObjectResult)result;
            okObjectResult.StatusCode.Should().Be(200);
            Assert.NotNull(okObjectResult.Value);
            Assert.IsType<CartState>(okObjectResult.Value);
            CartState resultState = (CartState)okObjectResult.Value;
            Assert.Equal(resultState.cartItems?[0].Name, cartItem.Name);
        }

        [Fact]
        public async void Post_OnProductNotFound_ReturnStatusCode400()
        {
            //Arrange
            CartItem cartItem = GetCartItem();          

            mockShoppingCartService.Setup(x => x.AddOrUpdateCart(cartItem));

            mockproductService.Setup(x => x.GetProductPrice(cartItem.Name)).ReturnsAsync(Tuple.Create<bool, decimal>(false, 0));

            var shoppingCartController = new ShoppingCartController(mockShoppingCartService.Object, _mapper, mockproductService.Object);


            //Act
            CartItemDto cartItemDto = new CartItemDto("cheerios", 1);
            var result = await shoppingCartController.AddItem(cartItemDto);

            //Assert
            Assert.IsType<BadRequestObjectResult>(result);
             BadRequestObjectResult badRequestObjectResult = (BadRequestObjectResult) result;
            badRequestObjectResult.StatusCode.Should().Be(400);
            Assert.NotNull(badRequestObjectResult.Value);
            Assert.IsType<string>(badRequestObjectResult.Value);
            string msg = (string)badRequestObjectResult.Value;
            Assert.Equal(msg, "Product not found !!!");
        }

        [Fact]
        public void Get_OnShoopingCartState_ReturnStatusCode200()
        {
            //Arrange
            CartState cartState = GetCartState();         

            mockShoppingCartService.Setup(x => x.GetCartState()).Returns(cartState);

            //Act
            var shoppingCartController = new ShoppingCartController(mockShoppingCartService.Object, _mapper, mockproductService.Object);
            var result = shoppingCartController.GetCartState();

            //Assert
            Assert.IsType<OkObjectResult>(result);
            OkObjectResult okObjectResult = (OkObjectResult)result;
            okObjectResult.StatusCode.Should().Be(200);
            Assert.NotNull(okObjectResult.Value);
            Assert.IsType<CartState>(okObjectResult.Value);
            CartState resultState = (CartState)okObjectResult.Value;
            Assert.Equal(resultState.cartItems?[0].Name, cartState.cartItems?[0].Name);
        }

        private static CartState GetCartState()
        {
            //Arrange
            CartState cartState = new CartState();
            cartState.cartItems = new List<CartItem>();
            cartState.cartItems.Add(new CartItem(1, "cheerios", 1, 2.45M));
            cartState.Subtotal = 2.45M;
            cartState.Tax = 2.45M * 12.5M / 100;
            cartState.Total = cartState.Subtotal + cartState.Tax;
            return cartState;
        }

        private  CartItem GetCartItem()
        {
            CartItem cartItem = new CartItem(1, "cheerios",1, 2.45M);           
            return cartItem;
        }


    }
}

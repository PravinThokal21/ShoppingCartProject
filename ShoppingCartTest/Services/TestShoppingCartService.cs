using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using ShoppingCart.API.Models;
using ShoppingCart.API.Services.Implementations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace ShoppingCart.UnitTests.Services
{
    public class TestShoppingCartService
    {
        ShoppingCartContext shoppingCartContext;
        ShoppingCartService shoppingCartService;        
        DbContextOptions<ShoppingCartContext> dbContextOptions;
        public TestShoppingCartService()
        {
            dbContextOptions = new DbContextOptionsBuilder<ShoppingCartContext>().UseInMemoryDatabase("ShoppingCart").Options;
            shoppingCartContext = new ShoppingCartContext(dbContextOptions);
            shoppingCartService = new ShoppingCartService(shoppingCartContext);
        }

        [Theory]
        [InlineData("cheerios","2", "8.43")]
        [InlineData("cornflakes","2", "2.52")]
        [InlineData("frosties", "2" ,"4.99")]
        [InlineData("shreddies", "2" ,"4.68")]
        [InlineData("weetabix", "2","9.98")]
        public async void  OnSuccess_AddCartItem(string name, string quantity,string price)
        {
            bool result = await shoppingCartService.AddOrUpdateCart(new API.Entity.CartItem(0,name,Convert.ToInt32(quantity),Convert.ToDecimal(price)));

            Assert.True(result);

        }

        [Fact]
        public void TestGetCartState()
        {
            var cartState = shoppingCartService.GetCartState();
            Assert.NotNull(cartState);
        }
    }
}

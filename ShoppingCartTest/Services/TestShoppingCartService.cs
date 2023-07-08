using Microsoft.EntityFrameworkCore;
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
        [Theory]
        [InlineData("cheerios","2", "8.43")]
        [InlineData("cornflakes","2", "2.52")]
        [InlineData("frosties", "2" ,"4.99")]
        [InlineData("shreddies", "2" ,"4.68")]
        [InlineData("weetabix", "2","9.98")]
        public async void  OnSuccess_AddCartItem(string name, string quantity,string price)
        {
            var dbContextOptions = new DbContextOptionsBuilder<ShoppingCartContext>().UseInMemoryDatabase("ShoppingCart").Options;
            ShoppingCartContext shoppingCartContext = new ShoppingCartContext(dbContextOptions);
            ShoppingCartService shoppingCartService = new ShoppingCartService(shoppingCartContext);

            bool result = await shoppingCartService.AddItem(new API.Entity.CartItem(0,name,Convert.ToInt32(quantity),Convert.ToDecimal(price)));

            Assert.True(result);

        }

        [Theory]
        [InlineData("cheerios", "2", "8.43")]
        public async void OnFailuer_AddCartItem(string name, string quantity, string price)
        {
            var dbContextOptions = new DbContextOptionsBuilder<ShoppingCartContext>().UseInMemoryDatabase("ShoppingCart").Options;
            ShoppingCartContext shoppingCartContext = new ShoppingCartContext(dbContextOptions);
            ShoppingCartService shoppingCartService = new ShoppingCartService(shoppingCartContext);

            bool result = await shoppingCartService.AddItem(new API.Entity.CartItem(1, name, Convert.ToInt32(quantity), Convert.ToDecimal(price)));

            Assert.False(result);

        }
    }
}

using Microsoft.Extensions.Options;
using ShoppingCart.API;
using ShoppingCart.API.Services.Implementations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace ShoppingCart.UnitTests.Services
{
    public class TestProductService
    {
        IOptions<AppSettings> options;
        HttpClient httpClient;        
        public TestProductService()
        {
            options = Options.Create<AppSettings>(new AppSettings("https://equalexperts.github.io/", "backend-take-home-test-data/{product}.json"));
            httpClient = new HttpClient();
        }

        [Theory]
        [InlineData("cheerios", "8.43")]
        [InlineData("cornflakes", "2.52")]
        [InlineData("frosties", "4.99")]
        [InlineData("shreddies", "4.68")]
        [InlineData("weetabix", "9.98")]
        public async void Test_OnSuccess_GetProductPrice(string name, string price)
        {
            ProductService productService = new ProductService(options, httpClient);

            var result = await productService.GetProductPrice(name);

            Assert.NotNull(result);
            Assert.True(result.Item1);
            Assert.Equal(result.Item2, Convert.ToDecimal(price));
        }

        [Theory]       
        [InlineData("abc")]
        public async void Test_OnFailuer_GetProductPrice(string name)
        {
            ProductService productService = new ProductService(options, httpClient);

            var result = await productService.GetProductPrice(name);

            Assert.NotNull(result);
            Assert.False(result.Item1);
        }
    }
}

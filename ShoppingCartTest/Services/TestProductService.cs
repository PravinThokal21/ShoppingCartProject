using Microsoft.Extensions.Caching.Memory;
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
        IOptions<AppSettings> appSettingsOptions;
        HttpClient httpClient;
        IMemoryCache memoryCache;
        public TestProductService()
        {
            appSettingsOptions = Options.Create<AppSettings>(new AppSettings("https://equalexperts.github.io/", "backend-take-home-test-data/{product}.json"));
            httpClient = new HttpClient();
            MemoryCacheOptions memoryCacheOptions = new MemoryCacheOptions();
            IOptions<MemoryCacheOptions> options = Options.Create<MemoryCacheOptions>(memoryCacheOptions);
            memoryCache = new MemoryCache(options);
        }

        [Theory]
        [InlineData("cheerios", "8.43")]
        [InlineData("cornflakes", "2.52")]
        [InlineData("frosties", "4.99")]
        [InlineData("shreddies", "4.68")]
        [InlineData("weetabix", "9.98")]
        public async void Test_OnSuccess_GetProductPrice(string name, string price)
        {
            ProductService productService = new ProductService(appSettingsOptions, httpClient, memoryCache);

            var result = await productService.GetProductPrice(name);

            Assert.NotNull(result);
            Assert.True(result.Item1);
            Assert.Equal(result.Item2, Convert.ToDecimal(price));
        }

        [Theory]       
        [InlineData("abc")]
        public async void Test_OnFailuer_GetProductPrice(string name)
        {
            ProductService productService = new ProductService(appSettingsOptions, httpClient, memoryCache);

            var result = await productService.GetProductPrice(name);

            Assert.NotNull(result);
            Assert.False(result.Item1);
        }
    }
}

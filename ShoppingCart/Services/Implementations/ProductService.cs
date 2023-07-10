using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using ShoppingCart.API.Services.Interfaces;

namespace ShoppingCart.API.Services.Implementations
{
    public class ProductService : IProductService
    {        
        private readonly AppSettings _appSettings ;
        private readonly HttpClient _client;
        private readonly IMemoryCache _memoryCache;

        public ProductService(IOptions<AppSettings> configuration, HttpClient client,IMemoryCache memoryCache)
        {
            _appSettings = configuration.Value;
            _client = client;
            _memoryCache = memoryCache;
        }

        public async Task<Tuple<bool,decimal>> GetProductPrice(string name)
        {
            decimal amount = 0;
            bool result = false;

            if (!_memoryCache.TryGetValue(name, out amount))
            {
                var relativeUri = _appSettings.ViewProductUri?.Replace("{product}", name);

                Uri getpriceUri = new Uri(new Uri(_appSettings.ProductBaseUri), relativeUri);

                var response = await _client.GetAsync(getpriceUri);

                if (response.IsSuccessStatusCode && response.Content != null)
                {
                    dynamic? myObject = JsonConvert.DeserializeObject<dynamic>(response.Content.ReadAsStringAsync().Result);
                    amount = Convert.ToDecimal(myObject?.price);

                    result = true;

                    var cacheEntryOptions = new MemoryCacheEntryOptions()
                    .SetSlidingExpiration(TimeSpan.FromMinutes(5));

                    _memoryCache.Set(name, amount, cacheEntryOptions);
                }
            }
            else 
            {
                 result = true;
            }

            return Tuple.Create<bool, decimal>(result,amount);
        }
    }
}

using Microsoft.Extensions.Options;
using ShoppingCart.API.Services.Interfaces;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using Newtonsoft.Json;

namespace ShoppingCart.API.Services.Implementations
{
    public class ProductService : IProductService
    {        
        private readonly AppSettings appSettings ;
        private readonly HttpClient _client;

        public ProductService(IOptions<AppSettings> configuration, HttpClient client)
        {
            appSettings = configuration.Value;
            _client = client;
        }

        public async Task<Tuple<bool,decimal>> GetProductPrice(string name)
        {
            decimal amount = 0;

            bool result = false;

            var relativeUri = appSettings.ViewProductUri?.Replace("{product}", name);
            
            Uri getpriceUri = new Uri(new Uri(appSettings.ProductBaseUri), relativeUri);

            var response = await _client.GetAsync(getpriceUri);

            if (response.IsSuccessStatusCode)
            {
                if(response.Content != null)
                {
                    dynamic? myObject = JsonConvert.DeserializeObject<dynamic>(response.Content.ReadAsStringAsync().Result);
                    amount = Convert.ToDecimal(myObject?.price);
                    result = true;
                }
                
            }

            return Tuple.Create<bool, decimal>(result,amount);
        }
    }
}

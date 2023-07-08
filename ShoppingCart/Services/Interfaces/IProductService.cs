namespace ShoppingCart.API.Services.Interfaces
{
    public interface IProductService
    {
        public  Task<Tuple<bool, decimal>> GetProductPrice(string name);
    }
}

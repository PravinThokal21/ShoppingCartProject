namespace ShoppingCart.API
{
    public class AppSettings
    {
        public AppSettings() { }
        public AppSettings(string productBaseUri, string viewProductUri) 
        {
            ProductBaseUri = productBaseUri;
            ViewProductUri = viewProductUri;
        }
        public string? ProductBaseUri { get; set; }
        public string? ViewProductUri { get; set; }
    }
}

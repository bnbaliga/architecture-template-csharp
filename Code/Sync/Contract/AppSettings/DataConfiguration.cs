namespace Vertical.Product.Service.Contract.AppSettings
{
    public class DataConfiguration
    {
        public const string DBConnectionString = "ConnectionStrings:DefaultConnection";
        public string? DefaultConnection { get; set; }    
    }
}

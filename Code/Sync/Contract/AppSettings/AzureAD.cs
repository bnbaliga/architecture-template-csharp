namespace Vertical.Product.Service.Contract.AppSettings
{
    public class AzureAD
    {
        public const string ADName = "AzureAd";
        public string? Instance { get; set; }
        public string? ClientId { get; set; }
        public string? TenantId { get; set; }
        public string? Audience { get; set; }
    }
}

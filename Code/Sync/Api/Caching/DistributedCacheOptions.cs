namespace Vertical.Product.Service.Api.Caching
{

    public class DistributedCacheOptions
    {
        public const string DistributedCache = "DistributedCache";

        public int AbsoluteExpirationInMinutes { get; set; }
        public string RedisUrl { get; set; } = string.Empty;
    }
}

using System;
using System.Threading.Tasks;

namespace Product.Functions.Cache
{
    public interface IResponseCacheService
    {
        Task CacheResponseAsync(string cacheKey, object response, TimeSpan timeToLive);
    }
}

using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Options;
using System.Net;
using System.Text;
using Vertical.Product.Service.Api.Caching;
using Vertical.Product.Service.Contract.Playground;

namespace Vertical.Product.Service.Api.Controllers.v2
{
    [ApiController]
    [Authorize]
    [Route("v2/[controller]")]
    public class CacheController : Controller
    {
        private readonly IMediator _mediator;
        private readonly ILogger<CacheController> _logger;
        private readonly IOptions<DistributedCacheOptions> _distributedCacheOptions;
        private readonly IDistributedCache _distributedCache;

        public CacheController(IMediator mediator, ILogger<CacheController> logger, IOptions<DistributedCacheOptions> distributedCacheOptions,
            IDistributedCache distributedCache)
        {
            _mediator = mediator;
            _logger = logger;
            _distributedCacheOptions = distributedCacheOptions;
            _distributedCache = distributedCache;
        }

        [HttpGet]
        [Route("GetValue1/{id}")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<ActionResult<string>> CachedDataDemo(int id)
        {
            var request = new CacheDataRequest();
            var cacheOptions = new DistributedCacheEntryOptions()
                    .SetAbsoluteExpiration(TimeSpan.FromMinutes(_distributedCacheOptions.Value.AbsoluteExpirationInMinutes));
            _distributedCache.Set("Key1", Encoding.UTF8.GetBytes(DateTime.UtcNow.ToString()), cacheOptions);

            var encodedCachedTimeUTC = await _distributedCache.GetAsync("Key1");
            if (encodedCachedTimeUTC != null)
            {
                var CachedTimeUTC = Encoding.UTF8.GetString(encodedCachedTimeUTC);
            }
            var result = await _mediator.Send(request);

            return Ok(result);

        }
    }
}

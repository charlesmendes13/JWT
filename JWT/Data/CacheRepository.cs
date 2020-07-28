using JWT.Models;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using System;

namespace JWT.Data
{
    public class CacheRepository : ICacheRepository
    {
        private readonly IDistributedCache _cache;

        public CacheRepository(IDistributedCache cache)
        {
            _cache = cache;
        }

        public RefreshToken Get(string token)
        {
            var refreshToken = _cache.GetString(token);

            if (!String.IsNullOrWhiteSpace(refreshToken))
            {
                return JsonConvert.DeserializeObject<RefreshToken>(refreshToken);
            }

            return null;  
        }

        public void Set(RefreshToken refreshToken)
        {
            var options = new DistributedCacheEntryOptions();
            options.SetAbsoluteExpiration(refreshToken.TokenExpires);

            _cache.SetString(refreshToken.Token, JsonConvert.SerializeObject(refreshToken), options);
        }
    }
}

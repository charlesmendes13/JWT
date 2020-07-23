using JWT.Models;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JWT.Data
{
    public class CacheRepository : ICacheRepository
    {
        private readonly IDistributedCache _cache;

        public CacheRepository(IDistributedCache cache)
        {
            _cache = cache;
        }

        public string Get(string token)
        {
            var refreshToken = _cache.GetString(token);

            return refreshToken;
        }

        public void Set(RefreshToken refreshToken)
        {
            DistributedCacheEntryOptions opcoesCache = new DistributedCacheEntryOptions();
            opcoesCache.SetAbsoluteExpiration(refreshToken.TokenExpires);

            _cache.SetString(refreshToken.Token, JsonConvert.SerializeObject(refreshToken), opcoesCache);
        }
    }
}

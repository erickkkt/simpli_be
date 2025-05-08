using Microsoft.Extensions.Caching.Memory;
using Sympli.SearchPortal.Application.Services.Interfaces;

namespace Sympli.SearchPortal.Application.Services
{
    public class CacheService : ICacheService
    {
        private readonly IMemoryCache _memoryCache;

        public CacheService(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
        }

        public void Set<T>(string key, T value, TimeSpan duration)
        {
            _memoryCache.Set(key, value, duration);
        }

        public bool TryGet<T>(string key, out T? value)
        {
            return _memoryCache.TryGetValue(key, out value);
        }
    }
}

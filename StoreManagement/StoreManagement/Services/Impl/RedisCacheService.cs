using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;
using System.Text;
using System.Threading.Tasks;
using StackExchange.Redis;

namespace StoreManagement.Services.Impl
{
    public class RedisCacheService : IRedisCacheService
    {
        private readonly IDistributedCache _cache;
        private readonly IDatabase _redisDatabase;
        public RedisCacheService(IDistributedCache cache, IConnectionMultiplexer connectionMultiplexer)
        {
            _cache = cache;
            _redisDatabase = connectionMultiplexer.GetDatabase();
        }

        public async Task SetCacheAsync<T>(string key, T value, TimeSpan expiration)
        {
            var options = new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = expiration
            };

            var serializedValue = JsonSerializer.Serialize(value);
            await _cache.SetStringAsync(key, serializedValue, options);
        }

        public async Task<T?> GetCacheAsync<T>(string key)
        {
            var cachedValue = await _cache.GetStringAsync(key);
            if (cachedValue == null)
                return default;

            return JsonSerializer.Deserialize<T>(cachedValue);
        }

        public async Task RemoveCacheAsync(string key)
        {
            await _cache.RemoveAsync(key);
        }

        public async Task<bool> ExistsAsync(string key)
        {
            return await _redisDatabase.KeyExistsAsync(key);
        }
    }
}
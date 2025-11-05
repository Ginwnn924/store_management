using System;
using System.Threading.Tasks;

namespace StoreManagement.Services
{
    public interface IRedisCacheService
    {
        Task SetCacheAsync<T>(string key, T value, TimeSpan expiration);
        Task<T?> GetCacheAsync<T>(string key);
        Task RemoveCacheAsync(string key);
        Task<bool> ExistsAsync(string key);

    }
}
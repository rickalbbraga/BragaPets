using System;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Distributed;

namespace BragaPets.Infra.CrossCutting.Extensions
{
    public static class DistributedCacheExtension
    {
        public static async Task SetRecordAsync<T>(
            this IDistributedCache cache, 
            string recordId, 
            T data, 
            TimeSpan? absoluteExpireTime = null,
            TimeSpan? unusedExpireTime = null
            )
        {
            var options = new DistributedCacheEntryOptions();
            options.AbsoluteExpirationRelativeToNow = absoluteExpireTime ?? TimeSpan.FromSeconds(120);
            options.SlidingExpiration = unusedExpireTime;

            var jsonData = JsonSerializer.Serialize(data);
            await cache.SetStringAsync(recordId, jsonData, options);
        }

        public static async Task<T> GetRecordAsync<T>(
            this IDistributedCache cache, string recordId)
        {
            var jsonData = await cache.GetStringAsync(recordId);

            if (jsonData is null)
                return default(T);

            return JsonSerializer.Deserialize<T>(jsonData);
        }
    }
}
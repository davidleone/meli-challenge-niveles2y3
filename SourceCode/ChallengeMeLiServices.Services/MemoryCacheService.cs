using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ChallengeMeLiServices.DataAccess.Models;
using ChallengeMeLiServices.Services.Interfaces;
using Microsoft.Extensions.Caching.Memory;

namespace ChallengeMeLiServices.Services
{
    public class MemoryCacheService : IMemoryCacheService
    {
        private MemoryCache _cache;
        private ConcurrentDictionary<string, SemaphoreSlim> _locks;
        private static IDictionary<string, Dna> _entitiesToSave = new Dictionary<string, Dna>();
        private static bool isWaitingToSave = false;

        public MemoryCacheService()
        {
            _cache = new MemoryCache(new MemoryCacheOptions());
            _locks = new ConcurrentDictionary<string, SemaphoreSlim>();
        }

        public async Task<TModel> GetAsync<TModel>(string key, Func<Task<TModel>> getFunction)
        {
            //I look for cache key
            if (!_cache.TryGetValue(key, out TModel cacheEntry))
            {
                SemaphoreSlim mylock = _locks.GetOrAdd(key, x => new SemaphoreSlim(1, 1));
                await mylock.WaitAsync();

                try
                {
                    if (!_cache.TryGetValue(key, out cacheEntry))
                    {
                        //key is not in cache, so I get data from db
                        cacheEntry = await getFunction();

                        //keep in cache for this time, reset time if accessed
                        MemoryCacheEntryOptions cacheEntryOptions = new MemoryCacheEntryOptions()
                            .SetSlidingExpiration(TimeSpan.FromSeconds(1));

                        //save data in cache
                        _cache.Set(key, cacheEntry, cacheEntryOptions);
                    }
                }
                finally
                {
                    mylock.Release();
                }
            }
            return cacheEntry;
        }

        public static async Task TriggerSaveActionAsync(Dna entityToSave, int secondsToWait, Func<ICollection<Dna>, Task> savingAction)
        {
            //I add in the list the entity to save
            if (!_entitiesToSave.ContainsKey(entityToSave.ChainString))
            {
                _entitiesToSave.Add(entityToSave.ChainString, entityToSave);
            }

            if (!isWaitingToSave)
            {
                isWaitingToSave = true;

                DateTime stopTime = DateTime.UtcNow.AddSeconds(secondsToWait);
                while (DateTime.UtcNow < stopTime) { }
                await savingAction(_entitiesToSave.Values);

                isWaitingToSave = false;
            }

            
        }
    }
}

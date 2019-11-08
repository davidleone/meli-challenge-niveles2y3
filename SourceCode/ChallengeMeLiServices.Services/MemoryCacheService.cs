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
    /// <summary>
    /// Service for Memory Cache.
    /// </summary>
    public class MemoryCacheService : IMemoryCacheService, IDisposable
    {
        /// <summary>
        /// Represents the time that an object can be stored in cache (in seconds).
        /// </summary>
        private const int k_SecondsToKeepObjectInCache = 1;

        /// <summary>
        /// Memory Cache from Microsoft.Extensions.Caching.Memory namespace.
        /// </summary>
        private MemoryCache _cache;

        /// <summary>
        /// Dictionary to handle locks in a local way, to get objects in cache.
        /// </summary>
        private ConcurrentDictionary<string, SemaphoreSlim> _locks;

        /// <summary>
        /// Entities to save in the database.
        /// key => Dna.ChainString (unique value).
        /// value => Dna.
        /// </summary>
        private static IDictionary<string, Dna> _entitiesToSave = new Dictionary<string, Dna>();

        /// <summary>
        /// true means the service is trying to save in DB and is awaiting in
        /// order to accumulate a bunch of entities to persist at the same time.
        /// </summary>
        private static bool isWaitingToSave = false;

        /// <summary>
        /// Initializes a new instance of the <see cref="MemoryCacheService"/> class.
        /// </summary>
        public MemoryCacheService()
        {
            _cache = new MemoryCache(new MemoryCacheOptions());
            _locks = new ConcurrentDictionary<string, SemaphoreSlim>();
        }

        /// <summary>
        /// Gets an object. If it's stored in cache, it returns that object; if not, it uses
        /// getFunction parameter in order to get it from database, and then store in the cache.
        /// </summary>
        /// <typeparam name="TModel">Type of hold object</typeparam>
        /// <param name="key">unique ID of stored object</param>
        /// <param name="getFunction">function to get the object from database</param>
        /// <returns>The cached object</returns>
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
                            .SetSlidingExpiration(TimeSpan.FromSeconds(k_SecondsToKeepObjectInCache));

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

        /// <summary>
        /// Saves in database a bunch of Dnas.
        /// </summary>
        /// <param name="entityToSave">entity to add in the internal list of DNAs to save</param>
        /// <param name="secondsToWait">seconds to await in order to accumulate a bunch of entities to save</param>
        /// <param name="savingAction">function to save the list in the database</param>
        /// <returns>void</returns>
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

        /// <summary>
        /// Dispose of unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Dispose of unmanaged resources.
        /// </summary>
        /// <param name="disposing">true for clean up managed resources</param>
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                // free managed resources
                if (_cache != null)
                {
                    _cache.Dispose();
                    _cache = null;
                }
            }

            // free native resources if there are any.
        }
    }
}

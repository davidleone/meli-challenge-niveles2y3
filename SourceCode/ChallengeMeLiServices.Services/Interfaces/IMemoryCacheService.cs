using System;
using System.Threading.Tasks;

namespace ChallengeMeLiServices.Services.Interfaces
{
    /// <summary>
    /// Interface for Memory Cache Service.
    /// </summary>
    public interface IMemoryCacheService
    {
        /// <summary>
        /// Gets an object. If it's stored in cache, it returns that object; if not, it uses
        /// getFunction parameter in order to get it from database, and then store in the cache.
        /// </summary>
        /// <typeparam name="TModel">Type of hold object</typeparam>
        /// <param name="key">unique ID of stored object</param>
        /// <param name="getFunction">function to get the object from database</param>
        /// <returns>The cached object</returns>
        Task<TModel> GetAsync<TModel>(string key, Func<Task<TModel>> getFunction);
    }
}

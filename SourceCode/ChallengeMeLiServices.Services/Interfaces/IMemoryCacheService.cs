using System;
using System.Threading.Tasks;

namespace ChallengeMeLiServices.Services.Interfaces
{
    public interface IMemoryCacheService
    {
        Task<TModel> GetAsync<TModel>(string key, Func<Task<TModel>> getFunction);
    }
}

using System;
using System.Threading.Tasks;
using ChallengeMeLiServices.DataAccess.Models;
using ChallengeMeLiServices.Services.Interfaces;

namespace ChallengeMeLiServices.Services
{
    /// <summary>
    /// Service for Stats.
    /// </summary>
    public class StatsService : IStatsService
    {
        /// <summary>
        /// DNA Service.
        /// </summary>
        private IDnaService _dnaService;

        /// <summary>
        /// Memory Cache Service.
        /// </summary>
        private IMemoryCacheService _memoryCacheService;

        /// <summary>
        /// Initializes a new instance of the <see cref="StatsService"/> class.
        /// </summary>
        /// <param name="dnaService">Service of DNA</param>
        /// <param name="memoryCacheService">Service for Memory Cache</param>
        public StatsService(IDnaService dnaService, IMemoryCacheService memoryCacheService)
        {
            _dnaService = dnaService;
            _memoryCacheService = memoryCacheService;
        }

        /// <summary>
        /// Get the stats of Mutants and Humans.
        /// </summary>
        /// <returns>DNA stats</returns>
        public async Task<DnaStats> GetDnaStatsAsync()
        {
            //I made both calls in parallel, using memory cache
            Task<int> mutantsTask = _memoryCacheService.GetAsync("mutantsCount", () => _dnaService.GetMutantsCountAsync());
            Task<int> humansTask = _memoryCacheService.GetAsync("humansCount", () => _dnaService.GetHumansCountAsync());

            //then, I await the results and set the ratio
            int mutants = await mutantsTask;
            int humans = await humansTask;
            decimal ratio = mutants;
            if (humans > 0)
                ratio /= humans;
            Math.Round(ratio, 2);

            //finally, I return the results
            return new DnaStats()
            {
                CountMutantDna = mutants,
                CountHumanDna = humans,
                Ratio = ratio
            };
        }
    }
}

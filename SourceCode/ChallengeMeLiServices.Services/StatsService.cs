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
        /// Initializes a new instance of the <see cref="StatsService"/> class.
        /// </summary>
        /// <param name="dnaService">Service of DNA</param>
        public StatsService(IDnaService dnaService)
        {
            _dnaService = dnaService;
        }

        /// <summary>
        /// Get the stats of Mutants and Humans.
        /// </summary>
        /// <returns>DNA stats</returns>
        public async Task<DnaStats> GetDnaStatsAsync()
        {
            //I made both calls in parallel
            Task<int> mutantsTask = _dnaService.GetMutantsCountAsync();
            Task<int> humansTask = _dnaService.GetHumansCountAsync();

            //then, I await the results and set the ratio
            int mutants = await mutantsTask;
            int humans = await humansTask;
            decimal ratio = mutants;
            if (humans > 0)
                ratio /= humans;

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

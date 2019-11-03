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
            //TODO: ver de hacerlos en paralelo
            int mutants = await _dnaService.GetMutantsCountAsync();
            int humans = await _dnaService.GetHumansCountAsync();
            decimal ratio = mutants;
            if (humans > 0)
                ratio /= humans;

            DnaStats dnaStats = new DnaStats()
            {
                CountMutantDna = mutants,
                CountHumanDna = humans,
                Ratio = ratio
            };

            return dnaStats;
        }
    }
}

using System.Collections.Generic;
using System.Threading.Tasks;
using ChallengeMeLiServices.DataAccess.Models;

namespace ChallengeMeLiServices.Services.Interfaces
{
    /// <summary>
    /// Interface for Stats Service.
    /// </summary>
    public interface IStatsService
    {
        /// <summary>
        /// Get the stats of Mutants and Humans.
        /// </summary>
        /// <returns>DNA stats</returns>
        Task<DnaStats> GetDnaStatsAsync();
    }
}

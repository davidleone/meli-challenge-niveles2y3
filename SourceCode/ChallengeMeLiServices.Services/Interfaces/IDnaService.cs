using System.Collections.Generic;
using System.Threading.Tasks;
using ChallengeMeLiServices.DataAccess.Models;

namespace ChallengeMeLiServices.Services.Interfaces
{
    /// <summary>
    /// Interface for DNA Service.
    /// </summary>
    public interface IDnaService
    {
        /// <summary>
        /// Get a specific DNA filtering by chain.
        /// </summary>
        /// <param name="chain">Dna chain</param>
        /// <returns>The fetched DNA</returns>
        Task<Dna> GetByChainAsync(string[] chain);
        
        /// <summary>
        /// Get the count of saved Mutants.
        /// </summary>
        /// <returns>Count of Mutants</returns>
        Task<int> GetMutantsCountAsync();

        /// <summary>
        /// Get the count of saved Humans.
        /// </summary>
        /// <returns>Count of Humans</returns>
        Task<int> GetHumansCountAsync();

        /// <summary>
        /// Saves in the database a bunch of DNAs.
        /// </summary>
        /// <param name="dnas">Collection of DNAs to save</param>
        /// <returns>void</returns>
        Task SaveAsync(ICollection<Dna> dnas);
    }
}

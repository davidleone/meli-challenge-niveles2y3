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
        /// Save the already verified DNA, at this point we assume that DNA is valid.
        /// </summary>
        /// <param name="chain">Dna chain</param>
        /// <param name="isMutant">true: is Mutant | false: is Human</param>
        /// <returns>void</returns>
        Task SaveVerifiedDnaAsync(string[] chain, bool isMutant);

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
    }
}

using System.Threading.Tasks;
using ChallengeMeLiServices.DataAccess.Models;

namespace ChallengeMeLiServices.Services.Interfaces
{
    /// <summary>
    /// Interface of Mutant Service.
    /// </summary>
    public interface IMutantService
    {
        /// <summary>
        /// Detects if a human is a mutant through its dna chain.
        /// </summary>
        /// <exception cref="ArgumentNullException">Thrown when human parameter is null</exception>
        /// <exception cref="DnaInvalidException">Thrown when dna chain is not valid</exception>
        /// <param name="human">Human being with a Dna chain</param>
        /// <returns>True if it's mutant; false if not.</returns>
        Task<bool> IsMutantAsync(Human human);
    }
}

namespace ChallengeMeLiServices.Services.Interfaces
{
    public interface IMutantService
    {
        /// <summary>
        /// Detects a mutant through its dna chain.
        /// </summary>
        /// <exception cref="DnaInvalidException">Thrown when dna chain is not valid.</exception>
        /// <param name="dna">Dna chain.</param>
        /// <returns>True if it's mutant; false if not.</returns>
        bool IsMutant(Human human);
    }
}

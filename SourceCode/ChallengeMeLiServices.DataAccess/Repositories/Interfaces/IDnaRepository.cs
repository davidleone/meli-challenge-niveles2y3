using System.Collections.Generic;
using ChallengeMeLiServices.DataAccess.Models;
using NHibernate;

namespace ChallengeMeLiServices.DataAccess.Repositories.Interfaces
{
    /// <summary>
    /// Interface for DNA Repository.
    /// </summary>
    public interface IDnaRepository
    {
        /// <summary>
        /// Get a list of all saved DNAs.
        /// </summary>
        /// <param name="session">NHibernate ISession</param>
        /// <returns>A list of DNAs</returns>
        IList<Dna> GetAll(ISession session);

        /// <summary>
        /// Get a specific DNA filtering by chain.
        /// </summary>
        /// <param name="session">NHibernate ISession</param>
        /// <param name="chain">Dna chain formatted in a single line</param>
        /// <returns>The fetched DNA</returns>
        Dna GetByChainString(ISession session, string chain);

        /// <summary>
        /// Save in database the dna passed by parameter.
        /// </summary>
        /// <param name="session">NHibernate ISession</param>
        /// <param name="dna">DNA to save</param>
        void Save(ISession session, Dna dna);

        /// <summary>
        /// Get the count of saved Mutants.
        /// </summary>
        /// <param name="session">NHibernate Session</param>
        /// <returns>Count of Mutants</returns>
        int GetMutantsCount(ISession session);

        /// <summary>
        /// Get the count of saved Humans.
        /// </summary>
        /// <param name="session">NHibernate Session</param>
        /// <returns>Count of Humans</returns>
        int GetHumansCount(ISession session);
    }
}

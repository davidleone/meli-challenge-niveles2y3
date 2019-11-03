using System.Collections.Generic;
using System.Linq;
using ChallengeMeLiServices.DataAccess.Daos.Interfaces;
using ChallengeMeLiServices.DataAccess.Models;
using ChallengeMeLiServices.DataAccess.Repositories.Interfaces;
using NHibernate;

namespace ChallengeMeLiServices.DataAccess.Repositories
{
    /// <summary>
    /// Repository for DNA.
    /// </summary>
    public class DnaRepository : IDnaRepository
    {
        /// <summary>
        /// DNA Dao.
        /// </summary>
        private IDnaDao _dao;

        /// <summary>
        /// Initializes a new instance of the <see cref="DnaRepository"/> class.
        /// </summary>
        /// <param name="dao">DNA dao</param>
        public DnaRepository(IDnaDao dao)
        {
            _dao = dao;
        }

        /// <summary>
        /// Get a list of all saved DNAs.
        /// </summary>
        /// <param name="session">NHibernate ISession</param>
        /// <returns>A list of DNAs</returns>
        public IList<Dna> GetAll(ISession session)
        {
            return _dao.GetAll(session)
                .ToList();
        }

        /// <summary>
        /// Get a specific DNA filtering by chain.
        /// </summary>
        /// <param name="session">NHibernate ISession</param>
        /// <param name="chain">Dna chain formatted in a single line</param>
        /// <returns>The fetched DNA</returns>
        public Dna GetByChainString(ISession session, string chain)
        {
            return _dao.GetAll(session)
                .Where(x => x.ChainString.Equals(chain))
                .ToList()
                .FirstOrDefault();
        }

        /// <summary>
        /// Save in database the dna passed by parameter.
        /// </summary>
        /// <param name="session">NHibernate ISession</param>
        /// <param name="dna">DNA to save</param>
        public void Save(ISession session, Dna dna)
        {
            _dao.Save(session, dna);
        }

        /// <summary>
        /// Get the count of saved Mutants.
        /// </summary>
        /// <param name="session">NHibernate Session</param>
        /// <returns>Count of Mutants</returns>
        public int GetMutantsCount(ISession session)
        {
            return _dao.GetAll(session)
                .Where(x => x.IsMutant)
                .Count();
        }

        /// <summary>
        /// Get the count of saved Humans.
        /// </summary>
        /// <param name="session">NHibernate Session</param>
        /// <returns>Count of Humans</returns>
        public int GetHumansCount(ISession session)
        {
            return _dao.GetAll(session)
                .Where(x => !x.IsMutant)
                .Count();
        }
    }
}

using System.Linq;
using ChallengeMeLiServices.DataAccess.Daos.Interfaces;
using ChallengeMeLiServices.DataAccess.Models;
using NHibernate;

namespace ChallengeMeLiServices.DataAccess.Daos
{
    /// <summary>
    /// Dao for DNA
    /// </summary>
    public class DnaDao : IDnaDao
    {
        /// <summary>
        /// Get all the DNAs saved in database, without any filter.
        /// </summary>
        /// <param name="session">NHibernate ISession</param>
        /// <returns>IQueryable of DNAs</returns>
        public IQueryable<Dna> GetAll(ISession session)
        {
            return session.Query<Dna>();
        }

        /// <summary>
        /// Save in database the current DNA.
        /// </summary>
        /// <param name="session">NHibernate ISession</param>
        /// <param name="dna">Dna to save</param>
        public void Save(ISession session, Dna dna)
        {
            session.Save(dna);
        }
    }
}

using System.Linq;
using ChallengeMeLiServices.DataAccess.Models;
using NHibernate;

namespace ChallengeMeLiServices.DataAccess.Daos.Interfaces
{
    /// <summary>
    /// Interface for Dna Dao.
    /// </summary>
    public interface IDnaDao
    {
        /// <summary>
        /// Get all the DNAs saved in database, without any filter.
        /// </summary>
        /// <param name="session">NHibernate ISession</param>
        /// <returns>IQueryable of DNAs</returns>
        IQueryable<Dna> GetAll(ISession session);

        /// <summary>
        /// Save in database the current DNA.
        /// </summary>
        /// <param name="session">NHibernate ISession</param>
        /// <param name="dna">Dna to save</param>
        void Save(ISession session, Dna dna);
    }
}

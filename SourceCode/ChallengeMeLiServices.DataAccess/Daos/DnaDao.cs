using System.Linq;
using ChallengeMeLiServices.DataAccess.Daos.Interfaces;
using ChallengeMeLiServices.DataAccess.Models;
using NHibernate;

namespace ChallengeMeLiServices.DataAccess.Daos
{
    public class DnaDao : IDnaDao
    {
        public IQueryable<Dna> GetAll(ISession session)
        {
            return session.Query<Dna>();
        }
    }
}

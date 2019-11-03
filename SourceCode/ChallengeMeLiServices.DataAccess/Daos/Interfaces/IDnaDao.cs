using System.Linq;
using ChallengeMeLiServices.DataAccess.Models;
using NHibernate;

namespace ChallengeMeLiServices.DataAccess.Daos.Interfaces
{
    public interface IDnaDao
    {
        IQueryable<Dna> GetAll(ISession session);
    }
}

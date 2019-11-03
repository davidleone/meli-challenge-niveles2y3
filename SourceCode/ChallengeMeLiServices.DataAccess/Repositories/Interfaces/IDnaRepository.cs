using System.Collections.Generic;
using ChallengeMeLiServices.DataAccess.Models;
using NHibernate;

namespace ChallengeMeLiServices.DataAccess.Repositories.Interfaces
{
    public interface IDnaRepository
    {
        IList<Dna> GetAll(ISession session);
    }
}

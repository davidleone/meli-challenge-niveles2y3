using System.Collections.Generic;
using System.Linq;
using ChallengeMeLiServices.DataAccess.Daos.Interfaces;
using ChallengeMeLiServices.DataAccess.Models;
using ChallengeMeLiServices.DataAccess.Repositories.Interfaces;
using NHibernate;

namespace ChallengeMeLiServices.DataAccess.Repositories
{
    public class DnaRepository : IDnaRepository
    {
        private IDnaDao _dao;

        /// <summary>
        /// Initializes a new instance of the <see cref="DnaRepository"/> class.
        /// </summary>
        /// <param name="dao">DNA dao</param>
        public DnaRepository(IDnaDao dao)
        {
            _dao = dao;
        }


        /*public Dna GetById(ISession session, Guid id)
        {
            session.Query<Dna>()
                .Where(x => x.Id == id)
                .FirstOrDefault(;
        }*/

        public IList<Dna> GetAll(ISession session)
        {
            return _dao.GetAll(session)
                .ToList();
        }
    }
}

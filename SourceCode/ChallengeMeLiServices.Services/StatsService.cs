using System.Collections.Generic;
using System.Threading.Tasks;
using ChallengeMeLiServices.DataAccess;
using ChallengeMeLiServices.DataAccess.Models;
using ChallengeMeLiServices.DataAccess.Repositories.Interfaces;
using ChallengeMeLiServices.Services.Interfaces;
using NHibernate;

namespace ChallengeMeLiServices.Services
{
    /// <summary>
    /// Service for Stats.
    /// </summary>
    public class StatsService : IStatsService
    {
        private IDnaRepository _dnaRepo;

        public StatsService(IDnaRepository dnaRepo)
        {
            _dnaRepo = dnaRepo;
        }


        public async Task<IList<Dna>> GetAllAsync()
        {
            IList<Dna> result = await Task.Run(() =>
            {
                ISession session = SessionManager.GetSession();
                return _dnaRepo.GetAll(session);
                
            });

            return result;
        }
    }
}

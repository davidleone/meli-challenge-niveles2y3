using System;
using System.Threading.Tasks;
using ChallengeMeLiServices.DataAccess;
using ChallengeMeLiServices.DataAccess.Models;
using ChallengeMeLiServices.DataAccess.Repositories.Interfaces;
using ChallengeMeLiServices.Services.Interfaces;
using NHibernate;

namespace ChallengeMeLiServices.Services
{
    /// <summary>
    /// Service for DNA.
    /// </summary>
    public class DnaService : IDnaService
    {
        /// <summary>
        /// DNA Repository.
        /// </summary>
        private IDnaRepository _dnaRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="DnaService"/> class.
        /// </summary>
        /// <param name="dnaRepository">Repository of DNA</param>
        public DnaService(IDnaRepository dnaRepository)
        {
            _dnaRepository = dnaRepository;
        }

        /// <summary>
        /// Get a specific DNA filtering by chain.
        /// </summary>
        /// <param name="chain">Dna chain</param>
        /// <returns>The fetched DNA</returns>
        public async Task<Dna> GetByChainAsync(string[] chain)
        {
            if (chain == null || chain.Length == 0)
            {
                throw new ArgumentException("Chain cannot be null or empty");
            }

            string chainString = string.Join(",", chain);

            return await Task.Run(() =>
            {
                using (ISession session = SessionManager.GetSession())
                {
                    return _dnaRepository.GetByChainString(session, chainString);
                }
            });
        }

        /// <summary>
        /// Save the already verified DNA, at this point we assume that DNA is valid.
        /// </summary>
        /// <param name="chain">Dna chain</param>
        /// <param name="isMutant">true: is Mutant | false: is Human</param>
        /// <returns>void</returns>
        public async Task SaveVerifiedDnaAsync(string[] chain, bool isMutant)
        {
            if (chain == null || chain.Length == 0)
            {
                throw new ArgumentException("Chain cannot be null or empty");
            }

            Dna dnaToSave = new Dna()
            {
                ChainString = string.Join(",", chain),
                IsMutant = isMutant
            };

            await Task.Run(() =>
            {
                using (ISession session = SessionManager.GetSession())
                {
                    using (ITransaction tx = session.BeginTransaction())
                    {
                        //I need to check it again in the transaction to cover concurrency issues
                        Dna savedDna = _dnaRepository.GetByChainString(session, dnaToSave.ChainString);
                        if (savedDna == null)
                        {
                            _dnaRepository.Save(session, dnaToSave);
                        }
                        tx.Commit();
                    }
                }
            });
        }

        /// <summary>
        /// Get the count of saved Mutants.
        /// </summary>
        /// <returns>Count of Mutants</returns>
        public async Task<int> GetMutantsCountAsync()
        {
            return await Task.Run(() =>
            {
                using (ISession session = SessionManager.GetSession())
                {
                    return _dnaRepository.GetMutantsCount(session);
                }
            });
        }

        /// <summary>
        /// Get the count of saved Humans.
        /// </summary>
        /// <returns>Count of Humans</returns>
        public async Task<int> GetHumansCountAsync()
        {
            return await Task.Run(() =>
            {
                using (ISession session = SessionManager.GetSession())
                {
                    return _dnaRepository.GetHumansCount(session);
                }
            });
        }
    }
}

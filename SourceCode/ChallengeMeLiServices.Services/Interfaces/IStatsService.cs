using System.Collections.Generic;
using System.Threading.Tasks;
using ChallengeMeLiServices.DataAccess.Models;

namespace ChallengeMeLiServices.Services.Interfaces
{
    /// <summary>
    /// Interface for Stats Service.
    /// </summary>
    public interface IStatsService
    {
        Task<IList<Dna>> GetAllAsync();
    }
}

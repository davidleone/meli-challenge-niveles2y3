using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using AutoMapper;
using ChallengeMeLiServices.DataAccess.Models;
using ChallengeMeLiServices.Services.Interfaces;
using ChallengeMeLiServices.Web.AutoMapper;
using ChallengeMeLiServices.Web.Models;

namespace ChallengeMeLiServices.Web.Controllers
{
    /// <summary>
    /// Controller for Stats
    /// </summary>
    [RoutePrefix(WebApiConfig.RootApiUri + "/v1/stats")]
    public class StatsV1Controller : ApiController
    {
        private readonly IMapper _autoMapper;
        private IStatsService _statsService;

        /// <summary>
        /// Initializes a new instance of the <see cref="StatsV1Controller"/> class.
        /// </summary>
        /// <param name="statsService">Service of Stats</param>
        public StatsV1Controller(IStatsService statsService)
        {
            _autoMapper = AutoMapperWeb.GetMapper();
            _statsService = statsService;
        }

        [HttpGet, Route]
        public async Task<DnaStatsV1Dto> GetDnaStatsAsync()
        {
            DnaStats dnaStats = await _statsService.GetDnaStatsAsync();
            DnaStatsV1Dto dnaStatsDto = _autoMapper.Map<DnaStatsV1Dto>(dnaStats);
            return dnaStatsDto;
        }
    }
}
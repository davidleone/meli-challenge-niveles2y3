using System.Web.Http;
using AutoMapper;
using ChallengeMeLiServices.Services.Interfaces;
using ChallengeMeLiServices.Web.AutoMapper;

namespace ChallengeMeLiServices.Web.Controllers
{
    /// <summary>
    /// Controller for Stats
    /// </summary>
    [RoutePrefix(WebApiConfig.RootApiUri + "/v1/stats")]
    public class StatsV1Controller : ApiController
    {
        private IMapper _autoMapper;
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
    }
}
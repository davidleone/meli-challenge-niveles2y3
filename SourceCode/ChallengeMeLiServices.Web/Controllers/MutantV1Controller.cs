using System.Net;
using System.Net.Http;
using System.Web.Http;
using AutoMapper;
using ChallengeMeLiServices.Services.Interfaces;
using ChallengeMeLiServices.Services.Models;
using ChallengeMeLiServices.Web.AutoMapper;
using ChallengeMeLiServices.Web.Models;

namespace ChallengeMeLiServices.Web.Controllers
{
    /// <summary>
    /// Controller for Mutants.
    /// </summary>
    [RoutePrefix(WebApiConfig.RootApiUri + "/v1/mutant")]
    public class MutantV1Controller : ApiController
    {
        private IMapper _autoMapper;
        private IMutantService _mutantService;

        /// <summary>
        /// Initializes a new instance of the <see cref="MutantV1Controller"/> class.
        /// </summary>
        /// <param name="mutantService">Service of Mutant</param>
        public MutantV1Controller(IMutantService mutantService)
        {
            _autoMapper = AutoMapperWeb.GetMapper();
            _mutantService = mutantService;
        }
        
        [HttpPost, Route]
        public HttpResponseMessage Post([FromBody] HumanV1Dto dnaDto)
        {
            Human dnaEntity = _autoMapper.Map<Human>(dnaDto);
            try
            {
                bool isMutant = _mutantService.IsMutant(dnaEntity);
                return new HttpResponseMessage(HttpStatusCode.OK);
            }
            catch
            {
                return new HttpResponseMessage(HttpStatusCode.Forbidden);
            }
        }
    }
}
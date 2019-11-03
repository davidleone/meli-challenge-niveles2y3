using System;
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

        /// <summary>
        /// POST, Detects if a human is a mutant verifying its dna chain.
        /// </summary>
        /// <param name="humanDto">JSON of a human, with a valid dna chain</param>
        /// <returns>200 if it's a mutant | 403 if it's not a mutant | 400 if there are some invalid argument | 500 if there are some not controlled exception</returns>
        [HttpPost, Route]
        public async Task<HttpResponseMessage> PostAsync([FromBody] HumanV1Dto humanDto)
        {
            HttpResponseMessage response = new HttpResponseMessage();
            try
            {
                Human human = _autoMapper.Map<Human>(humanDto);
                bool isMutant = await _mutantService.IsMutantAsync(human);
                if (isMutant)
                {
                    response.StatusCode = HttpStatusCode.OK;
                }
                else
                {
                    response.StatusCode = HttpStatusCode.Forbidden;
                }
            }
            catch (ArgumentException)
            {
                response.StatusCode = HttpStatusCode.BadRequest;
            }
            catch (Exception)
            {
                response.StatusCode = HttpStatusCode.InternalServerError;
            }
            return response;
        }
    }
}
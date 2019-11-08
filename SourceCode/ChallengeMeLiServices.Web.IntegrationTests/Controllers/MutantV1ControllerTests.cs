using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using ChallengeMeLiServices.Web.Controllers;
using ChallengeMeLiServices.Web.Models;
using ChallengeMeLiServices.Web.Unity;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ChallengeMeLiServices.Web.IntegrationTests.Controllers
{
    /// <summary>
    /// Integration Tests for Mutant V1 Controller.
    /// </summary>
    [TestClass]
    [TestCategory("Integration Tests")]
    public class MutantV1ControllerTests
    {
        [TestMethod]
        public async Task MutantV1Controller_PostAsync_Human_Succeeds()
        {
            //Arrange
            MutantV1Controller controller = UnityConfig.Resolve<MutantV1Controller>();
            HumanV1Dto dto = new HumanV1Dto()
            {
                Dna = new string[] { "AAA", "CCC", "TTT" }
            };
            
            //Action
            HttpResponseMessage result = await controller.PostAsync(dto);

            //Asserts
            Assert.IsNotNull(result);
            Assert.AreEqual(HttpStatusCode.Forbidden, result.StatusCode);
        }

        [TestMethod]
        public async Task MutantV1Controller_PostAsync_Mutant_Succeeds()
        {
            //Arrange
            MutantV1Controller controller = UnityConfig.Resolve<MutantV1Controller>();
            HumanV1Dto dto = new HumanV1Dto()
            {
                Dna = new string[] { "AAAA", "CACT", "TCTA", "TACC" }
            };

            //Action
            HttpResponseMessage result = await controller.PostAsync(dto);

            //Asserts
            Assert.IsNotNull(result);
            Assert.AreEqual(HttpStatusCode.OK, result.StatusCode);
        }

        [TestMethod]
        public async Task MutantV1Controller_PostAsync_WrongDto_Succeeds()
        {
            //Arrange
            MutantV1Controller controller = UnityConfig.Resolve<MutantV1Controller>();
            HumanV1Dto dto = new HumanV1Dto()
            {
                Dna = new string[] { "AAA", "CCC", "TTTXX" }
            };

            //Action
            HttpResponseMessage result = await controller.PostAsync(dto);

            //Asserts
            Assert.IsNotNull(result);
            Assert.AreEqual(HttpStatusCode.BadRequest, result.StatusCode);
        }
    }
}

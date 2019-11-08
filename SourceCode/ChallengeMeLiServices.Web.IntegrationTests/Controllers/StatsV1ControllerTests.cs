using System.Collections.Generic;
using System.Threading.Tasks;
using ChallengeMeLiServices.Web.Controllers;
using ChallengeMeLiServices.Web.Models;
using ChallengeMeLiServices.Web.Unity;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ChallengeMeLiServices.Web.IntegrationTests.Controllers
{
    /// <summary>
    /// Integration Tests for Stats V1 Controller.
    /// </summary>
    [TestClass]
    [TestCategory("Integration Tests")]
    public class StatsV1ControllerTests
    {
        [TestMethod]
        public async Task StatsV1Controller_GetDnaStatsAsync_ReturnsSomething_Succeeds()
        {
            //Arrange
            StatsV1Controller controller = UnityConfig.Resolve<StatsV1Controller>();

            //Action
            DnaStatsV1Dto result = await controller.GetDnaStatsAsync();

            //Asserts
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public async Task StatsV1Controller_GetDnaStatsAsync_VolumeTest50000_Succeeds()
        {
            //Arrange
            StatsV1Controller controller = UnityConfig.Resolve<StatsV1Controller>();
            IList<Task<DnaStatsV1Dto>> responsesTasks = new List<Task<DnaStatsV1Dto>>();
            IList<DnaStatsV1Dto> responses = new List<DnaStatsV1Dto>();

            //Action
            for (int i = 0; i < 50000; i++)
            {
                responsesTasks.Add(controller.GetDnaStatsAsync());
            }

            foreach (Task<DnaStatsV1Dto> task in responsesTasks)
            {
                responses.Add(await task);
            }

            //Asserts
            Assert.AreEqual(50000, responses.Count);
            foreach (DnaStatsV1Dto response in responses)
            {
                Assert.IsNotNull(response);
            }
        }
    }
}

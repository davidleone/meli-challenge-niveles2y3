using System.Threading.Tasks;
using ChallengeMeLiServices.DataAccess.Models;
using ChallengeMeLiServices.Services.Interfaces;
using ChallengeMeLiServices.Web.Controllers;
using ChallengeMeLiServices.Web.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace ChallengeMeLiServices.Web.Tests.Controllers
{
    [TestClass]
    public class StatsV1ControllerTests
    {
        private Mock<IStatsService> _statsServiceMock;

        [TestInitialize]
        public void SetUp()
        {
            _statsServiceMock = new Mock<IStatsService>();
        }

        #region Constructor

        [TestMethod]
        public void StatsV1Controller_Constructor_AllParameters_Succeeds()
        {
            //Arrange
            StatsV1Controller controller;

            //Action
            controller = GetStatsController();

            //Asserts
            Assert.IsNotNull(controller);
        }

        #endregion Constructor

        #region GetDnaStatsAsync()

        [TestMethod]
        public async Task StatsV1Controller_GetDnaStatsAsync_ReturnStatsFullyMapped_Succeeds()
        {
            //Arrange
            StatsV1Controller controller = GetStatsController();
            DnaStats stats = new DnaStats()
            {
                CountHumanDna = 100,
                CountMutantDna = 40,
                Ratio = 0.4m
            };

            _statsServiceMock.Setup(x => x.GetDnaStatsAsync()).ReturnsAsync(stats).Verifiable();

            //Action
            DnaStatsV1Dto result = await controller.GetDnaStatsAsync();

            //Asserts
            Assert.IsNotNull(result);
            Assert.AreEqual(100, result.CountHumanDna);
            Assert.AreEqual(40, result.CountMutantDna);
            Assert.AreEqual(0.4m, result.Ratio);
            _statsServiceMock.Verify(x => x.GetDnaStatsAsync(), Times.Once);
        }

        #endregion GetDnaStatsAsync()

        private StatsV1Controller GetStatsController()
        {
            return new StatsV1Controller(_statsServiceMock.Object);
        }
    }
}

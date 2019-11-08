using System;
using System.Threading.Tasks;
using ChallengeMeLiServices.DataAccess.Models;
using ChallengeMeLiServices.Services.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace ChallengeMeLiServices.Services.Tests
{
    /// <summary>
    /// Test class for Stats service.
    /// </summary>
    [TestClass]
    [TestCategory("Unit Tests")]
    public class StatsServiceTests : ServiceTests
    {
        private Mock<IDnaService> _dnaServiceMock;
        private Mock<IMemoryCacheService> _memoryCacheServiceMock;

        [TestInitialize]
        public void SetUp()
        {
            _dnaServiceMock = new Mock<IDnaService>();
            _memoryCacheServiceMock = new Mock<IMemoryCacheService>();
            MockSessionManager();
        }

        #region Constructor

        [TestMethod]
        public void StatsService_Constructor_AllParameters_Succeeds()
        {
            //Arrange
            StatsService service;

            //Action
            service = GetStatsService();

            //Asserts
            Assert.IsNotNull(service);
        }

        #endregion Constructor

        #region GetDnaStatsAsync()

        [TestMethod]
        public async Task StatsService_GetDnaStatsAsync_10MutantsAnd0Humans_Succeeds()
        {
            //Arrange
            StatsService service = GetStatsService();

            _memoryCacheServiceMock.Setup(x => x.GetAsync("mutantsCount", It.IsAny<Func<Task<int>>>())).ReturnsAsync(10).Verifiable();
            _memoryCacheServiceMock.Setup(x => x.GetAsync("humansCount", It.IsAny<Func<Task<int>>>())).ReturnsAsync(0).Verifiable();

            //Action
            DnaStats result = await service.GetDnaStatsAsync();

            //Asserts
            Assert.IsNotNull(result);
            Assert.AreEqual(10, result.CountMutantDna);
            Assert.AreEqual(0, result.CountHumanDna);
            Assert.AreEqual(10m, result.Ratio);
            _memoryCacheServiceMock.Verify(x => x.GetAsync("mutantsCount", It.IsAny<Func<Task<int>>>()), Times.Once);
            _memoryCacheServiceMock.Verify(x => x.GetAsync("humansCount", It.IsAny<Func<Task<int>>>()), Times.Once);
        }

        [TestMethod]
        public async Task StatsService_GetDnaStatsAsync_0MutantsAnd10Humans_Succeeds()
        {
            //Arrange
            StatsService service = GetStatsService();

            _memoryCacheServiceMock.Setup(x => x.GetAsync("mutantsCount", It.IsAny<Func<Task<int>>>())).ReturnsAsync(0).Verifiable();
            _memoryCacheServiceMock.Setup(x => x.GetAsync("humansCount", It.IsAny<Func<Task<int>>>())).ReturnsAsync(10).Verifiable();

            //Action
            DnaStats result = await service.GetDnaStatsAsync();

            //Asserts
            Assert.IsNotNull(result);
            Assert.AreEqual(0, result.CountMutantDna);
            Assert.AreEqual(10, result.CountHumanDna);
            Assert.AreEqual(0m, result.Ratio);
            _memoryCacheServiceMock.Verify(x => x.GetAsync("mutantsCount", It.IsAny<Func<Task<int>>>()), Times.Once);
            _memoryCacheServiceMock.Verify(x => x.GetAsync("humansCount", It.IsAny<Func<Task<int>>>()), Times.Once);
        }

        [TestMethod]
        public async Task StatsService_GetDnaStatsAsync_40MutantsAnd100Humans_Succeeds()
        {
            //Arrange
            StatsService service = GetStatsService();

            _memoryCacheServiceMock.Setup(x => x.GetAsync("mutantsCount", It.IsAny<Func<Task<int>>>())).ReturnsAsync(40).Verifiable();
            _memoryCacheServiceMock.Setup(x => x.GetAsync("humansCount", It.IsAny<Func<Task<int>>>())).ReturnsAsync(100).Verifiable();

            //Action
            DnaStats result = await service.GetDnaStatsAsync();

            //Asserts
            Assert.IsNotNull(result);
            Assert.AreEqual(40, result.CountMutantDna);
            Assert.AreEqual(100, result.CountHumanDna);
            Assert.AreEqual(0.4m, result.Ratio);
            _memoryCacheServiceMock.Verify(x => x.GetAsync("mutantsCount", It.IsAny<Func<Task<int>>>()), Times.Once);
            _memoryCacheServiceMock.Verify(x => x.GetAsync("humansCount", It.IsAny<Func<Task<int>>>()), Times.Once);
        }

        #endregion GetDnaStatsAsync()

        private StatsService GetStatsService()
        {
            return new StatsService(_dnaServiceMock.Object, _memoryCacheServiceMock.Object);
        }
    }
}

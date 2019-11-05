using System;
using ChallengeMeLiServices.DataAccess.Models;
using ChallengeMeLiServices.Services.Exceptions;
using ChallengeMeLiServices.Services.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace ChallengeMeLiServices.Services.Tests
{
    [TestClass]
    public class MutantServiceTests : ServiceTests
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
        public void MutantService_Constructor_AllParameters_Succeeds()
        {
            //Arrange
            MutantService service;

            //Action
            service = GetMutantService();

            //Asserts
            Assert.IsNotNull(service);
        }

        #endregion Constructor

        #region IsMutantAsync(Human)

        [TestMethod]
        public void MutantService_IsMutantAsync_ParameterHumanInNull_Fails()
        {
            //Arrange
            MutantService service = GetMutantService();

            //Action && Asserts
            Assert.ThrowsExceptionAsync<ArgumentNullException>(() => service.IsMutantAsync(null));
        }

        [TestMethod]
        public void MutantService_IsMutantAsync_IsDnaValidThrowsException_Fails()
        {
            //Arrange
            Mock<MutantService> serviceMock = GetMutantServiceMock();
            MutantService service = serviceMock.Object;

            serviceMock.Setup(x => x.IsDnaValid(It.IsAny<string[]>())).Throws<DnaInvalidException>();

            //Action && Asserts
            Assert.ThrowsExceptionAsync<DnaInvalidException>(() => service.IsMutantAsync(new Human()));
        }

        #endregion IsMutantAsync(Human)

        private MutantService GetMutantService()
        {
            return new MutantService(_dnaServiceMock.Object, _memoryCacheServiceMock.Object);
        }

        private Mock<MutantService> GetMutantServiceMock()
        {
            return new Mock<MutantService>(_dnaServiceMock.Object, _memoryCacheServiceMock.Object);
        }
    }
}

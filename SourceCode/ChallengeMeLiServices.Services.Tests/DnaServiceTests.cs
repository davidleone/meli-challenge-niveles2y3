using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ChallengeMeLiServices.DataAccess.Models;
using ChallengeMeLiServices.DataAccess.Repositories.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using NHibernate;

namespace ChallengeMeLiServices.Services.Tests
{
    /// <summary>
    /// Test class for DNA service.
    /// </summary>
    [TestClass]
    public class DnaServiceTests : ServiceTests
    {
        private Mock<IDnaRepository> _dnaRepositoryMock;

        [TestInitialize]
        public void SetUp()
        {
            _dnaRepositoryMock = new Mock<IDnaRepository>();
            MockSessionManager();
        }

        #region Constructor

        [TestMethod]
        public void DnaService_Constructor_AllParameters_Succeeds()
        {
            //Arrange
            DnaService service;

            //Action
            service = GetDnaService();

            //Asserts
            Assert.IsNotNull(service);
        }

        #endregion Constructor

        #region GetByChainAsync(string[])

        [TestMethod]
        public void DnaService_GetByChainAsync_WrongParameters_Fails()
        {
            //Arrange
            DnaService service = GetDnaService();
            string[] emptyArray = { };

            //Action && Asserts
            Assert.ThrowsExceptionAsync<ArgumentException>(() => service.GetByChainAsync(null));
            Assert.ThrowsExceptionAsync<ArgumentException>(() => service.GetByChainAsync(emptyArray));
        }

        [TestMethod]
        public async Task DnaService_GetByChainAsync_ValidArray_Succeeds()
        {
            //Arrange
            DnaService service = GetDnaService();
            string[] chain = { "AA", "CC" };

            Dna dna = new Dna()
            {
                Id = Guid.NewGuid(),
                ChainString = "AA,CC",
                IsMutant = false
            };
            _dnaRepositoryMock.Setup(x => x.GetByChainString(It.IsAny<ISession>(), It.IsAny<string>())).Returns(dna).Verifiable();

            //Action
            Dna result = await service.GetByChainAsync(chain);

            //Asserts
            Assert.IsNotNull(result);
            _sessionFactoryMock.Verify(x => x.OpenSession(), Times.Once);
            _dnaRepositoryMock.Verify(x => x.GetByChainString(It.IsAny<ISession>(), It.IsAny<string>()), Times.Once);
        }

        #endregion GetByChainAsync(string[])

        #region SaveAsync(ICollection<Dna>)

        [TestMethod]
        public void DnaService_SaveAsync_WrongParameters_Fails()
        {
            //Arrange
            DnaService service = GetDnaService();

            //Action && Asserts
            Assert.ThrowsExceptionAsync<ArgumentException>(() => service.SaveAsync(null));
        }

        [TestMethod]
        public async Task DnaService_SaveAsync_EmptyCollection_Succeeds()
        {
            //Arrange
            DnaService service = GetDnaService();
            ICollection<Dna> dnas = new List<Dna>();

            _dnaRepositoryMock.Setup(x => x.GetByChainString(It.IsAny<ISession>(), It.IsAny<string>())).Returns(new Dna()).Verifiable();
            _dnaRepositoryMock.Setup(x => x.Save(It.IsAny<ISession>(), It.IsAny<Dna>())).Verifiable();

            //Action
            await service.SaveAsync(dnas);

            //Asserts
            _sessionFactoryMock.Verify(x => x.OpenSession(), Times.Once);
            _sessionMock.Verify(x => x.BeginTransaction(), Times.Once);
            _transactionMock.Verify(x => x.Commit(), Times.Once);
            _dnaRepositoryMock.Verify(x => x.GetByChainString(It.IsAny<ISession>(), It.IsAny<string>()), Times.Never);
            _dnaRepositoryMock.Verify(x => x.Save(It.IsAny<ISession>(), It.IsAny<Dna>()), Times.Never);
        }

        [TestMethod]
        public async Task DnaService_SaveAsync_CollectionWithFiveDnas_Succeeds()
        {
            //Arrange
            DnaService service = GetDnaService();
            ICollection<Dna> dnas = new List<Dna>() { new Dna(), new Dna(), new Dna(), new Dna(), new Dna() };

            _dnaRepositoryMock.Setup(x => x.GetByChainString(It.IsAny<ISession>(), It.IsAny<string>())).Returns<Dna>(null).Verifiable();
            _dnaRepositoryMock.Setup(x => x.Save(It.IsAny<ISession>(), It.IsAny<Dna>())).Verifiable();

            //Action
            await service.SaveAsync(dnas);

            //Asserts
            _sessionFactoryMock.Verify(x => x.OpenSession(), Times.Once);
            _sessionMock.Verify(x => x.BeginTransaction(), Times.Once);
            _transactionMock.Verify(x => x.Commit(), Times.Once);
            _dnaRepositoryMock.Verify(x => x.GetByChainString(It.IsAny<ISession>(), It.IsAny<string>()), Times.Exactly(5));
            _dnaRepositoryMock.Verify(x => x.Save(It.IsAny<ISession>(), It.IsAny<Dna>()), Times.Exactly(5));
        }

        #endregion SaveAsync(ICollection<Dna>)

        #region GetMutantsCountAsync()

        [TestMethod]
        public async Task DnaService_GetMutantsCountAsync_5Mutants_Succeeds()
        {
            //Arrange
            DnaService service = GetDnaService();

            _dnaRepositoryMock.Setup(x => x.GetMutantsCount(It.IsAny<ISession>())).Returns(5).Verifiable();

            //Action
            int result = await service.GetMutantsCountAsync();

            //Asserts
            Assert.AreEqual(5, result);
            _sessionFactoryMock.Verify(x => x.OpenSession(), Times.Once);
            _dnaRepositoryMock.Verify(x => x.GetMutantsCount(It.IsAny<ISession>()), Times.Once);
        }

        #endregion GetMutantsCountAsync()

        #region GetHumansCountAsync()

        [TestMethod]
        public async Task DnaService_GetHumansCountAsync_5Humans_Succeeds()
        {
            //Arrange
            DnaService service = GetDnaService();

            _dnaRepositoryMock.Setup(x => x.GetHumansCount(It.IsAny<ISession>())).Returns(5).Verifiable();

            //Action
            int result = await service.GetHumansCountAsync();

            //Asserts
            Assert.AreEqual(5, result);
            _sessionFactoryMock.Verify(x => x.OpenSession(), Times.Once);
            _dnaRepositoryMock.Verify(x => x.GetHumansCount(It.IsAny<ISession>()), Times.Once);
        }

        #endregion GetMutantsCountAsync()

        private DnaService GetDnaService()
        {
            return new DnaService(_dnaRepositoryMock.Object);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using ChallengeMeLiServices.DataAccess.Daos.Interfaces;
using ChallengeMeLiServices.DataAccess.Models;
using ChallengeMeLiServices.DataAccess.Repositories;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using NHibernate;

namespace ChallengeMeLiServices.DataAccess.Tests.Repositories
{
    /// <summary>
    /// Test class for Dna Repository.
    /// </summary>
    [TestClass]
    public class DnaRepositoryTests
    {
        private Mock<IDnaDao> _dnaDaoMock;

        [TestInitialize]
        public void SetUp()
        {
            _dnaDaoMock = new Mock<IDnaDao>();
        }

        #region Constructor

        [TestMethod]
        public void DnaRepository_Constructor_AllParameters_Succeeds()
        {
            //Arrange
            DnaRepository repository;

            //Action
            repository = GetDnaRepository();

            //Asserts
            Assert.IsNotNull(repository);
        }

        #endregion Constructor

        #region GetAll(ISession)

        [TestMethod]
        public void DnaRepository_GetAll_EmptyList_Succeeds()
        {
            //Arrange
            DnaRepository repository = GetDnaRepository();
            Mock<ISession> sessionMock = new Mock<ISession>();
            IList<Dna> list = new List<Dna>();

            _dnaDaoMock.Setup(x => x.GetAll(It.IsAny<ISession>())).Returns(list.AsQueryable()).Verifiable();

            //Action
            IList<Dna> result = repository.GetAll(sessionMock.Object);

            //Asserts
            Assert.IsNotNull(result);
            Assert.IsFalse(result.Any());
            _dnaDaoMock.Verify(x => x.GetAll(It.IsAny<ISession>()), Times.Once);
        }

        [TestMethod]
        public void DnaRepository_GetAll_FullyList_Succeeds()
        {
            //Arrange
            DnaRepository repository = GetDnaRepository();
            Mock<ISession> sessionMock = new Mock<ISession>();
            IList<Dna> list = new List<Dna>()
            {
                new Dna(), new Dna(), new Dna(), new Dna(), new Dna()
            };

            _dnaDaoMock.Setup(x => x.GetAll(It.IsAny<ISession>())).Returns(list.AsQueryable()).Verifiable();

            //Action
            IList<Dna> result = repository.GetAll(sessionMock.Object);

            //Asserts
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Any());
            Assert.AreEqual(5, result.Count);
            _dnaDaoMock.Verify(x => x.GetAll(It.IsAny<ISession>()), Times.Once);
        }

        #endregion GetAll(ISession)

        #region GetByChainString(ISession, string)

        [TestMethod]
        public void DnaRepository_GetByChainString_ChainDoesNotExist_Succeeds()
        {
            //Arrange
            DnaRepository repository = GetDnaRepository();
            Mock<ISession> sessionMock = new Mock<ISession>();
            IList<Dna> list = new List<Dna>()
            {
                new Dna()
                {
                    ChainString = "AAA,CCC,TTT"
                },
                new Dna()
                {
                    ChainString = "CCC,CCC,TTT"
                },
                new Dna()
                {
                    ChainString = "TTT,TTT,TTT"
                }
            };
            string chain = "AAA,AAA,AAA";

            _dnaDaoMock.Setup(x => x.GetAll(It.IsAny<ISession>())).Returns(list.AsQueryable()).Verifiable();

            //Action
            Dna result = repository.GetByChainString(sessionMock.Object, chain);

            //Asserts
            Assert.IsNull(result);
            _dnaDaoMock.Verify(x => x.GetAll(It.IsAny<ISession>()), Times.Once);
        }

        [TestMethod]
        public void DnaRepository_GetByChainString_ChainDoesExist_Succeeds()
        {
            //Arrange
            DnaRepository repository = GetDnaRepository();
            Mock<ISession> sessionMock = new Mock<ISession>();
            Guid foundId = Guid.NewGuid();
            IList<Dna> list = new List<Dna>()
            {
                new Dna()
                {
                    ChainString = "AAA,CCC,TTT"
                },
                new Dna()
                {
                    Id = foundId,
                    ChainString = "CCC,CCC,TTT"
                },
                new Dna()
                {
                    ChainString = "TTT,TTT,TTT"
                }
            };
            string chain = "CCC,CCC,TTT"; //second one

            _dnaDaoMock.Setup(x => x.GetAll(It.IsAny<ISession>())).Returns(list.AsQueryable()).Verifiable();

            //Action
            Dna result = repository.GetByChainString(sessionMock.Object, chain);

            //Asserts
            Assert.IsNotNull(result);
            Assert.AreEqual(foundId, result.Id);
            Assert.AreEqual(chain, result.ChainString);
            _dnaDaoMock.Verify(x => x.GetAll(It.IsAny<ISession>()), Times.Once);
        }

        #endregion GetByChainString(ISession, string)

        #region Save(ISession, Dna)

        [TestMethod]
        public void DnaRepository_Save_SavingDna_Succeeds()
        {
            //Arrange
            DnaRepository repository = GetDnaRepository();
            Mock<ISession> sessionMock = new Mock<ISession>();
            Dna dna = new Dna()
            {
                Id = Guid.NewGuid(),
                ChainString = "CCC,CCC,TTT",
                IsMutant = true
            };

            _dnaDaoMock.Setup(x => x.Save(It.IsAny<ISession>(), It.IsAny<Dna>())).Verifiable();

            //Action
            repository.Save(sessionMock.Object, dna);

            //Asserts
            _dnaDaoMock.Verify(x => x.Save(It.IsAny<ISession>(), It.IsAny<Dna>()), Times.Once);
        }

        #endregion Save(ISession, Dna)

        #region GetMutantsCount(ISession)

        [TestMethod]
        public void DnaRepository_Save_0MutantsAnd2Humans_Succeeds()
        {
            //Arrange
            DnaRepository repository = GetDnaRepository();
            Mock<ISession> sessionMock = new Mock<ISession>();
            IList<Dna> list = new List<Dna>()
            {
                new Dna() { IsMutant = false },
                new Dna() { IsMutant = false }
            };

            _dnaDaoMock.Setup(x => x.GetAll(It.IsAny<ISession>())).Returns(list.AsQueryable()).Verifiable();

            //Action
            int result = repository.GetMutantsCount(sessionMock.Object);

            //Asserts
            Assert.AreEqual(0, result);
            _dnaDaoMock.Verify(x => x.GetAll(It.IsAny<ISession>()), Times.Once);
        }

        [TestMethod]
        public void DnaRepository_Save_2MutantsAnd2Humans_Succeeds()
        {
            //Arrange
            DnaRepository repository = GetDnaRepository();
            Mock<ISession> sessionMock = new Mock<ISession>();
            IList<Dna> list = new List<Dna>()
            {
                new Dna() { IsMutant = false },
                new Dna() { IsMutant = true },
                new Dna() { IsMutant = false },
                new Dna() { IsMutant = true }
            };

            _dnaDaoMock.Setup(x => x.GetAll(It.IsAny<ISession>())).Returns(list.AsQueryable()).Verifiable();

            //Action
            int result = repository.GetMutantsCount(sessionMock.Object);

            //Asserts
            Assert.AreEqual(2, result);
            _dnaDaoMock.Verify(x => x.GetAll(It.IsAny<ISession>()), Times.Once);
        }

        #endregion GetMutantsCount(ISession)

        #region GetHumansCount(ISession)

        [TestMethod]
        public void DnaRepository_Save_0HumansAnd2Mutants_Succeeds()
        {
            //Arrange
            DnaRepository repository = GetDnaRepository();
            Mock<ISession> sessionMock = new Mock<ISession>();
            IList<Dna> list = new List<Dna>()
            {
                new Dna() { IsMutant = true },
                new Dna() { IsMutant = true }
            };

            _dnaDaoMock.Setup(x => x.GetAll(It.IsAny<ISession>())).Returns(list.AsQueryable()).Verifiable();

            //Action
            int result = repository.GetHumansCount(sessionMock.Object);

            //Asserts
            Assert.AreEqual(0, result);
            _dnaDaoMock.Verify(x => x.GetAll(It.IsAny<ISession>()), Times.Once);
        }

        [TestMethod]
        public void DnaRepository_Save_3HumansAnd2Mutants_Succeeds()
        {
            //Arrange
            DnaRepository repository = GetDnaRepository();
            Mock<ISession> sessionMock = new Mock<ISession>();
            IList<Dna> list = new List<Dna>()
            {
                new Dna() { IsMutant = false },
                new Dna() { IsMutant = true },
                new Dna() { IsMutant = false },
                new Dna() { IsMutant = true },
                new Dna() { IsMutant = false }
            };

            _dnaDaoMock.Setup(x => x.GetAll(It.IsAny<ISession>())).Returns(list.AsQueryable()).Verifiable();

            //Action
            int result = repository.GetHumansCount(sessionMock.Object);

            //Asserts
            Assert.AreEqual(3, result);
            _dnaDaoMock.Verify(x => x.GetAll(It.IsAny<ISession>()), Times.Once);
        }

        #endregion GetHumansCount(ISession)

        private DnaRepository GetDnaRepository()
        {
            return new DnaRepository(_dnaDaoMock.Object);
        }
    }
}

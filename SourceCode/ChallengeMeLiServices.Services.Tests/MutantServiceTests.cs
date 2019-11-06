using System;
using System.Collections.Generic;
using System.Threading.Tasks;
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

            serviceMock.Setup(x => x.IsDnaValid(It.IsAny<string[]>())).Throws<DnaInvalidException>().Verifiable();

            //Action && Asserts
            Assert.ThrowsExceptionAsync<DnaInvalidException>(() => service.IsMutantAsync(new Human()));
            serviceMock.Verify(x => x.IsDnaValid(It.IsAny<string[]>()), Times.Once);
        }

        [TestMethod]
        public async Task MutantService_IsMutantAsync_InvalidDna_Succeeds()
        {
            //Arrange
            Mock<MutantService> serviceMock = GetMutantServiceMock();
            MutantService service = serviceMock.Object;

            serviceMock.Setup(x => x.IsDnaValid(It.IsAny<string[]>())).Returns(false).Verifiable();
            _memoryCacheServiceMock.Setup(x => x.GetAsync(It.IsAny<string>(), It.IsAny<Func<Task<Dna>>>())).ReturnsAsync(new Dna()).Verifiable();

            //Action
            bool result = await service.IsMutantAsync(new Human());

            //Asserts
            Assert.IsFalse(result);
            serviceMock.Verify(x => x.IsDnaValid(It.IsAny<string[]>()), Times.Once);
            _memoryCacheServiceMock.Verify(x => x.GetAsync(It.IsAny<string>(), It.IsAny<Func<Task<Dna>>>()), Times.Never);
        }

        [TestMethod]
        public async Task MutantService_IsMutantAsync_ValidDnaAlreadySaved_Succeeds()
        {
            //Arrange
            Mock<MutantService> serviceMock = GetMutantServiceMock();
            MutantService service = serviceMock.Object;
            Human human = new Human()
            {
                Dna = new string[] { "AAA", "CCC", "TTT" }
            };
            Dna savedDna = new Dna()
            {
                Id = Guid.NewGuid(),
                ChainString = "AAA,CCC,TTT",
                IsMutant = true
            };

            serviceMock.Setup(x => x.IsDnaValid(It.IsAny<string[]>())).Returns(true).Verifiable();
            _memoryCacheServiceMock.Setup(x => x.GetAsync(It.IsAny<string>(), It.IsAny<Func<Task<Dna>>>())).ReturnsAsync(savedDna).Verifiable();
            serviceMock.Setup(x => x.VerifyIsMutant(It.IsAny<string[]>())).Returns(true).Verifiable();

            //Action
            bool result = await service.IsMutantAsync(human);

            //Asserts
            Assert.IsTrue(result);
            serviceMock.Verify(x => x.IsDnaValid(It.IsAny<string[]>()), Times.Once);
            _memoryCacheServiceMock.Verify(x => x.GetAsync(It.IsAny<string>(), It.IsAny<Func<Task<Dna>>>()), Times.Once);
            serviceMock.Verify(x => x.VerifyIsMutant(It.IsAny<string[]>()), Times.Never);
        }

        [TestMethod]
        public async Task MutantService_IsMutantAsync_ValidDnaNonSaved_Succeeds()
        {
            //Arrange
            Mock<MutantService> serviceMock = GetMutantServiceMock();
            MutantService service = serviceMock.Object;
            Human human = new Human()
            {
                Dna = new string[] { "AAA", "CCC", "TTT" }
            };

            serviceMock.Setup(x => x.IsDnaValid(It.IsAny<string[]>())).Returns(true).Verifiable();
            _memoryCacheServiceMock.Setup(x => x.GetAsync(It.IsAny<string>(), It.IsAny<Func<Task<Dna>>>())).ReturnsAsync((Dna)null).Verifiable();
            serviceMock.Setup(x => x.VerifyIsMutant(It.IsAny<string[]>())).Returns(true).Verifiable();
            _dnaServiceMock.Setup(x => x.SaveAsync(It.IsAny<ICollection<Dna>>())).Verifiable();

            //Action
            bool result = await service.IsMutantAsync(human);

            //Asserts
            Assert.IsTrue(result);
            serviceMock.Verify(x => x.IsDnaValid(It.IsAny<string[]>()), Times.Once);
            _memoryCacheServiceMock.Verify(x => x.GetAsync(It.IsAny<string>(), It.IsAny<Func<Task<Dna>>>()), Times.Once);
            serviceMock.Verify(x => x.VerifyIsMutant(It.IsAny<string[]>()), Times.Once);
            _dnaServiceMock.Verify(x => x.SaveAsync(It.IsAny<ICollection<Dna>>()), Times.Once);
        }

        #endregion IsMutantAsync(Human)

        #region VerifyIsMutant(string[])

        [TestMethod]
        public void MutantService_VerifyIsMutant_WrongParameters_Fails()
        {
            //Arrange
            MutantService service = GetMutantService();

            //Action && Asserts
            Assert.ThrowsException<ArgumentException>(() => service.VerifyIsMutant(null));
            Assert.ThrowsException<ArgumentException>(() => service.VerifyIsMutant(new string[] { }));
        }

        /// <summary>
        /// DNA no-mutant 5x5.
        /// </summary>
        [TestMethod]
        public void MutantService_VerifyIsMutant_DnaNoMutant5x5_Succeeds()
        {
            //Arrange
            MutantService service = GetMutantService();
            string[] dna = {
                "AACCT",
                "ACTGC",
                "CCTGG",
                "GATCC",
                "TTCGA",
            };

            //Action
            bool result = service.VerifyIsMutant(dna);

            //Asserts
            Assert.IsFalse(result);
        }

        /// <summary>
        /// DNA mutant 5x5 (horizontal case)
        /// </summary>
        [TestMethod]
        public void MutantService_VerifyIsMutant_DnaMutant5x5Horizontal_Succeeds()
        {
            //Arrange
            MutantService service = GetMutantService();
            string[] dna = {
                "AAAAT",
                "ACTGC",
                "CCTGG",
                "GATCC",
                "TTCGA",
            };

            //Action
            bool result = service.VerifyIsMutant(dna);

            //Asserts
            Assert.IsTrue(result);
        }

        /// <summary>
        /// DNA mutant 5x5 (vertical case)
        /// </summary>
        [TestMethod]
        public void MutantService_VerifyIsMutant_DnaMutant5x5Vertical_Succeeds()
        {
            //Arrange
            MutantService service = GetMutantService();
            string[] dna = {
                "AACCT",
                "ACTGC",
                "CCTGG",
                "GATCC",
                "TATGA",
            };

            //Action
            bool result = service.VerifyIsMutant(dna);

            //Asserts
            Assert.IsTrue(result);
        }

        /// <summary>
        /// DNA mutant 5x5 (diagonal down-right case)
        /// </summary>
        [TestMethod]
        public void MutantService_VerifyIsMutant_DnaMutant5x5DiagonalRight_Succeeds()
        {
            //Arrange
            MutantService service = GetMutantService();
            string[] dna = {
                "AACCT",
                "TCTGC",
                "CTAGG",
                "GATCC",
                "TTCTA",
            };

            //Action
            bool result = service.VerifyIsMutant(dna);

            //Asserts
            Assert.IsTrue(result);
        }

        /// <summary>
        /// DNA mutant 5x5 (diagonal down-left case)
        /// </summary>
        [TestMethod]
        public void MutantService_VerifyIsMutant_DnaMutant5x5DiagonalLeft_Succeeds()
        {
            //Arrange
            MutantService service = GetMutantService();
            string[] dna = {
                "AACCT",
                "ACTGT",
                "CCATG",
                "GATCC",
                "TTCGA",
            };

            //Action
            bool result = service.VerifyIsMutant(dna);

            //Asserts
            Assert.IsTrue(result);
        }

        /// <summary>
        /// DNA mutant 5x5 (horizontal case and with lower letters)
        /// </summary>
        [TestMethod]
        public void MutantService_VerifyIsMutant_DnaMutant5x5LowerLetters_Succeeds()
        {
            //Arrange
            MutantService service = GetMutantService();
            string[] dna = {
                "AACCT",
                "ACTGC",
                "ccccG",
                "GATCC",
                "TTCGA",
            };

            //Action
            bool result = service.VerifyIsMutant(dna);

            //Asserts
            Assert.IsTrue(result);
        }

        #endregion VerifyIsMutant(string[])

        #region IsDnaValid(string[])

        /// <summary>
        /// DNA in null.
        /// </summary>
        [TestMethod]
        public void MutantService_IsDnaValid_DnaInNull_Fails()
        {
            //Arrange
            MutantService service = GetMutantService();
            string[] dna = null;

            //Action & Asserts
            Assert.ThrowsException<DnaInvalidException>(() => service.IsDnaValid(dna));
        }

        /// <summary>
        /// DNA as empty array.
        /// </summary>
        [TestMethod]
        public void MutantService_IsDnaValid_DnaEmpty_Fails()
        {
            //Arrange
            MutantService service = GetMutantService();
            string[] dna = { };

            //Action & Asserts
            Assert.ThrowsException<DnaInvalidException>(() => service.IsDnaValid(dna));
        }

        /// <summary>
        /// DNA with a null value in the middle of the array.
        /// </summary>
        [TestMethod]
        public void MutantService_IsDnaValid_DnaNullInMiddle_Fails()
        {
            //Arrange
            MutantService service = GetMutantService();
            string[] dna = { "AGA", null, "CCT" };

            //Action & Asserts
            Assert.ThrowsException<DnaInvalidException>(() => service.IsDnaValid(dna));
        }

        /// <summary>
        /// DNA with a string empty value in the middle of the array.
        /// </summary>
        [TestMethod]
        public void MutantService_IsDnaValid_DnaEmptyInMiddle_Fails()
        {
            //Arrange
            MutantService service = GetMutantService();
            string[] dna = { "AGA", "   ", "CCT" };

            //Action & Asserts
            Assert.ThrowsException<DnaInvalidException>(() => service.IsDnaValid(dna));
        }

        /// <summary>
        /// DNA with values as NxM table.
        /// </summary>
        [TestMethod]
        public void MutantService_IsDnaValid_DnaNxM_Fails()
        {
            //Arrange
            MutantService service = GetMutantService();
            string[] dna = { "AGA", "AT", "CCT" };

            //Action & Asserts
            Assert.ThrowsException<DnaInvalidException>(() => service.IsDnaValid(dna));
        }

        /// <summary>
        /// DNA with an invalid letter ("W") at the beginning of the first line.
        /// </summary>
        [TestMethod]
        public void MutantService_IsDnaValid_OneInvalidLetterAtTheBeginning_Fails()
        {
            //Arrange
            MutantService service = GetMutantService();
            string[] dna = { "WAG", "ATC", "CCT" }; //==> W is invalid

            //Action & Asserts
            Assert.ThrowsException<DnaInvalidException>(() => service.IsDnaValid(dna));
        }

        /// <summary>
        /// DNA with an invalid letter ("W") in the middle of the second line.
        /// </summary>
        [TestMethod]
        public void MutantService_IsDnaValid_OneInvalidLetterInTheMiddle_Fails()
        {
            //Arrange
            MutantService service = GetMutantService();
            string[] dna = { "AGT", "AWC", "CCT" }; //==> W is invalid

            //Action & Asserts
            Assert.ThrowsException<DnaInvalidException>(() => service.IsDnaValid(dna));
        }

        /// <summary>
        /// DNA with an invalid letter ("W") at the end of the last line.
        /// </summary>
        [TestMethod]
        public void MutantService_IsDnaValid_OneInvalidLetterAtTheEnd_Fails()
        {
            //Arrange
            MutantService service = GetMutantService();
            string[] dna = { "AGT", "ATC", "CCW" }; //==> W is invalid

            //Action & Asserts
            Assert.ThrowsException<DnaInvalidException>(() => service.IsDnaValid(dna));
        }

        /// <summary>
        /// DNA with a white space in the middle of the second line.
        /// </summary>
        [TestMethod]
        public void MutantService_IsDnaValid_OneInvalidWhiteSpaceInTheMiddle_Fails()
        {
            //Arrange
            MutantService service = GetMutantService();
            string[] dna = { "AGT", "A C", "CCW" }; //==> " " is invalid

            //Action & Asserts
            Assert.ThrowsException<DnaInvalidException>(() => service.IsDnaValid(dna));
        }

        /// <summary>
        /// Valid DNA 2x2.
        /// </summary>
        [TestMethod]
        public void MutantService_IsDnaValid_ValidDna2x2_Succeeds()
        {
            //Arrange
            MutantService service = GetMutantService();
            string[] dna = { "AA", "AA" };

            //Action
            bool result = service.IsDnaValid(dna);

            //Asserts
            Assert.IsTrue(result);
        }

        /// <summary>
        /// Valid DNA 3x3.
        /// </summary>
        [TestMethod]
        public void MutantService_IsDnaValid_ValidDna3x3_Succeeds()
        {
            //Arrange
            MutantService service = GetMutantService();
            string[] dna = { "AGT", "ATC", "CCA" };

            //Action
            bool result = service.IsDnaValid(dna);

            //Asserts
            Assert.IsTrue(result);
        }

        /// <summary>
        /// Valid DNA 4x4.
        /// </summary>
        [TestMethod]
        public void MutantService_IsDnaValid_ValidDna4x4_Succeeds()
        {
            //Arrange
            MutantService service = GetMutantService();
            string[] dna = { "AGTA", "ATCA", "ACCA", "TGAC" };

            //Action
            bool result = service.IsDnaValid(dna);

            //Asserts
            Assert.IsTrue(result);
        }

        /// <summary>
        /// Valid DNA 5x5.
        /// </summary>
        [TestMethod]
        public void MutantService_IsDnaValid_ValidDna5x5_Succeeds()
        {
            //Arrange
            MutantService service = GetMutantService();
            string[] dna = {
                "AGATT",
                "TATGC",
                "CCAGT",
                "AACGT",
                "CCGGT"
            };

            //Action
            bool result = service.IsDnaValid(dna);

            //Asserts
            Assert.IsTrue(result);
        }

        #endregion IsDnaValid(string[])

        private MutantService GetMutantService()
        {
            return new MutantService(_dnaServiceMock.Object, _memoryCacheServiceMock.Object);
        }

        private Mock<MutantService> GetMutantServiceMock()
        {
            return new Mock<MutantService>(_dnaServiceMock.Object, _memoryCacheServiceMock.Object) { CallBase = true };
        }
    }
}

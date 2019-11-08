using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using ChallengeMeLiServices.DataAccess.Models;
using ChallengeMeLiServices.Services.Exceptions;
using ChallengeMeLiServices.Services.Interfaces;
using ChallengeMeLiServices.Web.Controllers;
using ChallengeMeLiServices.Web.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace ChallengeMeLiServices.Web.Tests.Controllers
{
    [TestClass]
    [TestCategory("Unit Tests")]
    public class MutantV1ControllerTests
    {
        private Mock<IMutantService> _mutantServiceMock;

        [TestInitialize]
        public void SetUp()
        {
            _mutantServiceMock = new Mock<IMutantService>();
        }

        #region Constructor

        [TestMethod]
        public void MutantV1Controller_Constructor_AllParameters_Succeeds()
        {
            //Arrange
            MutantV1Controller controller;

            //Action
            controller = GetMutantController();

            //Asserts
            Assert.IsNotNull(controller);
        }

        #endregion Constructor

        #region PostAsync(HumanV1Dto)

        [TestMethod]
        public async Task MutantV1Controller_PostAsync_DtoNull_Succeeds()
        {
            //Arrange
            MutantV1Controller controller = GetMutantController();

            _mutantServiceMock.Setup(x => x.IsMutantAsync(It.IsAny<Human>())).ThrowsAsync(new ArgumentNullException()).Verifiable();

            //Action
            HttpResponseMessage result = await controller.PostAsync(null);

            //Asserts
            Assert.IsNotNull(result);
            Assert.AreEqual(HttpStatusCode.BadRequest, result.StatusCode);
            _mutantServiceMock.Verify(x => x.IsMutantAsync(It.IsAny<Human>()), Times.Once);
        }

        [TestMethod]
        public async Task MutantV1Controller_PostAsync_DtoInvalid_Succeeds()
        {
            //Arrange
            MutantV1Controller controller = GetMutantController();
            HumanV1Dto dto = new HumanV1Dto();

            _mutantServiceMock.Setup(x => x.IsMutantAsync(It.IsAny<Human>())).ThrowsAsync(new DnaInvalidException()).Verifiable();

            //Action
            HttpResponseMessage result = await controller.PostAsync(dto);

            //Asserts
            Assert.IsNotNull(result);
            Assert.AreEqual(HttpStatusCode.BadRequest, result.StatusCode);
            _mutantServiceMock.Verify(x => x.IsMutantAsync(It.IsAny<Human>()), Times.Once);
        }

        [TestMethod]
        public async Task MutantV1Controller_PostAsync_NonHandledException_Succeeds()
        {
            //Arrange
            MutantV1Controller controller = GetMutantController();
            HumanV1Dto dto = new HumanV1Dto();

            _mutantServiceMock.Setup(x => x.IsMutantAsync(It.IsAny<Human>())).ThrowsAsync(new Exception()).Verifiable();

            //Action
            HttpResponseMessage result = await controller.PostAsync(dto);

            //Asserts
            Assert.IsNotNull(result);
            Assert.AreEqual(HttpStatusCode.InternalServerError, result.StatusCode);
            _mutantServiceMock.Verify(x => x.IsMutantAsync(It.IsAny<Human>()), Times.Once);
        }

        [TestMethod]
        public async Task MutantV1Controller_PostAsync_ValidDtoFullyMapped_Succeeds()
        {
            //Arrange
            MutantV1Controller controller = GetMutantController();
            HumanV1Dto dto = new HumanV1Dto()
            {
                Dna = new string[] { "AAA", "CCC", "TTT" }
            };

            _mutantServiceMock.Setup(x => x.IsMutantAsync(It.IsAny<Human>())).ReturnsAsync(true).Verifiable();

            //Action
            HttpResponseMessage result = await controller.PostAsync(dto);

            //Asserts
            Assert.IsNotNull(result);
            Assert.AreEqual(HttpStatusCode.OK, result.StatusCode);
            _mutantServiceMock.Verify(x => x.IsMutantAsync(It.IsAny<Human>()), Times.Once);
        }

        #endregion PostAsync(HumanV1Dto)

        private MutantV1Controller GetMutantController()
        {
            return new MutantV1Controller(_mutantServiceMock.Object);
        }
    }
}

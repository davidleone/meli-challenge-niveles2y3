using System;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ChallengeMeLiServices.Services.Tests
{
    [TestClass]
    public class MemoryCacheServiceTests : ServiceTests
    {
        [TestInitialize]
        public void SetUp()
        {
            MockSessionManager();
        }

        #region Constructor

        [TestMethod]
        public void MemoryCacheService_Constructor_AllParameters_Succeeds()
        {
            //Arrange
            MemoryCacheService service;

            //Action
            service = GetMemoryCacheService();

            //Asserts
            Assert.IsNotNull(service);
        }

        #endregion Constructor

        #region GetAsync<TModel>(string, Func<Task<TModel>>)

        [TestMethod]
        public async Task MemoryCacheService_GetAsync_NonCachedObject_Succeeds()
        {
            //Arrange
            MemoryCacheService service = GetMemoryCacheService();
            string key = "testKey";
            int getInvokedCount = 0;
            Task<int> myFunc()
            {
                getInvokedCount++;
                return Task.FromResult(10);
            }

            //Action
            int result = await service.GetAsync(key, myFunc);

            //Asserts
            Assert.AreEqual(10, result);
            Assert.AreEqual(1, getInvokedCount);
        }

        [TestMethod]
        public async Task MemoryCacheService_GetAsync_CachedObjectWithSeveralCalls_Succeeds()
        {
            //Arrange
            MemoryCacheService service = GetMemoryCacheService();
            string key = "testKey";
            int getInvokedCount = 0;
            Task<int> myFunc()
            {
                getInvokedCount++;
                return Task.FromResult(10);
            }

            //Action
            Task<int> resultTask1 = service.GetAsync(key, myFunc);
            Task<int> resultTask2 = service.GetAsync(key, myFunc);
            Task<int> resultTask3 = service.GetAsync(key, myFunc);
            Task<int> resultTask4 = service.GetAsync(key, myFunc);
            Task<int> resultTask5 = service.GetAsync(key, myFunc);
            int result1 = await resultTask1;
            int result2 = await resultTask2;
            int result3 = await resultTask3;
            int result4 = await resultTask4;
            int result5 = await resultTask5;

            //Asserts
            Assert.AreEqual(10, result1);
            Assert.AreEqual(10, result2);
            Assert.AreEqual(10, result3);
            Assert.AreEqual(10, result4);
            Assert.AreEqual(10, result5);
            Assert.AreEqual(1, getInvokedCount);
        }

        #endregion GetAsync<TModel>(string, Func<Task<TModel>>)

        private MemoryCacheService GetMemoryCacheService()
        {
            return new MemoryCacheService();
        }
    }
}

using Bettery.Kiosk.Common;
using Bettery.Kiosk.Controllers;
using Bettery.Kiosk.DataAccess;
using Bettery.Kiosk.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Bettery.Kiosk.UnitTest.Controllers
{
    /// <summary>
    ///This is a test class for GetCaseControllerTest and is intended
    ///to contain all GetCaseControllerTest Unit Tests
    ///</summary>
    [TestClass()]
    public class GetCaseControllerTest
    {
        private TestContext testContextInstance;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        #region Additional test attributes

        //
        //You can use the following additional attributes as you write your tests:
        //
        //Use ClassInitialize to run code before running the first test in the class
        //[ClassInitialize()]
        //public static void MyClassInitialize(TestContext testContext)
        //{
        //}
        //
        //Use ClassCleanup to run code after all tests in a class have run
        //[ClassCleanup()]
        //public static void MyClassCleanup()
        //{
        //}
        //
        //Use TestInitialize to run code before running each test
        //[TestInitialize()]
        //public void MyTestInitialize()
        //{
        //}
        //
        //Use TestCleanup to run code after each test has run
        //[TestCleanup()]
        //public void MyTestCleanup()
        //{
        //}
        //

        #endregion Additional test attributes

        /// <summary>
        /// Gets the max empty cases case1001 test.
        /// </summary>
        [TestMethod]
        public void GetMaxEmptyCasesCase1001Test()
        {
            BaseController.LoggedOnUser = new BetteryUser { FreeCasesRemaining = 0 };
            int expected = 0;
            int actual = GetCaseController.GetMaxEmptyCases();
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Gets the max empty cases case1002 test.
        /// </summary>
        [TestMethod]
        public void GetMaxEmptyCasesCase1002Test()
        {
            BaseController.LoggedOnUser = null;
            int expected = 0;
            int actual = GetCaseController.GetMaxEmptyCases();
            Assert.AreEqual(expected, actual);
        }
    }
}
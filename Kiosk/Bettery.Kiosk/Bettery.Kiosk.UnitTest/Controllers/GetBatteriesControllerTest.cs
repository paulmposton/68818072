using Bettery.Kiosk.Common;
using Bettery.Kiosk.Controllers;
using Bettery.Kiosk.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Bettery.Kiosk.UnitTest.Controllers
{
    /// <summary>
    ///This is a test class for GetBatteriesControllerTest and is intended
    ///to contain all GetBatteriesControllerTest Unit Tests
    ///</summary>
    [TestClass()]
    public class GetBatteriesControllerTest
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
        ///A test for GetMaxAaProduct
        ///</summary>
        [TestMethod]
        public void GetMaxAaProductTest()
        {
            int actual;
            actual = GetBatteriesController.GetMaxAaProduct() ?? 0;

            bool result = actual <= Constants.BetteryProduct.AaMax;
            Assert.IsTrue(result);
        }

        /// <summary>
        ///A test for GetMaxAaaProduct
        ///</summary>
        [TestMethod]
        public void GetMaxAaaProductTest()
        {
            int actual;
            actual = GetBatteriesController.GetMaxAaaProduct() ?? 0;

            bool result = actual <= Constants.BetteryProduct.AaaMax;
            Assert.IsTrue(result);
        }

        /// <summary>
        /// Determines whether [has total transaction not zero test].
        /// </summary>
        [TestMethod]
        public void HasTotalTransactionNotZeroCase1001Test()
        {
            BaseController.SelectedBettery = new BetteryVend
                                                 {
                                                     TotalAmount = 9.99M
                                                 };
            bool expected = true;
            bool actual = GetBatteriesController.HasTotalTransactionNotZero();
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Determines whether [has total transaction not zero test].
        /// </summary>
        [TestMethod]
        public void HasTotalTransactionNotZeroCase1002Test()
        {
            BaseController.SelectedBettery = null;
            bool expected = false;
            bool actual = GetBatteriesController.HasTotalTransactionNotZero();
            Assert.AreEqual(expected, actual);
        }
    }
}
using Bettery.Kiosk.Controllers;
using Bettery.Kiosk.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Bettery.Kiosk.UnitTest.Controllers
{
    /// <summary>
    ///This is a test class for SwapControllerTest and is intended
    ///to contain all SwapControllerTest Unit Tests
    ///</summary>
    [TestClass()]
    public class SwapControllerTest
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

        [TestMethod]
        public void HasCreditForOneArgumentCase1001Test()
        {
            int batteryPackages;
            BaseController.SelectedBettery = new BetteryVend();
            BaseController.SelectedBettery.AaReturn = 2;
            BaseController.SelectedBettery.AaaReturn = 1;
            BaseController.SelectedBettery.AaVend = 0;
            BaseController.SelectedBettery.AaaVend = 1;

            int batteryPackagesExpected = 2;
            bool expected = true;
            bool actual;
            actual = SwapController.HasCredit(out batteryPackages);
            Assert.AreEqual(batteryPackagesExpected, batteryPackages);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void HasCreditForOneArgumentCase1002Test()
        {
            int batteryPackages;
            BaseController.SelectedBettery = new BetteryVend();
            BaseController.SelectedBettery.AaReturn = 2;
            BaseController.SelectedBettery.AaaReturn = 1;
            BaseController.SelectedBettery.AaVend = 2;
            BaseController.SelectedBettery.AaaVend = 2;

            int batteryPackagesExpected = 0;
            bool expected = false;
            bool actual;
            actual = SwapController.HasCredit(out batteryPackages);
            Assert.AreEqual(batteryPackagesExpected, batteryPackages);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for HasCredit
        ///</summary>
        [TestMethod]
        public void HasCreditForNoneArgumentCase1001Test()
        {
            BaseController.SelectedBettery = new BetteryVend();
            BaseController.SelectedBettery.AaVend = 1;
            BaseController.SelectedBettery.AaaVend = 1;
            BaseController.SelectedBettery.AaReturn = 2;
            BaseController.SelectedBettery.AaaReturn = 2;

            bool expected = true;
            bool actual;
            actual = SwapController.HasCredit();
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for HasCredit
        ///</summary>
        [TestMethod]
        public void HasCreditForNoneArgumentCase1002Test()
        {
            BaseController.SelectedBettery = new BetteryVend();
            BaseController.SelectedBettery.AaVend = 2;
            BaseController.SelectedBettery.AaaVend = 2;
            BaseController.SelectedBettery.AaReturn = 2;
            BaseController.SelectedBettery.AaaReturn = 2;

            bool expected = false;
            bool actual;
            actual = SwapController.HasCredit();
            Assert.AreEqual(expected, actual);
        }
    }
}
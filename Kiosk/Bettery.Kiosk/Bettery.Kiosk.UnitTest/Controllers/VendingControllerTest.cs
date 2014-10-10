using System;
using Bettery.Kiosk.Controllers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Bettery.Kiosk.UnitTest.Controllers
{
    /// <summary>
    ///This is a test class for VendingControllerTest and is intended
    ///to contain all VendingControllerTest Unit Tests
    ///</summary>
    [TestClass()]
    public class VendingControllerTest
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
        ///A test for Vending
        ///</summary>
        [TestMethod()]
        public void VendingTest()
        {
            VendingController.Vending();
        }
    }
}
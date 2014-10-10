using System;
using Bettery.Kiosk.Controllers;
using Bettery.Kiosk.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Bettery.Kiosk.UnitTest.Controllers
{
    /// <summary>
    ///This is a test class for CountDownControllerTest and is intended
    ///to contain all CountDownControllerTest Unit Tests
    ///</summary>
    [TestClass()]
    public class CountDownControllerTest
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
        ///A test for ReturnedBatteriesExists
        ///</summary>
        [TestMethod()]
        public void ReturnedBatteriesExistsCase1001Test()
        {
            BaseController.SelectedBettery = new BetteryVend
                                                 {
                                                     AaReturn = 1
                                                 };

            bool expected = true;
            bool actual = CountDownController.ReturnedBatteriesExists();
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for ReturnedBatteriesExists
        ///</summary>
        [TestMethod()]
        public void ReturnedBatteriesExistsCase1002Test()
        {
            BaseController.SelectedBettery = null;

            bool expected = false;
            bool actual = CountDownController.ReturnedBatteriesExists();
            Assert.AreEqual(expected, actual);
        }
    }
}
using System;
using Bettery.Kiosk.Common;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Bettery.Kiosk.UnitTest.Common
{
    /// <summary>
    ///This is a test class for UtilsTest and is intended
    ///to contain all UtilsTest Unit Tests
    ///</summary>
    [TestClass()]
    public class UtilsTest
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
        ///A test for ToLowerString
        ///</summary>
        [TestMethod]
        public void ToLowerStringCase1001Test()
        {
            object value = "BetteRy";
            string expected = "bettery";
            string actual;
            actual = Utils.ToLowerString(value);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for ToLowerString
        ///</summary>
        [TestMethod]
        public void ToLowerStringCase1002Test()
        {
            object value = null;
            string expected = "";
            string actual;
            actual = Utils.ToLowerString(value);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for ToString
        ///</summary>
        [TestMethod]
        public void ToStringCase1001Test()
        {
            object value = "Bettery";
            string expected = "Bettery";
            string actual;
            actual = Utils.ToString(value);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for ToString
        ///</summary>
        [TestMethod]
        public void ToStringCase1002Test()
        {
            object value = null;
            string expected = "";
            string actual;
            actual = Utils.ToString(value);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for ToUpperString
        ///</summary>
        [TestMethod]
        public void ToUpperStringCase1001Test()
        {
            object value = "BettERy";
            string expected = "BETTERY";
            string actual;
            actual = Utils.ToUpperString(value);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for ToUpperString
        ///</summary>
        [TestMethod]
        public void ToUpperStringCase1002Test()
        {
            object value = null;
            string expected = string.Empty;
            string actual;
            actual = Utils.ToUpperString(value);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Mins the test.
        /// </summary>
        [TestMethod]
        public void MinTest()
        {
            int[] numberItems = new int[] { 4, 7, 8 };

            int expected = 4;
            int actual = Utils.Min(numberItems);

            Assert.AreEqual(expected, actual);
        }
    }
}
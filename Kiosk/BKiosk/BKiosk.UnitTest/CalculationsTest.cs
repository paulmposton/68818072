using BKiosk.HelperClasses;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BKiosk.UnitTest
{
    /// <summary>
    ///This is a test class for CalculationsTest and is intended
    ///to contain all CalculationsTest Unit Tests
    ///</summary>
    [TestClass]
    public class CalculationsTest
    {
        private TestContext testContextInstance;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get { return testContextInstance; }
            set { testContextInstance = value; }
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
        /// betteryMath.TotalCartridges > betteryMath.ReturnedCartridges
        /// </summary>
        [TestMethod]
        public void CalcChargesCase1001Test()
        {
            int aaReturn = 0;
            int aaaReturn = 0;
            int aaVend = 1;
            int aaaVend = 1;
            BetteryVend expected = new BetteryVend
            {
                AaReturn = 0,
                AaaReturn = 0,
                AaVend = 1,
                AaaVend = 1,
                Swapped = 0,
                CalculatedNew = 2,
                CalculatedNewAmount = 19.98M,
                CalculatedReturned = 0,
                CalculatedReturnedAmount = 0M,
                SwappedAmount = 0M,
                AaReturnedAmount = 0M,
                AaaReturnedAmount = 0M,
                AaNewAmount = 9.99M,
                AaaNewAmount = 9.99M,
                TotalAmount = 19.98M
            };

            BetteryVend actual;
            actual = Calculations.CalcCharges(aaReturn, aaaReturn, aaVend, aaaVend);
            Equals(expected, actual);
        }

        /// <summary>
        /// betteryMath.TotalCartridges < betteryMath.ReturnedCartridges
        /// </summary>
        [TestMethod]
        public void CalcChargesCase1002Test()
        {
            int aaReturn = 1;
            int aaaReturn = 1;
            int aaVend = 0;
            int aaaVend = 0;
            BetteryVend expected = new BetteryVend
                                       {
                                           AaReturn = 1,
                                           AaaReturn = 1,
                                           AaVend = 0,
                                           AaaVend = 0,
                                           Swapped = 0,
                                           CalculatedNew = 0,
                                           CalculatedNewAmount = 0M,
                                           CalculatedReturned = 2,
                                           CalculatedReturnedAmount = -16M,
                                           SwappedAmount = 0M,
                                           AaReturnedAmount = 8M,
                                           AaaReturnedAmount = 8M,
                                           AaNewAmount = 0M,
                                           AaaNewAmount = 0M,
                                           TotalAmount = -16M
                                       };
            BetteryVend actual;
            actual = Calculations.CalcCharges(aaReturn, aaaReturn, aaVend, aaaVend);
            Equals(expected, actual);
        }

        /// <summary>
        /// betteryMath.TotalCartridges == betteryMath.ReturnedCartridges
        /// </summary>
        [TestMethod]
        public void CalcChargesCase1003Test()
        {
            int aaReturn = 0;
            int aaaReturn = 1;
            int aaVend = 2;
            int aaaVend = 2;
            BetteryVend expected = new BetteryVend
                                       {
                                           AaReturn = 0,
                                           AaaReturn = 1,
                                           AaVend = 2,
                                           AaaVend = 2,
                                           Swapped = 1,
                                           CalculatedNew = 3,
                                           CalculatedNewAmount = 29.97M,
                                           CalculatedReturned = 0,
                                           CalculatedReturnedAmount = 0M,
                                           SwappedAmount = 1.99M,
                                           AaReturnedAmount = 0M,
                                           AaaReturnedAmount = 8M,
                                           AaNewAmount = 19.98M,
                                           AaaNewAmount = 19.98M,
                                           TotalAmount = 31.96M
                                       };
            BetteryVend actual;
            actual = Calculations.CalcCharges(aaReturn, aaaReturn, aaVend, aaaVend);
            Equals(expected, actual);
        }

        /// <summary>
        /// Equals test common
        /// </summary>
        /// <param name="expected"></param>
        /// <param name="actual"></param>
        private void Equals(BetteryVend expected, BetteryVend actual)
        {
            Assert.AreEqual(expected.AaVend, actual.AaVend, "BetteryVend.AaVend");
            Assert.AreEqual(expected.AaaVend, actual.AaaVend, "BetteryVend.AaaVend");
            Assert.AreEqual(expected.TotalCartridges, actual.TotalCartridges, "BetteryVend.TotalCartridges");
            Assert.AreEqual(expected.NewBatteries, actual.NewBatteries, "BetteryVend.NewBatteries");
            Assert.AreEqual(expected.AaReturn, actual.AaReturn, "BetteryVend.AaReturn");
            Assert.AreEqual(expected.AaaReturn, actual.AaaReturn, "BetteryVend.AaaReturn");
            Assert.AreEqual(expected.ReturnedCartridges, actual.ReturnedCartridges, "BetteryVend.ReturnedCartridges");
            Assert.AreEqual(expected.ReturnedBatteries, actual.ReturnedBatteries, "BetteryVend.ReturnedBatteries");
            Assert.AreEqual(expected.NewCartridges, actual.NewCartridges, "BetteryVend.NewCartridges");
            Assert.AreEqual(expected.Swapped, actual.Swapped, "BetteryVend.Swapped");
            Assert.AreEqual(expected.CalculatedNew, actual.CalculatedNew, "BetteryVend.CalculatedNew");
            Assert.AreEqual(expected.CalculatedNewAmount, actual.CalculatedNewAmount, "BetteryVend.CalculatedNewAmount");
            Assert.AreEqual(expected.CalculatedReturned, actual.CalculatedReturned, "BetteryVend.CalculatedReturned");
            Assert.AreEqual(expected.CalculatedReturnedAmount, actual.CalculatedReturnedAmount, "BetteryVend.CalculatedReturnedAmount");
            Assert.AreEqual(expected.SwappedAmount, actual.SwappedAmount, "BetteryVend.SwappedAmount");
            Assert.AreEqual(expected.AaReturnedAmount, actual.AaReturnedAmount, "BetteryVend.AaReturnedAmount");
            Assert.AreEqual(expected.AaaReturnedAmount, actual.AaaReturnedAmount, "BetteryVend.AaaReturnedAmount");
            Assert.AreEqual(expected.ReturnedAmount, actual.ReturnedAmount, "BetteryVend.ReturnedAmount");
            Assert.AreEqual(expected.AaNewAmount, actual.AaNewAmount, "BetteryVend.AaNewAmount");
            Assert.AreEqual(expected.AaaNewAmount, actual.AaaNewAmount, "BetteryVend.AaaNewAmount");
            Assert.AreEqual(expected.NewAmount, actual.NewAmount, "BetteryVend.NewAmount");
            Assert.AreEqual(expected.TotalAmount, actual.TotalAmount, "BetteryVend.TotalAmount");
        }
    }
}
using System;
using Bettery.Kiosk.Common;
using Bettery.Kiosk.Controllers;
using Bettery.Kiosk.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Bettery.Kiosk.UnitTest.Controllers
{
    /// <summary>
    /// This is a test class for BaseControllerTest and is intended
    /// to contain all BaseControllerTest Unit Tests
    /// </summary>
    [TestClass]
    public class BaseControllerTest
    {
        #region Logout

        /// <summary>
        ///A test for Logout
        ///</summary>
        [TestMethod]
        public void LogoutTest()
        {
            BaseController.Logout();

            Assert.IsNull(BaseController.LoggedOnUser, "BaseController.LoggedOnUser");
            Assert.IsNull(BaseController.SelectedBettery, "BaseController.SelectedBettery");
            Assert.IsFalse(BaseController.CartridgeInserted, "BaseController.CartridgeInserted");
            Assert.AreEqual("", BaseController.PreviousView, "BaseController.PreviousView");
            Assert.IsNull(BaseController.CurrentTransaction, "BaseController.CurrentTransaction");
            Assert.IsNull(BaseController.RegistrationUser, "BaseController.RegistrationUser");
            Assert.AreEqual("", BaseController.CardInfo, "BaseController.CardInfo");
            Assert.AreEqual(GetBatteriesModes.BuyNew, BaseController.GetBatteriesMode, "BaseController.GetBatteriesMode");
            Assert.AreEqual(Constants.ViewName.Start, BaseController.CurrentView, "BaseController.CurrentVie");
            Assert.AreEqual("", BaseController.RecentViewOfCurrentFlow, "BaseController.RecentViewOfCurrentFlow");
        }

        #endregion Logout

        #region ValidateFirstName

        /// <summary>
        /// Validates the first name case1001 test.
        /// </summary>
        [TestMethod]
        public void ValidateFirstNameCase1001Test()
        {
            string firstname = "";

            bool expected = false;
            bool actual = BaseController.ValidateFirstName(firstname);

            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Validates the first name case1002 test.
        /// </summary>
        [TestMethod]
        public void ValidateFirstNameCase1002Test()
        {
            string firstname = "Sopia";

            bool expected = true;
            bool actual = BaseController.ValidateFirstName(firstname);

            Assert.AreEqual(expected, actual);
        }

        #endregion ValidateFirstName

        #region ValidateLastName

        /// <summary>
        /// Validates the last name case1001 test.
        /// </summary>
        [TestMethod]
        public void ValidateLastNameCase1001Test()
        {
            string lastname = "";

            bool expected = false;
            bool actual = BaseController.ValidateFirstName(lastname);

            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Validates the last name case1002 test.
        /// </summary>
        [TestMethod]
        public void ValidateLastNameCase1002Test()
        {
            string lastname = "Victory";

            bool expected = true;
            bool actual = BaseController.ValidateFirstName(lastname);

            Assert.AreEqual(expected, actual);
        }

        #endregion ValidateLastName

        #region ValidatePassword

        /// <summary>
        /// Validates the password case1001 test.
        /// </summary>
        [TestMethod]
        public void ValidatePasswordCase1001Test()
        {
            string password = "be001";

            bool expected = false;
            bool actual = BaseController.ValidatePassword(password);

            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Validates the password case1002 test.
        /// </summary>
        [TestMethod]
        public void ValidatePasswordCase1002Test()
        {
            string password = "bettery";

            bool expected = true;
            bool actual = BaseController.ValidatePassword(password);

            Assert.AreEqual(expected, actual);
        }

        #endregion ValidatePassword

        #region ValidateConfirmPassword

        /// <summary>
        /// Validates the confirm password case1001 test.
        /// </summary>
        [TestMethod]
        public void ValidateConfirmPasswordCase1001Test()
        {
            string password = "bettery";
            string confirm = "bettery";

            bool expected = true;
            bool actual = BaseController.ValidateConfirmPassword(password, confirm);

            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Validates the confirm password case1002 test.
        /// </summary>
        [TestMethod]
        public void ValidateConfirmPasswordCase1002Test()
        {
            string password = "bettery";
            string confirm = "betteru";

            bool expected = false;
            bool actual = BaseController.ValidateConfirmPassword(password, confirm);

            Assert.AreEqual(expected, actual);
        }

        #endregion ValidateConfirmPassword

        #region ValidateZipcode

        /// <summary>
        /// Validates the zipcode case1001 test.
        /// </summary>
        [TestMethod]
        public void ValidateZipcodeCase1001Test()
        {
            string zipcode = "123";
            bool expected = false;

            bool actual = BaseController.ValidateZipcode(zipcode);

            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Validates the zipcode case1001 test.
        /// </summary>
        [TestMethod]
        public void ValidateZipcodeCase1002Test()
        {
            string zipcode = "12345";
            bool expected = true;

            bool actual = BaseController.ValidateZipcode(zipcode);

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void ValidateZipcodeCase1003Test()
        {
            string zipcode = "1234a";
            bool expected = false;

            bool actual = BaseController.ValidateZipcode(zipcode);

            Assert.AreEqual(expected, actual);
        }

        #endregion ValidateZipcode

        #region ValidateEmailAddress

        /// <summary>
        ///A test for ValidateEmailAddress
        ///</summary>
        [TestMethod]
        public void ValidateEmailAddressTest()
        {
            string emailAddress = "anhnd3@fsoft.com.vn";
            bool expected = true;
            bool actual;
            actual = BaseController.ValidateEmailAddress(emailAddress);
            Assert.AreEqual(expected, actual);
        }

        #endregion ValidateEmailAddress

        #region ChargeHelper

        #region CalcCharges For Two Argument

        /// <summary>
        /// Calcs the charges case1001 test.
        /// </summary>
        [TestMethod]
        public void CalcChargesForTwoArgumentCase1001Test()
        {
            int aaReturn = 0;
            int aaaReturn = 1;
            BaseController.SelectedBettery = null;

            // expecteds value
            int AaReturn = 0;
            int AaaReturn = 1;

            int swapped = 0;
            decimal SwappedAmount = 0.00M;

            decimal AaNewAmount = 0M;
            decimal AaaNewAmount = 0.00M;

            decimal AaNewForgotDrainedAmount = 0.00M;
            decimal AaaNewForgotDrainedAmount = 0.00M;

            int CalculatedNew = 0;
            decimal CalculatedNewAmount = 0.00M;
            int CalculatedReturned = 1;
            decimal CalculatedReturnedAmount = 8.00M;

            decimal TotalAmount = -8.00M;

            BaseController.CalcCharges(aaReturn, aaaReturn);

            Assert.IsNotNull(BaseController.SelectedBettery);
            Assert.AreEqual(AaReturn, BaseController.SelectedBettery.AaReturn);
            Assert.AreEqual(AaaReturn, BaseController.SelectedBettery.AaaReturn);

            Assert.AreEqual(swapped, BaseController.SelectedBettery.Swapped);
            Assert.AreEqual(SwappedAmount, BaseController.SelectedBettery.SwappedAmount);
            Assert.AreEqual(AaNewAmount, BaseController.SelectedBettery.CalculatedAaAmount);
            Assert.AreEqual(AaaNewAmount, BaseController.SelectedBettery.CalculatedAaaAmount);
            Assert.AreEqual(AaNewForgotDrainedAmount, BaseController.SelectedBettery.AaNewForgotDrainedAmount);
            Assert.AreEqual(AaaNewForgotDrainedAmount, BaseController.SelectedBettery.AaaNewForgotDrainedAmount);
            Assert.AreEqual(CalculatedNew, BaseController.SelectedBettery.CalculatedNew);
            Assert.AreEqual(CalculatedNewAmount, BaseController.SelectedBettery.CalculatedNewAmount);
            Assert.AreEqual(CalculatedReturned, BaseController.SelectedBettery.CalculatedReturned);
            Assert.AreEqual(CalculatedReturnedAmount, BaseController.SelectedBettery.CalculatedReturnedAmount);
            Assert.AreEqual(TotalAmount, BaseController.SelectedBettery.TotalAmount);
        }

        /// <summary>
        /// Calcs the charges case1002 test.
        /// </summary>
        [TestMethod]
        public void CalcChargesForTwoArgumentCase1002Test()
        {
            int aaReturn = 1;
            int aaaReturn = 0;
            BaseController.SelectedBettery = new BetteryVend();

            // expecteds value
            int AaReturn = 1;
            int AaaReturn = 0;

            int swapped = 0;
            decimal SwappedAmount = 0.00M;

            decimal AaNewAmount = 0M;
            decimal AaaNewAmount = 0.00M;

            decimal AaNewForgotDrainedAmount = 0.00M;
            decimal AaaNewForgotDrainedAmount = 0.00M;

            int CalculatedNew = 0;
            decimal CalculatedNewAmount = 0.00M;
            int CalculatedReturned = 1;
            decimal CalculatedReturnedAmount = 8.00M;

            decimal TotalAmount = -8.00M;

            BaseController.CalcCharges(aaReturn, aaaReturn);

            Assert.IsNotNull(BaseController.SelectedBettery);
            Assert.AreEqual(AaReturn, BaseController.SelectedBettery.AaReturn);
            Assert.AreEqual(AaaReturn, BaseController.SelectedBettery.AaaReturn);

            Assert.AreEqual(swapped, BaseController.SelectedBettery.Swapped);
            Assert.AreEqual(SwappedAmount, BaseController.SelectedBettery.SwappedAmount);
            Assert.AreEqual(AaNewAmount, BaseController.SelectedBettery.CalculatedAaAmount);
            Assert.AreEqual(AaaNewAmount, BaseController.SelectedBettery.CalculatedAaaAmount);
            Assert.AreEqual(AaNewForgotDrainedAmount, BaseController.SelectedBettery.AaNewForgotDrainedAmount);
            Assert.AreEqual(AaaNewForgotDrainedAmount, BaseController.SelectedBettery.AaaNewForgotDrainedAmount);
            Assert.AreEqual(CalculatedNew, BaseController.SelectedBettery.CalculatedNew);
            Assert.AreEqual(CalculatedNewAmount, BaseController.SelectedBettery.CalculatedNewAmount);
            Assert.AreEqual(CalculatedReturned, BaseController.SelectedBettery.CalculatedReturned);
            Assert.AreEqual(CalculatedReturnedAmount, BaseController.SelectedBettery.CalculatedReturnedAmount);
            Assert.AreEqual(TotalAmount, BaseController.SelectedBettery.TotalAmount);
        }

        #endregion CalcCharges For Two Argument

        #region CalcCharges For Three Argument

        /// <summary>
        /// Calcs the charges for three argument case1001 test.
        /// </summary>
        [TestMethod]
        public void CalcChargesForThreeArgumentCase1001Test()
        {
            int aaVend = 1;
            int aaaVend = 0;
            GetBatteriesModes getBatteriesMode = GetBatteriesModes.BuyNew;
            BaseController.SelectedBettery = null;

            // expecteds value
            int AaVend = 1;
            int AaaVend = 0;

            int swapped = 0;
            decimal SwappedAmount = 0.00M;

            decimal AaNewAmount = 9.99M;
            decimal AaaNewAmount = 0.00M;

            decimal AaNewForgotDrainedAmount = 0.00M;
            decimal AaaNewForgotDrainedAmount = 0.00M;

            int CalculatedNew = 1;
            decimal CalculatedNewAmount = 9.99M;
            int CalculatedReturned = 0;
            decimal CalculatedReturnedAmount = 0.00M;

            decimal TotalAmount = 9.99M;

            BaseController.CalcCharges(aaVend, aaaVend, getBatteriesMode);

            Assert.IsNotNull(BaseController.SelectedBettery);
            Assert.AreEqual(AaVend, BaseController.SelectedBettery.AaVend);
            Assert.AreEqual(AaaVend, BaseController.SelectedBettery.AaaVend);

            Assert.AreEqual(swapped, BaseController.SelectedBettery.Swapped);
            Assert.AreEqual(SwappedAmount, BaseController.SelectedBettery.SwappedAmount);
            Assert.AreEqual(AaNewAmount, BaseController.SelectedBettery.CalculatedAaAmount);
            Assert.AreEqual(AaaNewAmount, BaseController.SelectedBettery.CalculatedAaaAmount);
            Assert.AreEqual(AaNewForgotDrainedAmount, BaseController.SelectedBettery.AaNewForgotDrainedAmount);
            Assert.AreEqual(AaaNewForgotDrainedAmount, BaseController.SelectedBettery.AaaNewForgotDrainedAmount);
            Assert.AreEqual(CalculatedNew, BaseController.SelectedBettery.CalculatedNew);
            Assert.AreEqual(CalculatedNewAmount, BaseController.SelectedBettery.CalculatedNewAmount);
            Assert.AreEqual(CalculatedReturned, BaseController.SelectedBettery.CalculatedReturned);
            Assert.AreEqual(CalculatedReturnedAmount, BaseController.SelectedBettery.CalculatedReturnedAmount);
            Assert.AreEqual(TotalAmount, BaseController.SelectedBettery.TotalAmount);
        }

        [TestMethod]
        public void CalcChargesForThreeArgumentCase1002Test()
        {
            int aaVend = 0;
            int aaaVend = 1;
            GetBatteriesModes getBatteriesMode = GetBatteriesModes.BuyNew;
            BaseController.SelectedBettery = new BetteryVend();

            // expecteds value
            int AaVend = 0;
            int AaaVend = 1;

            int swapped = 0;
            decimal SwappedAmount = 0.00M;

            decimal AaNewAmount = 0.00M;
            decimal AaaNewAmount = 9.99M;

            decimal AaNewForgotDrainedAmount = 0.00M;
            decimal AaaNewForgotDrainedAmount = 0.00M;

            int CalculatedNew = 1;
            decimal CalculatedNewAmount = 9.99M;
            int CalculatedReturned = 0;
            decimal CalculatedReturnedAmount = 0.00M;

            decimal TotalAmount = 9.99M;

            BaseController.CalcCharges(aaVend, aaaVend, getBatteriesMode);

            Assert.IsNotNull(BaseController.SelectedBettery);
            Assert.AreEqual(AaVend, BaseController.SelectedBettery.AaVend);
            Assert.AreEqual(AaaVend, BaseController.SelectedBettery.AaaVend);

            Assert.AreEqual(swapped, BaseController.SelectedBettery.Swapped);
            Assert.AreEqual(SwappedAmount, BaseController.SelectedBettery.SwappedAmount);
            Assert.AreEqual(AaNewAmount, BaseController.SelectedBettery.CalculatedAaAmount);
            Assert.AreEqual(AaaNewAmount, BaseController.SelectedBettery.CalculatedAaaAmount);
            Assert.AreEqual(AaNewForgotDrainedAmount, BaseController.SelectedBettery.AaNewForgotDrainedAmount);
            Assert.AreEqual(AaaNewForgotDrainedAmount, BaseController.SelectedBettery.AaaNewForgotDrainedAmount);
            Assert.AreEqual(CalculatedNew, BaseController.SelectedBettery.CalculatedNew);
            Assert.AreEqual(CalculatedNewAmount, BaseController.SelectedBettery.CalculatedNewAmount);
            Assert.AreEqual(CalculatedReturned, BaseController.SelectedBettery.CalculatedReturned);
            Assert.AreEqual(CalculatedReturnedAmount, BaseController.SelectedBettery.CalculatedReturnedAmount);
            Assert.AreEqual(TotalAmount, BaseController.SelectedBettery.TotalAmount);
        }

        [TestMethod]
        public void CalcChargesForThreeArgumentCase1003Test()
        {
            int aaVend = 1;
            int aaaVend = 0;
            GetBatteriesModes getBatteriesMode = GetBatteriesModes.ForgotDrained;
            BaseController.SelectedBettery = null;

            // expecteds value
            int AaForgotDrainedVend = 1;
            int AaaForgotDrainedVend = 0;

            int swapped = 0;
            decimal SwappedAmount = 0.00M;

            decimal AaNewAmount = 0.00M;
            decimal AaaNewAmount = 0.00M;

            decimal AaNewForgotDrainedAmount = 1.99M;
            decimal AaaNewForgotDrainedAmount = 0.00M;

            int CalculatedNew = 0;
            decimal CalculatedNewAmount = 0.00M;
            int CalculatedReturned = 0;
            decimal CalculatedReturnedAmount = 0.00M;

            decimal TotalAmount = 1.99M;

            BaseController.CalcCharges(aaVend, aaaVend, getBatteriesMode);

            Assert.IsNotNull(BaseController.SelectedBettery);
            Assert.AreEqual(AaForgotDrainedVend, BaseController.SelectedBettery.AaForgotDrainedVend);
            Assert.AreEqual(AaaForgotDrainedVend, BaseController.SelectedBettery.AaaForgotDrainedVend);

            Assert.AreEqual(swapped, BaseController.SelectedBettery.Swapped);
            Assert.AreEqual(SwappedAmount, BaseController.SelectedBettery.SwappedAmount);
            Assert.AreEqual(AaNewAmount, BaseController.SelectedBettery.CalculatedAaAmount);
            Assert.AreEqual(AaaNewAmount, BaseController.SelectedBettery.CalculatedAaaAmount);
            Assert.AreEqual(AaNewForgotDrainedAmount, BaseController.SelectedBettery.AaNewForgotDrainedAmount);
            Assert.AreEqual(AaaNewForgotDrainedAmount, BaseController.SelectedBettery.AaaNewForgotDrainedAmount);
            Assert.AreEqual(CalculatedNew, BaseController.SelectedBettery.CalculatedNew);
            Assert.AreEqual(CalculatedNewAmount, BaseController.SelectedBettery.CalculatedNewAmount);
            Assert.AreEqual(CalculatedReturned, BaseController.SelectedBettery.CalculatedReturned);
            Assert.AreEqual(CalculatedReturnedAmount, BaseController.SelectedBettery.CalculatedReturnedAmount);
            Assert.AreEqual(TotalAmount, BaseController.SelectedBettery.TotalAmount);
        }

        [TestMethod]
        public void CalcChargesForThreeArgumentCase1004Test()
        {
            int aaVend = 0;
            int aaaVend = 1;
            GetBatteriesModes getBatteriesMode = GetBatteriesModes.ForgotDrained;
            BaseController.SelectedBettery = new BetteryVend();

            // expecteds value
            int AaForgotDrainedVend = 0;
            int AaaForgotDrainedVend = 1;

            int swapped = 0;
            decimal SwappedAmount = 0.00M;

            decimal AaNewAmount = 0.00M;
            decimal AaaNewAmount = 0.00M;

            decimal AaNewForgotDrainedAmount = 0.00M;
            decimal AaaNewForgotDrainedAmount = 1.99M;

            int CalculatedNew = 0;
            decimal CalculatedNewAmount = 0.00M;
            int CalculatedReturned = 0;
            decimal CalculatedReturnedAmount = 0.00M;

            decimal TotalAmount = 1.99M;

            BaseController.CalcCharges(aaVend, aaaVend, getBatteriesMode);

            Assert.IsNotNull(BaseController.SelectedBettery);
            Assert.AreEqual(AaForgotDrainedVend, BaseController.SelectedBettery.AaForgotDrainedVend);
            Assert.AreEqual(AaaForgotDrainedVend, BaseController.SelectedBettery.AaaForgotDrainedVend);

            Assert.AreEqual(swapped, BaseController.SelectedBettery.Swapped);
            Assert.AreEqual(SwappedAmount, BaseController.SelectedBettery.SwappedAmount);
            Assert.AreEqual(AaNewAmount, BaseController.SelectedBettery.CalculatedAaAmount);
            Assert.AreEqual(AaaNewAmount, BaseController.SelectedBettery.CalculatedAaaAmount);
            Assert.AreEqual(AaNewForgotDrainedAmount, BaseController.SelectedBettery.AaNewForgotDrainedAmount);
            Assert.AreEqual(AaaNewForgotDrainedAmount, BaseController.SelectedBettery.AaaNewForgotDrainedAmount);
            Assert.AreEqual(CalculatedNew, BaseController.SelectedBettery.CalculatedNew);
            Assert.AreEqual(CalculatedNewAmount, BaseController.SelectedBettery.CalculatedNewAmount);
            Assert.AreEqual(CalculatedReturned, BaseController.SelectedBettery.CalculatedReturned);
            Assert.AreEqual(CalculatedReturnedAmount, BaseController.SelectedBettery.CalculatedReturnedAmount);
            Assert.AreEqual(TotalAmount, BaseController.SelectedBettery.TotalAmount);
        }

        #endregion CalcCharges For Three Argument

        #region CalcCharges For None Argument

        /// <summary>
        /// Calcs the charges for none argument case1001 test.
        /// </summary>
        [TestMethod]
        public void CalcChargesForNoneArgumentCase1001Test()
        {
            BaseController.SelectedBettery = null;
            BaseController.LoggedOnUser = new BetteryUser
                                              {
                                                  BatteriesInPlan = 1
                                              };

            // expecteds value
            int swapped = 0;
            decimal SwappedAmount = 0.00M;

            decimal CalculatedAaAmount = 0.00M;
            decimal CalculatedAaaAmount = 0.00M;

            decimal AaNewForgotDrainedAmount = 0.00M;
            decimal AaaNewForgotDrainedAmount = 0.00M;

            int CalculatedNew = 0;
            decimal CalculatedNewAmount = 0.00M;
            int CalculatedReturned = 0;
            decimal CalculatedReturnedAmount = 0.00M;

            decimal TotalAmount = 0.00M;

            BaseController.CalcCharges();

            Assert.IsNotNull(BaseController.SelectedBettery);
            Assert.AreEqual(swapped, BaseController.SelectedBettery.Swapped);
            Assert.AreEqual(SwappedAmount, BaseController.SelectedBettery.SwappedAmount);
            Assert.AreEqual(CalculatedAaAmount, BaseController.SelectedBettery.CalculatedAaAmount);
            Assert.AreEqual(CalculatedAaaAmount, BaseController.SelectedBettery.CalculatedAaaAmount);
            Assert.AreEqual(AaNewForgotDrainedAmount, BaseController.SelectedBettery.AaNewForgotDrainedAmount);
            Assert.AreEqual(AaaNewForgotDrainedAmount, BaseController.SelectedBettery.AaaNewForgotDrainedAmount);
            Assert.AreEqual(CalculatedNew, BaseController.SelectedBettery.CalculatedNew);
            Assert.AreEqual(CalculatedNewAmount, BaseController.SelectedBettery.CalculatedNewAmount);
            Assert.AreEqual(CalculatedReturned, BaseController.SelectedBettery.CalculatedReturned);
            Assert.AreEqual(CalculatedReturnedAmount, BaseController.SelectedBettery.CalculatedReturnedAmount);
            Assert.AreEqual(TotalAmount, BaseController.SelectedBettery.TotalAmount);
        }

        /// <summary>
        /// Calcs the charges for none argument case1002 test.
        /// </summary>
        [TestMethod]
        public void CalcChargesForNoneArgumentCase1002Test()
        {
            BaseController.SelectedBettery = new BetteryVend { AaVend = 1, AaaVend = 1, AaReturn = 0, AaaReturn = 0 };
            BaseController.LoggedOnUser = new BetteryUser
            {
                BatteriesInPlan = 1
            };

            // expecteds value
            int swapped = 0;
            decimal SwappedAmount = 0.00M;

            decimal CalculatedAaAmount = 0.00M;
            decimal CalculatedAaaAmount = 0.00M;

            decimal AaNewForgotDrainedAmount = 0.00M;
            decimal AaaNewForgotDrainedAmount = 0.00M;

            int CalculatedNew = 2;
            decimal CalculatedNewAmount = 0.00M;
            int CalculatedReturned = 0;
            decimal CalculatedReturnedAmount = 0.00M;

            decimal TotalAmount = 0.00M;

            BaseController.CalcCharges();

            Assert.IsNotNull(BaseController.SelectedBettery);
            Assert.AreEqual(swapped, BaseController.SelectedBettery.Swapped);
            Assert.AreEqual(SwappedAmount, BaseController.SelectedBettery.SwappedAmount);
            Assert.AreEqual(CalculatedAaAmount, BaseController.SelectedBettery.CalculatedAaAmount);
            Assert.AreEqual(CalculatedAaaAmount, BaseController.SelectedBettery.CalculatedAaaAmount);
            Assert.AreEqual(AaNewForgotDrainedAmount, BaseController.SelectedBettery.AaNewForgotDrainedAmount);
            Assert.AreEqual(AaaNewForgotDrainedAmount, BaseController.SelectedBettery.AaaNewForgotDrainedAmount);
            Assert.AreEqual(CalculatedNew, BaseController.SelectedBettery.CalculatedNew);
            Assert.AreEqual(CalculatedNewAmount, BaseController.SelectedBettery.CalculatedNewAmount);
            Assert.AreEqual(CalculatedReturned, BaseController.SelectedBettery.CalculatedReturned);
            Assert.AreEqual(CalculatedReturnedAmount, BaseController.SelectedBettery.CalculatedReturnedAmount);
            Assert.AreEqual(TotalAmount, BaseController.SelectedBettery.TotalAmount);
        }

        /// <summary>
        /// Calcs the charges for none argument case1003 test.
        /// </summary>
        [TestMethod]
        public void CalcChargesForNoneArgumentCase1003Test()
        {
            BaseController.SelectedBettery = new BetteryVend { AaVend = 0, AaaVend = 0, AaReturn = 1, AaaReturn = 1 };
            BaseController.LoggedOnUser = new BetteryUser
            {
                BatteriesInPlan = 1
            };

            // expecteds value
            int swapped = 0;
            decimal SwappedAmount = 0.00M;

            decimal CalculatedAaAmount = 0.00M;
            decimal CalculatedAaaAmount = 0.00M;

            decimal AaNewForgotDrainedAmount = 0.00M;
            decimal AaaNewForgotDrainedAmount = 0.00M;

            int CalculatedNew = 0;
            decimal CalculatedNewAmount = 0.00M;
            int CalculatedReturned = 2;
            decimal CalculatedReturnedAmount = 0.00M;

            decimal TotalAmount = 0.00M;

            BaseController.CalcCharges();

            Assert.IsNotNull(BaseController.SelectedBettery);
            Assert.AreEqual(swapped, BaseController.SelectedBettery.Swapped);
            Assert.AreEqual(SwappedAmount, BaseController.SelectedBettery.SwappedAmount);
            Assert.AreEqual(CalculatedAaAmount, BaseController.SelectedBettery.CalculatedAaAmount);
            Assert.AreEqual(CalculatedAaaAmount, BaseController.SelectedBettery.CalculatedAaaAmount);
            Assert.AreEqual(AaNewForgotDrainedAmount, BaseController.SelectedBettery.AaNewForgotDrainedAmount);
            Assert.AreEqual(AaaNewForgotDrainedAmount, BaseController.SelectedBettery.AaaNewForgotDrainedAmount);
            Assert.AreEqual(CalculatedNew, BaseController.SelectedBettery.CalculatedNew);
            Assert.AreEqual(CalculatedNewAmount, BaseController.SelectedBettery.CalculatedNewAmount);
            Assert.AreEqual(CalculatedReturned, BaseController.SelectedBettery.CalculatedReturned);
            Assert.AreEqual(CalculatedReturnedAmount, BaseController.SelectedBettery.CalculatedReturnedAmount);
            Assert.AreEqual(TotalAmount, BaseController.SelectedBettery.TotalAmount);
        }

        /// <summary>
        /// Calcs the charges for none argument case1004 test.
        /// </summary>
        [TestMethod]
        public void CalcChargesForNoneArgumentCase1004Test()
        {
            BaseController.SelectedBettery = null;
            BaseController.LoggedOnUser = new BetteryUser
                                              {
                                                  BatteriesInPlan = 0,
                                                  OutstandingCredit = 3.00M
                                              };

            // expecteds value
            int swapped = 0;
            decimal SwappedAmount = 0.00M;

            decimal CalculatedAaAmount = 0.00M;
            decimal CalculatedAaaAmount = 0.00M;

            decimal AaNewForgotDrainedAmount = 0.00M;
            decimal AaaNewForgotDrainedAmount = 0.00M;

            int CalculatedNew = 0;
            decimal CalculatedNewAmount = 0.00M;
            int CalculatedReturned = 0;
            decimal CalculatedReturnedAmount = 0.00M;

            decimal TotalAmount = -3.00M;

            BaseController.CalcCharges();

            Assert.IsNotNull(BaseController.SelectedBettery);
            Assert.AreEqual(swapped, BaseController.SelectedBettery.Swapped);
            Assert.AreEqual(SwappedAmount, BaseController.SelectedBettery.SwappedAmount);
            Assert.AreEqual(CalculatedAaAmount, BaseController.SelectedBettery.CalculatedAaAmount);
            Assert.AreEqual(CalculatedAaaAmount, BaseController.SelectedBettery.CalculatedAaaAmount);
            Assert.AreEqual(AaNewForgotDrainedAmount, BaseController.SelectedBettery.AaNewForgotDrainedAmount);
            Assert.AreEqual(AaaNewForgotDrainedAmount, BaseController.SelectedBettery.AaaNewForgotDrainedAmount);
            Assert.AreEqual(CalculatedNew, BaseController.SelectedBettery.CalculatedNew);
            Assert.AreEqual(CalculatedNewAmount, BaseController.SelectedBettery.CalculatedNewAmount);
            Assert.AreEqual(CalculatedReturned, BaseController.SelectedBettery.CalculatedReturned);
            Assert.AreEqual(CalculatedReturnedAmount, BaseController.SelectedBettery.CalculatedReturnedAmount);
            Assert.AreEqual(TotalAmount, BaseController.SelectedBettery.TotalAmount);
        }

        /// <summary>
        /// Calcs the charges for none argument case1005 test.
        /// </summary>
        [TestMethod]
        public void CalcChargesForNoneArgumentCase1005Test()
        {
            BaseController.SelectedBettery = new BetteryVend { PromotionalAmount = 1.00M };
            BaseController.SelectedBettery.AaVend = 2;
            BaseController.SelectedBettery.AaaVend = 2;
            BaseController.SelectedBettery.AaReturn = 1;
            BaseController.SelectedBettery.AaaReturn = 1;
            BaseController.LoggedOnUser = new BetteryUser
                                              {
                                                  BatteriesInPlan = 0,
                                                  OutstandingCredit = 3.00M
                                              };

            // expecteds value
            int swapped = 2;
            decimal SwappedAmount = 3.98M;

            decimal CalculatedAaAmount = 3.98M;
            decimal CalculatedAaaAmount = 19.98M;

            decimal AaNewForgotDrainedAmount = 0M;
            decimal AaaNewForgotDrainedAmount = 0M;

            int CalculatedNew = 2;
            decimal CalculatedNewAmount = 19.98M;
            int CalculatedReturned = 0;
            decimal CalculatedReturnedAmount = 0.00M;

            decimal TotalAmount = 19.96M;

            BaseController.CalcCharges();

            Assert.IsNotNull(BaseController.SelectedBettery);
            Assert.AreEqual(swapped, BaseController.SelectedBettery.Swapped);
            Assert.AreEqual(SwappedAmount, BaseController.SelectedBettery.SwappedAmount);
            Assert.AreEqual(CalculatedAaAmount, BaseController.SelectedBettery.CalculatedAaAmount);
            Assert.AreEqual(CalculatedAaaAmount, BaseController.SelectedBettery.CalculatedAaaAmount);
            Assert.AreEqual(AaNewForgotDrainedAmount, BaseController.SelectedBettery.AaNewForgotDrainedAmount);
            Assert.AreEqual(AaaNewForgotDrainedAmount, BaseController.SelectedBettery.AaaNewForgotDrainedAmount);
            Assert.AreEqual(CalculatedNew, BaseController.SelectedBettery.CalculatedNew);
            Assert.AreEqual(CalculatedNewAmount, BaseController.SelectedBettery.CalculatedNewAmount);
            Assert.AreEqual(CalculatedReturned, BaseController.SelectedBettery.CalculatedReturned);
            Assert.AreEqual(CalculatedReturnedAmount, BaseController.SelectedBettery.CalculatedReturnedAmount);
            Assert.AreEqual(TotalAmount, BaseController.SelectedBettery.TotalAmount);
        }

        /// <summary>
        /// Calcs the charges for none argument case1006 test.
        /// </summary>
        [TestMethod]
        public void CalcChargesForNoneArgumentCase1006Test()
        {
            BaseController.SelectedBettery = new BetteryVend { PromotionalAmount = 1.00M };
            BaseController.SelectedBettery.AaVend = 2;
            BaseController.SelectedBettery.AaaVend = 0;
            BaseController.SelectedBettery.AaReturn = 1;
            BaseController.SelectedBettery.AaaReturn = 1;
            BaseController.LoggedOnUser = new BetteryUser
                                              {
                                                  BatteriesInPlan = 0,
                                                  OutstandingCredit = 3.00M
                                              };

            // expecteds value
            int swapped = 2;
            decimal SwappedAmount = 3.98M;

            decimal CalculatedAaAmount = 3.98M;
            decimal CalculatedAaaAmount = 0.00M;

            decimal AaNewForgotDrainedAmount = 0M;
            decimal AaaNewForgotDrainedAmount = 0M;

            int CalculatedNew = 0;
            decimal CalculatedNewAmount = 0.00M;
            int CalculatedReturned = 0;
            decimal CalculatedReturnedAmount = 0.00M;

            decimal TotalAmount = -0.02M;

            BaseController.CalcCharges();

            Assert.IsNotNull(BaseController.SelectedBettery);
            Assert.AreEqual(swapped, BaseController.SelectedBettery.Swapped);
            Assert.AreEqual(SwappedAmount, BaseController.SelectedBettery.SwappedAmount);
            Assert.AreEqual(CalculatedAaAmount, BaseController.SelectedBettery.CalculatedAaAmount);
            Assert.AreEqual(CalculatedAaaAmount, BaseController.SelectedBettery.CalculatedAaaAmount);
            Assert.AreEqual(AaNewForgotDrainedAmount, BaseController.SelectedBettery.AaNewForgotDrainedAmount);
            Assert.AreEqual(AaaNewForgotDrainedAmount, BaseController.SelectedBettery.AaaNewForgotDrainedAmount);
            Assert.AreEqual(CalculatedNew, BaseController.SelectedBettery.CalculatedNew);
            Assert.AreEqual(CalculatedNewAmount, BaseController.SelectedBettery.CalculatedNewAmount);
            Assert.AreEqual(CalculatedReturned, BaseController.SelectedBettery.CalculatedReturned);
            Assert.AreEqual(CalculatedReturnedAmount, BaseController.SelectedBettery.CalculatedReturnedAmount);
            Assert.AreEqual(TotalAmount, BaseController.SelectedBettery.TotalAmount);
        }

        /// <summary>
        /// Calcs the charges for none argument case1007 test.
        /// </summary>
        [TestMethod]
        public void CalcChargesForNoneArgumentCase1007Test()
        {
            BaseController.SelectedBettery = new BetteryVend { PromotionalAmount = 1.00M };
            BaseController.SelectedBettery.AaVend = 1;
            BaseController.SelectedBettery.AaaVend = 2;
            BaseController.SelectedBettery.AaReturn = 1;
            BaseController.SelectedBettery.AaaReturn = 1;
            BaseController.LoggedOnUser = new BetteryUser
                                              {
                                                  BatteriesInPlan = 0,
                                                  OutstandingCredit = 3.00M
                                              };

            // expecteds value
            int swapped = 2;
            decimal SwappedAmount = 3.98M;

            decimal CalculatedAaAmount = 1.99M;
            decimal CalculatedAaaAmount = 11.98M;

            decimal AaNewForgotDrainedAmount = 0M;
            decimal AaaNewForgotDrainedAmount = 0M;

            int CalculatedNew = 1;
            decimal CalculatedNewAmount = 9.99M;
            int CalculatedReturned = 0;
            decimal CalculatedReturnedAmount = 0.00M;

            decimal TotalAmount = 9.97M;

            BaseController.CalcCharges();

            Assert.IsNotNull(BaseController.SelectedBettery);
            Assert.AreEqual(swapped, BaseController.SelectedBettery.Swapped);
            Assert.AreEqual(SwappedAmount, BaseController.SelectedBettery.SwappedAmount);
            Assert.AreEqual(CalculatedAaAmount, BaseController.SelectedBettery.CalculatedAaAmount);
            Assert.AreEqual(CalculatedAaaAmount, BaseController.SelectedBettery.CalculatedAaaAmount);
            Assert.AreEqual(AaNewForgotDrainedAmount, BaseController.SelectedBettery.AaNewForgotDrainedAmount);
            Assert.AreEqual(AaaNewForgotDrainedAmount, BaseController.SelectedBettery.AaaNewForgotDrainedAmount);
            //Assert.AreEqual(AaReturnedAmount, BaseController.SelectedBettery.AaReturnedAmount);
            //Assert.AreEqual(AaaReturnedAmount, BaseController.SelectedBettery.AaaReturnedAmount);
            Assert.AreEqual(CalculatedNew, BaseController.SelectedBettery.CalculatedNew);
            Assert.AreEqual(CalculatedNewAmount, BaseController.SelectedBettery.CalculatedNewAmount);
            Assert.AreEqual(CalculatedReturned, BaseController.SelectedBettery.CalculatedReturned);
            Assert.AreEqual(CalculatedReturnedAmount, BaseController.SelectedBettery.CalculatedReturnedAmount);
            Assert.AreEqual(TotalAmount, BaseController.SelectedBettery.TotalAmount);
        }

        /// <summary>
        /// Calcs the charges for none argument case1008 test.
        /// </summary>
        [TestMethod]
        public void CalcChargesForNoneArgumentCase1008Test()
        {
            BaseController.SelectedBettery = new BetteryVend { PromotionalAmount = 1.00M };
            BaseController.SelectedBettery.AaVend = 0;
            BaseController.SelectedBettery.AaaVend = 2;
            BaseController.SelectedBettery.AaReturn = 1;
            BaseController.SelectedBettery.AaaReturn = 1;

            BaseController.LoggedOnUser = new BetteryUser
                                              {
                                                  BatteriesInPlan = 0,
                                                  OutstandingCredit = 3.00M
                                              };

            // expecteds value
            int swapped = 2;
            decimal SwappedAmount = 3.98M;

            decimal CalculatedAaAmount = 0.00M;
            decimal CalculatedAaaAmount = 3.98M;

            decimal AaNewForgotDrainedAmount = 0M;
            decimal AaaNewForgotDrainedAmount = 0M;

            int CalculatedNew = 0;
            decimal CalculatedNewAmount = 0.00M;
            int CalculatedReturned = 0;
            decimal CalculatedReturnedAmount = 0.00M;

            decimal TotalAmount = -0.02M;

            BaseController.CalcCharges();

            Assert.IsNotNull(BaseController.SelectedBettery);
            Assert.AreEqual(swapped, BaseController.SelectedBettery.Swapped);
            Assert.AreEqual(SwappedAmount, BaseController.SelectedBettery.SwappedAmount);
            Assert.AreEqual(CalculatedAaAmount, BaseController.SelectedBettery.CalculatedAaAmount);
            Assert.AreEqual(CalculatedAaaAmount, BaseController.SelectedBettery.CalculatedAaaAmount);
            Assert.AreEqual(AaNewForgotDrainedAmount, BaseController.SelectedBettery.AaNewForgotDrainedAmount);
            Assert.AreEqual(AaaNewForgotDrainedAmount, BaseController.SelectedBettery.AaaNewForgotDrainedAmount);
            Assert.AreEqual(CalculatedNew, BaseController.SelectedBettery.CalculatedNew);
            Assert.AreEqual(CalculatedNewAmount, BaseController.SelectedBettery.CalculatedNewAmount);
            Assert.AreEqual(CalculatedReturned, BaseController.SelectedBettery.CalculatedReturned);
            Assert.AreEqual(CalculatedReturnedAmount, BaseController.SelectedBettery.CalculatedReturnedAmount);
            Assert.AreEqual(TotalAmount, BaseController.SelectedBettery.TotalAmount);
        }

        /// <summary>
        /// Calcs the charges for none argument case1009 test.
        /// </summary>
        [TestMethod]
        public void CalcChargesForNoneArgumentCase1009Test()
        {
            BaseController.SelectedBettery = new BetteryVend { PromotionalAmount = 1.00M };
            BaseController.SelectedBettery.AaVend = 0;
            BaseController.SelectedBettery.AaaVend = 0;
            BaseController.SelectedBettery.AaReturn = 2;
            BaseController.SelectedBettery.AaaReturn = 2;

            BaseController.LoggedOnUser = new BetteryUser
                                              {
                                                  BatteriesInPlan = 0,
                                                  OutstandingCredit = 3.00M
                                              };

            // expecteds value
            int swapped = 0;
            decimal SwappedAmount = 0.00M;

            decimal CalculatedAaAmount = 0.00M;
            decimal CalculatedAaaAmount = 0.00M;

            decimal AaNewForgotDrainedAmount = 0M;
            decimal AaaNewForgotDrainedAmount = 0M;

            int CalculatedNew = 0;
            decimal CalculatedNewAmount = 0.0M;
            int CalculatedReturned = 4;
            decimal CalculatedReturnedAmount = 32.0M;

            decimal TotalAmount = -36.0M;

            BaseController.CalcCharges();

            Assert.IsNotNull(BaseController.SelectedBettery);
            Assert.AreEqual(swapped, BaseController.SelectedBettery.Swapped);
            Assert.AreEqual(SwappedAmount, BaseController.SelectedBettery.SwappedAmount);
            Assert.AreEqual(CalculatedAaAmount, BaseController.SelectedBettery.CalculatedAaAmount);
            Assert.AreEqual(CalculatedAaaAmount, BaseController.SelectedBettery.CalculatedAaaAmount);
            Assert.AreEqual(AaNewForgotDrainedAmount, BaseController.SelectedBettery.AaNewForgotDrainedAmount);
            Assert.AreEqual(AaaNewForgotDrainedAmount, BaseController.SelectedBettery.AaaNewForgotDrainedAmount);
            Assert.AreEqual(CalculatedNew, BaseController.SelectedBettery.CalculatedNew);
            Assert.AreEqual(CalculatedNewAmount, BaseController.SelectedBettery.CalculatedNewAmount);
            Assert.AreEqual(CalculatedReturned, BaseController.SelectedBettery.CalculatedReturned);
            Assert.AreEqual(CalculatedReturnedAmount, BaseController.SelectedBettery.CalculatedReturnedAmount);
            Assert.AreEqual(TotalAmount, BaseController.SelectedBettery.TotalAmount);
        }

        /// <summary>
        /// Calcs the charges for none argument case1010 test.
        /// </summary>
        [TestMethod]
        public void CalcChargesForNoneArgumentCase1010Test()
        {
            BaseController.SelectedBettery = null;
            BaseController.LoggedOnUser = null;

            // expecteds value
            int swapped = 0;
            decimal SwappedAmount = 0.00M;

            decimal CalculatedAaAmount = 0.00M;
            decimal CalculatedAaaAmount = 0.00M;

            decimal AaNewForgotDrainedAmount = 0.00M;
            decimal AaaNewForgotDrainedAmount = 0.00M;

            int CalculatedNew = 0;
            decimal CalculatedNewAmount = 0.00M;
            int CalculatedReturned = 0;
            decimal CalculatedReturnedAmount = 0.00M;

            decimal TotalAmount = 0.00M;

            BaseController.CalcCharges();

            Assert.IsNotNull(BaseController.SelectedBettery);
            Assert.AreEqual(swapped, BaseController.SelectedBettery.Swapped);
            Assert.AreEqual(SwappedAmount, BaseController.SelectedBettery.SwappedAmount);
            Assert.AreEqual(CalculatedAaAmount, BaseController.SelectedBettery.CalculatedAaAmount);
            Assert.AreEqual(CalculatedAaaAmount, BaseController.SelectedBettery.CalculatedAaaAmount);
            Assert.AreEqual(AaNewForgotDrainedAmount, BaseController.SelectedBettery.AaNewForgotDrainedAmount);
            Assert.AreEqual(AaaNewForgotDrainedAmount, BaseController.SelectedBettery.AaaNewForgotDrainedAmount);
            Assert.AreEqual(CalculatedNew, BaseController.SelectedBettery.CalculatedNew);
            Assert.AreEqual(CalculatedNewAmount, BaseController.SelectedBettery.CalculatedNewAmount);
            Assert.AreEqual(CalculatedReturned, BaseController.SelectedBettery.CalculatedReturned);
            Assert.AreEqual(CalculatedReturnedAmount, BaseController.SelectedBettery.CalculatedReturnedAmount);
            Assert.AreEqual(TotalAmount, BaseController.SelectedBettery.TotalAmount);
        }

        /// <summary>
        /// Calcs the charges for none argument case1011 test.
        /// </summary>
        [TestMethod]
        public void CalcChargesForNoneArgumentCase1011Test()
        {
            BaseController.SelectedBettery = new BetteryVend { PromotionalAmount = 1.00M };
            BaseController.SelectedBettery.AaVend = 2;
            BaseController.SelectedBettery.AaaVend = 2;
            BaseController.SelectedBettery.AaReturn = 1;
            BaseController.SelectedBettery.AaaReturn = 1;
            BaseController.LoggedOnUser = null;

            // expecteds value
            int swapped = 2;
            decimal SwappedAmount = 3.98M;

            decimal CalculatedAaAmount = 3.98M;
            decimal CalculatedAaaAmount = 19.98M;

            decimal AaNewForgotDrainedAmount = 0M;
            decimal AaaNewForgotDrainedAmount = 0M;

            int CalculatedNew = 2;
            decimal CalculatedNewAmount = 19.98M;
            int CalculatedReturned = 0;
            decimal CalculatedReturnedAmount = 0.00M;

            decimal TotalAmount = 22.96M;

            BaseController.CalcCharges();

            Assert.IsNotNull(BaseController.SelectedBettery);
            Assert.AreEqual(swapped, BaseController.SelectedBettery.Swapped);
            Assert.AreEqual(SwappedAmount, BaseController.SelectedBettery.SwappedAmount);
            Assert.AreEqual(CalculatedAaAmount, BaseController.SelectedBettery.CalculatedAaAmount);
            Assert.AreEqual(CalculatedAaaAmount, BaseController.SelectedBettery.CalculatedAaaAmount);
            Assert.AreEqual(AaNewForgotDrainedAmount, BaseController.SelectedBettery.AaNewForgotDrainedAmount);
            Assert.AreEqual(AaaNewForgotDrainedAmount, BaseController.SelectedBettery.AaaNewForgotDrainedAmount);
            Assert.AreEqual(CalculatedNew, BaseController.SelectedBettery.CalculatedNew);
            Assert.AreEqual(CalculatedNewAmount, BaseController.SelectedBettery.CalculatedNewAmount);
            Assert.AreEqual(CalculatedReturned, BaseController.SelectedBettery.CalculatedReturned);
            Assert.AreEqual(CalculatedReturnedAmount, BaseController.SelectedBettery.CalculatedReturnedAmount);
            Assert.AreEqual(TotalAmount, BaseController.SelectedBettery.TotalAmount);
        }

        /// <summary>
        /// Calcs the charges for none argument case1012 test.
        /// </summary>
        [TestMethod]
        public void CalcChargesForNoneArgumentCase1012Test()
        {
            BaseController.SelectedBettery = new BetteryVend { PromotionalAmount = 1.00M };
            BaseController.SelectedBettery.AaVend = 2;
            BaseController.SelectedBettery.AaaVend = 0;
            BaseController.SelectedBettery.AaReturn = 1;
            BaseController.SelectedBettery.AaaReturn = 1;
            BaseController.LoggedOnUser = null;

            // expecteds value
            int swapped = 2;
            decimal SwappedAmount = 3.98M;

            decimal CalculatedAaAmount = 3.98M;
            decimal CalculatedAaaAmount = 0.00M;

            decimal AaNewForgotDrainedAmount = 0M;
            decimal AaaNewForgotDrainedAmount = 0M;

            int CalculatedNew = 0;
            decimal CalculatedNewAmount = 0.00M;
            int CalculatedReturned = 0;
            decimal CalculatedReturnedAmount = 0.00M;

            decimal TotalAmount = 2.98M;

            BaseController.CalcCharges();

            Assert.IsNotNull(BaseController.SelectedBettery);
            Assert.AreEqual(swapped, BaseController.SelectedBettery.Swapped);
            Assert.AreEqual(SwappedAmount, BaseController.SelectedBettery.SwappedAmount);
            Assert.AreEqual(CalculatedAaAmount, BaseController.SelectedBettery.CalculatedAaAmount);
            Assert.AreEqual(CalculatedAaaAmount, BaseController.SelectedBettery.CalculatedAaaAmount);
            Assert.AreEqual(AaNewForgotDrainedAmount, BaseController.SelectedBettery.AaNewForgotDrainedAmount);
            Assert.AreEqual(AaaNewForgotDrainedAmount, BaseController.SelectedBettery.AaaNewForgotDrainedAmount);
            Assert.AreEqual(CalculatedNew, BaseController.SelectedBettery.CalculatedNew);
            Assert.AreEqual(CalculatedNewAmount, BaseController.SelectedBettery.CalculatedNewAmount);
            Assert.AreEqual(CalculatedReturned, BaseController.SelectedBettery.CalculatedReturned);
            Assert.AreEqual(CalculatedReturnedAmount, BaseController.SelectedBettery.CalculatedReturnedAmount);
            Assert.AreEqual(TotalAmount, BaseController.SelectedBettery.TotalAmount);
        }

        /// <summary>
        /// Calcs the charges for none argument case1013 test.
        /// </summary>
        [TestMethod]
        public void CalcChargesForNoneArgumentCase1013Test()
        {
            BaseController.SelectedBettery = new BetteryVend { PromotionalAmount = 1.00M };
            BaseController.SelectedBettery.AaVend = 1;
            BaseController.SelectedBettery.AaaVend = 2;
            BaseController.SelectedBettery.AaReturn = 1;
            BaseController.SelectedBettery.AaaReturn = 1;
            BaseController.LoggedOnUser = null;

            // expecteds value
            int swapped = 2;
            decimal SwappedAmount = 3.98M;

            decimal CalculatedAaAmount = 1.99M;
            decimal CalculatedAaaAmount = 11.98M;

            decimal AaNewForgotDrainedAmount = 0M;
            decimal AaaNewForgotDrainedAmount = 0M;

            int CalculatedNew = 1;
            decimal CalculatedNewAmount = 9.99M;
            int CalculatedReturned = 0;
            decimal CalculatedReturnedAmount = 0.00M;

            decimal TotalAmount = 12.97M;

            BaseController.CalcCharges();

            Assert.IsNotNull(BaseController.SelectedBettery);
            Assert.AreEqual(swapped, BaseController.SelectedBettery.Swapped);
            Assert.AreEqual(SwappedAmount, BaseController.SelectedBettery.SwappedAmount);
            Assert.AreEqual(CalculatedAaAmount, BaseController.SelectedBettery.CalculatedAaAmount);
            Assert.AreEqual(CalculatedAaaAmount, BaseController.SelectedBettery.CalculatedAaaAmount);
            Assert.AreEqual(AaNewForgotDrainedAmount, BaseController.SelectedBettery.AaNewForgotDrainedAmount);
            Assert.AreEqual(AaaNewForgotDrainedAmount, BaseController.SelectedBettery.AaaNewForgotDrainedAmount);
            //Assert.AreEqual(AaReturnedAmount, BaseController.SelectedBettery.AaReturnedAmount);
            //Assert.AreEqual(AaaReturnedAmount, BaseController.SelectedBettery.AaaReturnedAmount);
            Assert.AreEqual(CalculatedNew, BaseController.SelectedBettery.CalculatedNew);
            Assert.AreEqual(CalculatedNewAmount, BaseController.SelectedBettery.CalculatedNewAmount);
            Assert.AreEqual(CalculatedReturned, BaseController.SelectedBettery.CalculatedReturned);
            Assert.AreEqual(CalculatedReturnedAmount, BaseController.SelectedBettery.CalculatedReturnedAmount);
            Assert.AreEqual(TotalAmount, BaseController.SelectedBettery.TotalAmount);
        }

        /// <summary>
        /// Calcs the charges for none argument case1014 test.
        /// </summary>
        [TestMethod]
        public void CalcChargesForNoneArgumentCase1014Test()
        {
            BaseController.SelectedBettery = new BetteryVend { PromotionalAmount = 1.00M };
            BaseController.SelectedBettery.AaVend = 0;
            BaseController.SelectedBettery.AaaVend = 2;
            BaseController.SelectedBettery.AaReturn = 1;
            BaseController.SelectedBettery.AaaReturn = 1;

            BaseController.LoggedOnUser = null;

            // expecteds value
            int swapped = 2;
            decimal SwappedAmount = 3.98M;

            decimal CalculatedAaAmount = 0.00M;
            decimal CalculatedAaaAmount = 3.98M;

            decimal AaNewForgotDrainedAmount = 0M;
            decimal AaaNewForgotDrainedAmount = 0M;

            int CalculatedNew = 0;
            decimal CalculatedNewAmount = 0.00M;
            int CalculatedReturned = 0;
            decimal CalculatedReturnedAmount = 0.00M;

            decimal TotalAmount = 2.98M;

            BaseController.CalcCharges();

            Assert.IsNotNull(BaseController.SelectedBettery);
            Assert.AreEqual(swapped, BaseController.SelectedBettery.Swapped);
            Assert.AreEqual(SwappedAmount, BaseController.SelectedBettery.SwappedAmount);
            Assert.AreEqual(CalculatedAaAmount, BaseController.SelectedBettery.CalculatedAaAmount);
            Assert.AreEqual(CalculatedAaaAmount, BaseController.SelectedBettery.CalculatedAaaAmount);
            Assert.AreEqual(AaNewForgotDrainedAmount, BaseController.SelectedBettery.AaNewForgotDrainedAmount);
            Assert.AreEqual(AaaNewForgotDrainedAmount, BaseController.SelectedBettery.AaaNewForgotDrainedAmount);
            Assert.AreEqual(CalculatedNew, BaseController.SelectedBettery.CalculatedNew);
            Assert.AreEqual(CalculatedNewAmount, BaseController.SelectedBettery.CalculatedNewAmount);
            Assert.AreEqual(CalculatedReturned, BaseController.SelectedBettery.CalculatedReturned);
            Assert.AreEqual(CalculatedReturnedAmount, BaseController.SelectedBettery.CalculatedReturnedAmount);
            Assert.AreEqual(TotalAmount, BaseController.SelectedBettery.TotalAmount);
        }

        /// <summary>
        /// Calcs the charges for none argument case1015 test.
        /// </summary>
        [TestMethod]
        public void CalcChargesForNoneArgumentCase1015Test()
        {
            BaseController.SelectedBettery = new BetteryVend { PromotionalAmount = 1.00M };
            BaseController.SelectedBettery.AaVend = 0;
            BaseController.SelectedBettery.AaaVend = 0;
            BaseController.SelectedBettery.AaReturn = 2;
            BaseController.SelectedBettery.AaaReturn = 2;

            BaseController.LoggedOnUser = null;

            // expecteds value
            int swapped = 0;
            decimal SwappedAmount = 0.00M;

            decimal CalculatedAaAmount = 0.00M;
            decimal CalculatedAaaAmount = 0.00M;

            decimal AaNewForgotDrainedAmount = 0M;
            decimal AaaNewForgotDrainedAmount = 0M;

            int CalculatedNew = 0;
            decimal CalculatedNewAmount = 0.0M;
            int CalculatedReturned = 4;
            decimal CalculatedReturnedAmount = 32.0M;

            decimal TotalAmount = -33.0M;

            BaseController.CalcCharges();

            Assert.IsNotNull(BaseController.SelectedBettery);
            Assert.AreEqual(swapped, BaseController.SelectedBettery.Swapped);
            Assert.AreEqual(SwappedAmount, BaseController.SelectedBettery.SwappedAmount);
            Assert.AreEqual(CalculatedAaAmount, BaseController.SelectedBettery.CalculatedAaAmount);
            Assert.AreEqual(CalculatedAaaAmount, BaseController.SelectedBettery.CalculatedAaaAmount);
            Assert.AreEqual(AaNewForgotDrainedAmount, BaseController.SelectedBettery.AaNewForgotDrainedAmount);
            Assert.AreEqual(AaaNewForgotDrainedAmount, BaseController.SelectedBettery.AaaNewForgotDrainedAmount);
            Assert.AreEqual(CalculatedNew, BaseController.SelectedBettery.CalculatedNew);
            Assert.AreEqual(CalculatedNewAmount, BaseController.SelectedBettery.CalculatedNewAmount);
            Assert.AreEqual(CalculatedReturned, BaseController.SelectedBettery.CalculatedReturned);
            Assert.AreEqual(CalculatedReturnedAmount, BaseController.SelectedBettery.CalculatedReturnedAmount);
            Assert.AreEqual(TotalAmount, BaseController.SelectedBettery.TotalAmount);
        }

        #endregion CalcCharges For None Argument

        #endregion ChargeHelper

        #region GetChargesSummary

        /// <summary>
        /// Gets the charges summary case1001 test.
        /// </summary>
        [TestMethod]
        public void GetChargesSummaryCase1001Test()
        {
            BaseController.SelectedBettery = null;

            string expected = "";
            string actual = BaseController.GetChargesSummary();
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Gets the charges summary case1002 test.
        /// </summary>
        [TestMethod]
        public void GetChargesSummaryCase1002Test()
        {
            BaseController.SelectedBettery = new BetteryVend
            {
                AaVend = 0,
                AaaVend = 0,
                AaReturn = 0,
                AaaReturn = 0,
                PromotionalAmount = 0M,
                AaForgotDrainedVend = 0
            };

            BaseController.LoggedOnUser = null;
            BaseController.CalcCharges();

            string expected = "Your total = $0.00\n";
            string actual = BaseController.GetChargesSummary();
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Gets the charges summary case1003 test.
        /// </summary>
        [TestMethod]
        public void GetChargesSummaryCase1003Test()
        {
            BaseController.SelectedBettery = new BetteryVend
            {
                AaVend = 0,
                AaaVend = 0,
                AaReturn = 0,
                AaaReturn = 0,
                PromotionalAmount = 1M,
                AaForgotDrainedVend = 0
            };

            BaseController.LoggedOnUser = null;
            BaseController.CalcCharges();

            string expected = "Your promotion credit = $1.00\nYour credit = $1.00\n";
            string actual = BaseController.GetChargesSummary();
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Gets the charges summary case1004 test.
        /// </summary>
        [TestMethod]
        public void GetChargesSummaryCase1004Test()
        {
            BaseController.SelectedBettery = new BetteryVend
            {
                AaVend = 0,
                AaaVend = 0,
                AaReturn = 0,
                AaaReturn = 0,
                PromotionalAmount = 0M,
                AaForgotDrainedVend = 0
            };

            BaseController.LoggedOnUser = new BetteryUser { OutstandingCredit = 1.99M };
            BaseController.CalcCharges();

            string expected = "Your account credit = $1.99\nYour credit = $1.99\n";
            string actual = BaseController.GetChargesSummary();
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Gets the charges summary case1005 test.
        /// </summary>
        [TestMethod]
        public void GetChargesSummaryCase1005Test()
        {
            BaseController.SelectedBettery = new BetteryVend
            {
                AaVend = 0,
                AaaVend = 0,
                AaReturn = 0,
                AaaReturn = 0,
                PromotionalAmount = 1M,
                AaForgotDrainedVend = 0
            };

            BaseController.LoggedOnUser = new BetteryUser { OutstandingCredit = 1.99M };
            BaseController.CalcCharges();

            string expected = "Your account credit = $1.99\nYour promotion credit = $1.00\nYour credit = $2.99\n";
            string actual = BaseController.GetChargesSummary();
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Gets the charges summary case1006 test.
        /// </summary>
        [TestMethod]
        public void GetChargesSummaryCase1006Test()
        {
            BaseController.SelectedBettery = new BetteryVend
            {
                AaVend = 0,
                AaaVend = 0,
                AaReturn = 0,
                AaaReturn = 0,
                PromotionalAmount = 0M,
                AaForgotDrainedVend = 1
            };
            BaseController.LoggedOnUser = null;

            BaseController.CalcCharges();

            string expected = "1 pack(s) of four batteries swapped for forgot pack(s) @ 1.99 = $1.99\nYour total = $1.99\n";
            string actual = BaseController.GetChargesSummary();
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Gets the charges summary case1007 test.
        /// </summary>
        [TestMethod]
        public void GetChargesSummaryCase1007Test()
        {
            BaseController.SelectedBettery = new BetteryVend
            {
                AaVend = 0,
                AaaVend = 0,
                AaReturn = 0,
                AaaReturn = 0,
                PromotionalAmount = 1M,
                AaForgotDrainedVend = 1
            };
            BaseController.LoggedOnUser = null;

            BaseController.CalcCharges();

            string expected = "1 pack(s) of four batteries swapped for forgot pack(s) @ 1.99 = $1.99\nYour promotion credit = $1.00\nYour total = $0.99\n";
            string actual = BaseController.GetChargesSummary();
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Gets the charges summary case1008 test.
        /// </summary>
        [TestMethod]
        public void GetChargesSummaryCase1008Test()
        {
            BaseController.SelectedBettery = new BetteryVend
            {
                AaVend = 0,
                AaaVend = 0,
                AaReturn = 0,
                AaaReturn = 0,
                PromotionalAmount = 0M,
                AaForgotDrainedVend = 1
            };

            BaseController.LoggedOnUser = new BetteryUser { OutstandingCredit = 1.99M };

            BaseController.CalcCharges();

            string expected = "1 pack(s) of four batteries swapped for forgot pack(s) @ 1.99 = $1.99\nYour account credit = $1.99\nYour total = $0.00\n";
            string actual = BaseController.GetChargesSummary();
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Gets the charges summary case1009 test.
        /// </summary>
        [TestMethod]
        public void GetChargesSummaryCase1009Test()
        {
            BaseController.SelectedBettery = new BetteryVend
            {
                AaVend = 0,
                AaaVend = 0,
                AaReturn = 0,
                AaaReturn = 0,
                PromotionalAmount = 1M,
                AaForgotDrainedVend = 1
            };

            BaseController.LoggedOnUser = new BetteryUser { OutstandingCredit = 1.99M };

            BaseController.CalcCharges();

            string expected = "1 pack(s) of four batteries swapped for forgot pack(s) @ 1.99 = $1.99\nYour account credit = $1.99\nYour promotion credit = $1.00\nYour credit = $1.00\n";
            string actual = BaseController.GetChargesSummary();
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Gets the charges summary case1010 test.
        /// </summary>
        [TestMethod]
        public void GetChargesSummaryCase1010Test()
        {
            BaseController.SelectedBettery = new BetteryVend
            {
                AaVend = 0,
                AaaVend = 0,
                AaReturn = 1,
                AaaReturn = 1,
                PromotionalAmount = 0M,
                AaForgotDrainedVend = 0
            };

            BaseController.LoggedOnUser = null;
            BaseController.CalcCharges();

            string expected = "2 pack(s) of four batteries returned @ -8.00 = -$16.00\nYour credit = $16.00\n";
            string actual = BaseController.GetChargesSummary();
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Gets the charges summary case1011 test.
        /// </summary>
        [TestMethod]
        public void GetChargesSummaryCase1011Test()
        {
            BaseController.SelectedBettery = new BetteryVend
            {
                AaVend = 0,
                AaaVend = 0,
                AaReturn = 1,
                AaaReturn = 1,
                PromotionalAmount = 1M,
                AaForgotDrainedVend = 0
            };

            BaseController.LoggedOnUser = null;
            BaseController.CalcCharges();

            string expected = "2 pack(s) of four batteries returned @ -8.00 = -$16.00\nYour promotion credit = $1.00\nYour credit = $17.00\n";
            string actual = BaseController.GetChargesSummary();
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Gets the charges summary case1012 test.
        /// </summary>
        [TestMethod]
        public void GetChargesSummaryCase1012Test()
        {
            BaseController.SelectedBettery = new BetteryVend
            {
                AaVend = 0,
                AaaVend = 0,
                AaReturn = 1,
                AaaReturn = 1,
                PromotionalAmount = 0M,
                AaForgotDrainedVend = 0
            };

            BaseController.LoggedOnUser = new BetteryUser { OutstandingCredit = 1.99M };
            BaseController.CalcCharges();

            string expected = "2 pack(s) of four batteries returned @ -8.00 = -$16.00\nYour account credit = $1.99\nYour credit = $17.99\n";
            string actual = BaseController.GetChargesSummary();
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Gets the charges summary case1013 test.
        /// </summary>
        [TestMethod]
        public void GetChargesSummaryCase1013Test()
        {
            BaseController.SelectedBettery = new BetteryVend
            {
                AaVend = 0,
                AaaVend = 0,
                AaReturn = 1,
                AaaReturn = 1,
                PromotionalAmount = 1M,
                AaForgotDrainedVend = 0
            };

            BaseController.LoggedOnUser = new BetteryUser { OutstandingCredit = 1.99M };
            BaseController.CalcCharges();

            string expected = "2 pack(s) of four batteries returned @ -8.00 = -$16.00\nYour account credit = $1.99\nYour promotion credit = $1.00\nYour credit = $18.99\n";
            string actual = BaseController.GetChargesSummary();
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Gets the charges summary case1014 test.
        /// </summary>
        [TestMethod]
        public void GetChargesSummaryCase1014Test()
        {
            BaseController.SelectedBettery = new BetteryVend
            {
                AaVend = 0,
                AaaVend = 0,
                AaReturn = 1,
                AaaReturn = 1,
                PromotionalAmount = 0M,
                AaForgotDrainedVend = 1
            };

            BaseController.LoggedOnUser = null;
            BaseController.CalcCharges();

            string expected = "1 pack(s) of four batteries swapped for forgot pack(s) @ 1.99 = $1.99\n2 pack(s) of four batteries returned @ -8.00 = -$16.00\nYour credit = $14.01\n";
            string actual = BaseController.GetChargesSummary();
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Gets the charges summary case1015 test.
        /// </summary>
        [TestMethod]
        public void GetChargesSummaryCase1015Test()
        {
            BaseController.SelectedBettery = new BetteryVend
            {
                AaVend = 0,
                AaaVend = 0,
                AaReturn = 1,
                AaaReturn = 1,
                PromotionalAmount = 1M,
                AaForgotDrainedVend = 1
            };

            BaseController.LoggedOnUser = null;
            BaseController.CalcCharges();

            string expected = "1 pack(s) of four batteries swapped for forgot pack(s) @ 1.99 = $1.99\n2 pack(s) of four batteries returned @ -8.00 = -$16.00\nYour promotion credit = $1.00\nYour credit = $15.01\n";
            string actual = BaseController.GetChargesSummary();
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Gets the charges summary case1016 test.
        /// </summary>
        [TestMethod]
        public void GetChargesSummaryCase1016Test()
        {
            BaseController.SelectedBettery = new BetteryVend
            {
                AaVend = 0,
                AaaVend = 0,
                AaReturn = 1,
                AaaReturn = 1,
                PromotionalAmount = 0M,
                AaForgotDrainedVend = 1
            };

            BaseController.LoggedOnUser = new BetteryUser { OutstandingCredit = 1.99M };
            BaseController.CalcCharges();

            string expected = "1 pack(s) of four batteries swapped for forgot pack(s) @ 1.99 = $1.99\n2 pack(s) of four batteries returned @ -8.00 = -$16.00\nYour account credit = $1.99\nYour credit = $16.00\n";
            string actual = BaseController.GetChargesSummary();
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Gets the charges summary case1017 test.
        /// </summary>
        [TestMethod]
        public void GetChargesSummaryCase1017Test()
        {
            BaseController.SelectedBettery = new BetteryVend
            {
                AaVend = 0,
                AaaVend = 0,
                AaReturn = 1,
                AaaReturn = 1,
                PromotionalAmount = 1M,
                AaForgotDrainedVend = 1
            };

            BaseController.LoggedOnUser = new BetteryUser { OutstandingCredit = 1.99M };
            BaseController.CalcCharges();

            string expected = "1 pack(s) of four batteries swapped for forgot pack(s) @ 1.99 = $1.99\n2 pack(s) of four batteries returned @ -8.00 = -$16.00\nYour account credit = $1.99\nYour promotion credit = $1.00\nYour credit = $17.00\n";
            string actual = BaseController.GetChargesSummary();
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Gets the charges summary case1018 test.
        /// </summary>
        [TestMethod]
        public void GetChargesSummaryCase1018Test()
        {
            BaseController.SelectedBettery = new BetteryVend
            {
                AaVend = 1,
                AaaVend = 1,
                AaReturn = 0,
                AaaReturn = 0,
                PromotionalAmount = 0M,
                AaForgotDrainedVend = 0
            };

            BaseController.LoggedOnUser = null;
            BaseController.CalcCharges();

            string expected = "2 additional pack(s) of four batteries @ 9.99 = $19.98\nYour total = $19.98\n";
            string actual = BaseController.GetChargesSummary();
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Gets the charges summary case1019 test.
        /// </summary>
        [TestMethod]
        public void GetChargesSummaryCase1019Test()
        {
            BaseController.SelectedBettery = new BetteryVend
            {
                AaVend = 1,
                AaaVend = 1,
                AaReturn = 0,
                AaaReturn = 0,
                PromotionalAmount = 1M,
                AaForgotDrainedVend = 0
            };

            BaseController.LoggedOnUser = null;
            BaseController.CalcCharges();

            string expected = "2 additional pack(s) of four batteries @ 9.99 = $19.98\nYour promotion credit = $1.00\nYour total = $18.98\n";
            string actual = BaseController.GetChargesSummary();
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Gets the charges summary case1020 test.
        /// </summary>
        [TestMethod]
        public void GetChargesSummaryCase1020Test()
        {
            BaseController.SelectedBettery = new BetteryVend
            {
                AaVend = 1,
                AaaVend = 1,
                AaReturn = 0,
                AaaReturn = 0,
                PromotionalAmount = 0M,
                AaForgotDrainedVend = 0
            };

            BaseController.LoggedOnUser = new BetteryUser { OutstandingCredit = 1.99M };
            BaseController.CalcCharges();

            string expected = "2 additional pack(s) of four batteries @ 9.99 = $19.98\nYour account credit = $1.99\nYour total = $17.99\n";
            string actual = BaseController.GetChargesSummary();
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Gets the charges summary case1021 test.
        /// </summary>
        [TestMethod]
        public void GetChargesSummaryCase1021Test()
        {
            BaseController.SelectedBettery = new BetteryVend
            {
                AaVend = 1,
                AaaVend = 1,
                AaReturn = 0,
                AaaReturn = 0,
                PromotionalAmount = 1M,
                AaForgotDrainedVend = 0
            };

            BaseController.LoggedOnUser = new BetteryUser { OutstandingCredit = 1.99M };
            BaseController.CalcCharges();

            string expected = "2 additional pack(s) of four batteries @ 9.99 = $19.98\nYour account credit = $1.99\nYour promotion credit = $1.00\nYour total = $16.99\n";
            string actual = BaseController.GetChargesSummary();
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Gets the charges summary case1022 test.
        /// </summary>
        [TestMethod]
        public void GetChargesSummaryCase1022Test()
        {
            BaseController.SelectedBettery = new BetteryVend
            {
                AaVend = 1,
                AaaVend = 1,
                AaReturn = 0,
                AaaReturn = 0,
                PromotionalAmount = 0M,
                AaForgotDrainedVend = 1
            };

            BaseController.LoggedOnUser = null;
            BaseController.CalcCharges();

            string expected = "1 pack(s) of four batteries swapped for forgot pack(s) @ 1.99 = $1.99\n2 additional pack(s) of four batteries @ 9.99 = $19.98\nYour total = $21.97\n";
            string actual = BaseController.GetChargesSummary();
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Gets the charges summary case1023 test.
        /// </summary>
        [TestMethod]
        public void GetChargesSummaryCase1023Test()
        {
            BaseController.SelectedBettery = new BetteryVend
            {
                AaVend = 1,
                AaaVend = 1,
                AaReturn = 0,
                AaaReturn = 0,
                PromotionalAmount = 1M,
                AaForgotDrainedVend = 1
            };

            BaseController.LoggedOnUser = null;
            BaseController.CalcCharges();

            string expected = "1 pack(s) of four batteries swapped for forgot pack(s) @ 1.99 = $1.99\n2 additional pack(s) of four batteries @ 9.99 = $19.98\nYour promotion credit = $1.00\nYour total = $20.97\n";
            string actual = BaseController.GetChargesSummary();
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Gets the charges summary case1024 test.
        /// </summary>
        [TestMethod]
        public void GetChargesSummaryCase1024Test()
        {
            BaseController.SelectedBettery = new BetteryVend
            {
                AaVend = 1,
                AaaVend = 1,
                AaReturn = 0,
                AaaReturn = 0,
                PromotionalAmount = 0M,
                AaForgotDrainedVend = 1
            };

            BaseController.LoggedOnUser = new BetteryUser { OutstandingCredit = 1.99M };
            BaseController.CalcCharges();

            string expected = "1 pack(s) of four batteries swapped for forgot pack(s) @ 1.99 = $1.99\n2 additional pack(s) of four batteries @ 9.99 = $19.98\nYour account credit = $1.99\nYour total = $19.98\n";
            string actual = BaseController.GetChargesSummary();
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Gets the charges summary case1025 test.
        /// </summary>
        [TestMethod]
        public void GetChargesSummaryCase1025Test()
        {
            BaseController.SelectedBettery = new BetteryVend
            {
                AaVend = 1,
                AaaVend = 1,
                AaReturn = 0,
                AaaReturn = 0,
                PromotionalAmount = 1M,
                AaForgotDrainedVend = 1
            };

            BaseController.LoggedOnUser = new BetteryUser { OutstandingCredit = 1.99M };
            BaseController.CalcCharges();

            string expected = "1 pack(s) of four batteries swapped for forgot pack(s) @ 1.99 = $1.99\n2 additional pack(s) of four batteries @ 9.99 = $19.98\nYour account credit = $1.99\nYour promotion credit = $1.00\nYour total = $18.98\n";
            string actual = BaseController.GetChargesSummary();
            Assert.AreEqual(expected, actual);
        }

        #endregion GetChargesSummary

        #region UpdateRecentViewOfCurrentFlow

        /// <summary>
        /// Updates the recent view of current flow case1001 test.
        /// </summary>
        [TestMethod]
        public void UpdateRecentViewOfCurrentFlowCase1001Test()
        {
            BaseController.RecentViewOfCurrentFlow = null;
            BaseController.UpdateRecentViewOfCurrentFlow(Constants.ViewName.Welcome);
            string expected = Constants.ViewName.Welcome;
            Assert.AreEqual(expected, BaseController.RecentViewOfCurrentFlow);
        }

        /// <summary>
        /// Updates the recent view of current flow case1002 test.
        /// </summary>
        [TestMethod]
        public void UpdateRecentViewOfCurrentFlowCase1002Test()
        {
            BaseController.RecentViewOfCurrentFlow = null;
            BaseController.UpdateRecentViewOfCurrentFlow(Constants.ViewName.ThankYou);
            string expected = null;
            Assert.AreEqual(expected, BaseController.RecentViewOfCurrentFlow);
        }

        #endregion UpdateRecentViewOfCurrentFlow
    }
}
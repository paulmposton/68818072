using Bettery.Kiosk;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BKiosk.UnitTest
{
    /// <summary>
    /// Summary description for MembershipTest
    /// </summary>
    [TestClass]
    public class MembershipTest
    {
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

        /// <summary>
        /// Validates the inputs case1001 test.
        /// </summary>
        [TestMethod]
        public void ValidateInputsCase1001Test()
        {
            string firstname = "";
            string lastname = "";
            string email = "";
            string password = "";
            string confirmpassword = "";
            string zipcode = "";

            bool expected = false;

            bool actual = BaseController.ValidateMembershipInputs(firstname, lastname, email, password, confirmpassword, zipcode);

            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Validates the inputs case1002 test.
        /// </summary>
        [TestMethod]
        public void ValidateInputsCase1002Test()
        {
            string firstname = "Anh";
            string lastname = "";
            string email = "";
            string password = "";
            string confirmpassword = "";
            string zipcode = "";

            bool expected = false;

            bool actual = BaseController.ValidateMembershipInputs(firstname, lastname, email, password, confirmpassword, zipcode);

            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Validates the inputs case1003 test.
        /// </summary>
        [TestMethod]
        public void ValidateInputsCase1003Test()
        {
            string firstname = "Anh";
            string lastname = "Nguyen Duc";
            string email = "";
            string password = "";
            string confirmpassword = "";
            string zipcode = "";

            bool expected = false;

            bool actual = BaseController.ValidateMembershipInputs(firstname, lastname, email, password, confirmpassword, zipcode);

            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Validates the inputs case1004 test.
        /// </summary>
        [TestMethod]
        public void ValidateInputsCase1004Test()
        {
            string firstname = "Anh";
            string lastname = "Nguyen Duc";
            string email = "ducanhtk5@hotmail.com";
            string password = "";
            string confirmpassword = "";
            string zipcode = "";

            bool expected = false;

            bool actual = BaseController.ValidateMembershipInputs(firstname, lastname, email, password, confirmpassword, zipcode);

            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Validates the inputs case1005 test.
        /// </summary>
        [TestMethod]
        public void ValidateInputsCase1005Test()
        {
            string firstname = "Anh";
            string lastname = "Nguyen Duc";
            string email = "ducanhtk5@hotmail.com";
            string password = "Bettery@123";
            string confirmpassword = "";
            string zipcode = "";

            bool expected = false;

            bool actual = BaseController.ValidateMembershipInputs(firstname, lastname, email, password, confirmpassword, zipcode);

            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Validates the inputs case1006 test.
        /// </summary>
        [TestMethod]
        public void ValidateInputsCase1006Test()
        {
            string firstname = "Anh";
            string lastname = "Nguyen Duc";
            string email = "ducanhtk5@hotmail.com";
            string password = "Bettery@123";
            string confirmpassword = "Bettery@123";
            string zipcode = "123";

            bool expected = false;

            bool actual = BaseController.ValidateMembershipInputs(firstname, lastname, email, password, confirmpassword, zipcode);

            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Validates the inputs case1007 test.
        /// </summary>
        [TestMethod]
        public void ValidateInputsCase1007Test()
        {
            string firstname = "Anh";
            string lastname = "Nguyen Duc";
            string email = "ducanhtk5@hotmail.com";
            string password = "Bettery@123";
            string confirmpassword = "Bettery@123";
            string zipcode = "12345";

            bool expected = true;

            bool actual = BaseController.ValidateMembershipInputs(firstname, lastname, email, password, confirmpassword, zipcode);

            Assert.AreEqual(expected, actual);
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Security;
using Bettery.WebRole.Models;
using System.Data.SqlClient;
using System.Data;
using System.Configuration;
using Bettery.WebRole.Services;
using AuthorizeNet;
using Bettery.Web.AuthorizeNetService;
using MvcWebRole1.Common;
using System.Net.Mail;
using Microsoft.Web.Helpers;

namespace Bettery.WebRole.Controllers
{
    public class AccountController : Controller
    {

        private static MerchantAuthenticationType merchantAuthenticationType = null;
        private static ServiceSoapClient serviceSoapClient = null;
        private AccountService accountService;
        private static MerchantAuthenticationType MerchantAuthentication
        {
            get
            {
                if (merchantAuthenticationType == null)
                {
                    merchantAuthenticationType = new MerchantAuthenticationType();
                    merchantAuthenticationType.name = ConfigurationManager.AppSettings["ApiLogin"];
                    merchantAuthenticationType.transactionKey = ConfigurationManager.AppSettings["TransactionKey"];
                }

                return merchantAuthenticationType;
            }
        }


        public AccountController()
            : this(new AccountService())
        {
        }

        public AccountController(AccountService accountService)
        {
            this.accountService = accountService;
        }


        //
        // GET: /Account/Credits/5

        public ActionResult Credits(int id)
        {
            IEnumerable<Credit> credits = accountService.GetCredits(id);
            return View(credits);
        }

        //
        // GET: /Account/Transactions/5
        [Authorize]
        public ActionResult Transactions()
        {
            if (Session["MemberID"] == null)
                return RedirectToAction("Index", "Home");
            else
            {
                IEnumerable<Bettery.WebRole.Models.Transaction> transactions = accountService.GetTransactions((int)(Session["MemberID"]));
                TimeZoneInfo pstZone = TimeZoneInfo.FindSystemTimeZoneById("Pacific Standard Time");
                foreach (var transaction in transactions)
                    transaction.TransactionDateTime = TimeZoneInfo.ConvertTimeFromUtc(transaction.TransactionDateTime, pstZone);
                return View(transactions);
            }
        }
        //
        // GET: /Account/TransactionDetail/5
        [Authorize]
        public ActionResult TransactionDetail(int id)
        {
            //int remainingReturns = 0;

            Bettery.WebRole.Models.TransactionDetail transactionDetail = accountService.GetTransactionDetail(id);

            int depositUnits = (transactionDetail.AAVend + transactionDetail.AAAVend) - transactionDetail.AAReturn;

            decimal aaVendAmount = transactionDetail.AAVend * transactionDetail.SwapPrice;
            decimal aaaVendAmount = transactionDetail.AAAVend * transactionDetail.SwapPrice;
            decimal depositAmount = depositUnits * (transactionDetail.PurchasePrice - transactionDetail.SwapPrice);
            decimal subTotal = aaVendAmount + aaaVendAmount;

            TimeZoneInfo pstZone = TimeZoneInfo.FindSystemTimeZoneById("Pacific Standard Time");

            transactionDetail.Deposits = depositAmount;

            transactionDetail.TransactionDateTime = TimeZoneInfo.ConvertTimeFromUtc(transactionDetail.TransactionDateTime, pstZone);
            return View(transactionDetail);


        }

        //
        // GET: /Account/Terms
        public ActionResult Terms()
        {
            return View();

        }
        //[Authorize]
        //public ActionResult CreditCard()
        //{
        //    return View();

        //}

        ////
        //// POST: /Account/CreditCard
        //[HttpPost]
        //[Authorize]
        //public ActionResult CreditCard(CreditCard creditCard)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        if (Request.HttpMethod == "POST")
        //        {

        //            creditCard.CustomerProfileID = CreateCustomerProfile(creditCard.EmailAddress);
        //            creditCard.PaymentProfileID = CreateCustomerPaymentProfile(creditCard.CustomerProfileID, creditCard.CCNumber, creditCard.CCExpireDate, creditCard.FirstName, creditCard.LastName, creditCard.Zipcode);
        //            creditCard.CCLastFourDigits = creditCard.CCNumber.Substring(creditCard.CCNumber.Length - 4);
        //            accountService.AddCreditCard((int)Session["MemberID"], creditCard);
        //            return RedirectToAction("Details", "Account");


        //        }
        //    }

        //    return View(creditCard);

        //}

        //
        // GET: /Account/LogIn
        //[RequireHttps]
        public ActionResult LogIn()
        {
            return View();
        }

        //
        // POST: /Account/LogOn

        [HttpPost]
        public ActionResult LogIn(LogInModel model, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                if (ValidateUser(model.UserName, model.Password))
                {



                    FormsAuthentication.SetAuthCookie(model.UserName, model.RememberMe);

                    if (Url.IsLocalUrl(returnUrl) && returnUrl.Length > 1 && returnUrl.StartsWith("/")
                        && !returnUrl.StartsWith("//") && !returnUrl.StartsWith("/\\"))
                    {

                        return Redirect(returnUrl);
                    }
                    else
                    {
                        return RedirectToAction("Details", "Account");
                    }
                }
                else
                {
                    ModelState.AddModelError("", "The email or password provided is incorrect.");
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // GET: /Account/LogOff

        public ActionResult LogOff()
        {
            Session.Abandon();
            FormsAuthentication.SignOut();
            Response.Redirect("http://betteryinc.com");
            return RedirectToAction("Index", "Home");
        }

        //
        // GET: /Account/Register

        public ActionResult Register()
        {
            return View();
        }

        //
        // GET: /Account/ChangePassword

        [Authorize]
        public ActionResult ChangePassword()
        {
            return View();
        }

        //
        // POST: /Account/ChangePassword

        [Authorize]
        [HttpPost]
        public ActionResult ChangePassword(ChangePasswordModel changePasswordModel)
        {
            if (ModelState.IsValid)
            {

                // ChangePassword will throw an exception rather
                // than return fals e in certain failure scenarios.
                bool changePasswordSucceeded = false;
                try
                {
                    changePasswordModel.MemberID = (int)Session["MemberID"];
                    accountService.UpdatePassword(changePasswordModel);



                    // Fetch template body from disk
                    var template = TemplateUtils.GetEmailTemplate("http://www2.betteryinc.com/content/htmlchangepassword.htm");


                    // Add any tokens you want to find/replace within your template file
                    var tokens = new Dictionary<string, string>() { { "##FULLNAME##", Session["Email"].ToString() } };


                    // Specify addresses (CC and BCC are optional)
                    var to = new MailAddress(Session["Email"].ToString());
                    var fr = new MailAddress("bruce.wilson@live.com");
                    // Send the mail
                    TemplateUtils.Send(to, fr, "BETTERY Account Password Update", tokens, template, true);


                    changePasswordSucceeded = true;
                }
                catch (Exception)
                {
                    changePasswordSucceeded = false;
                }

                if (changePasswordSucceeded)
                {
                    return RedirectToAction("ChangePasswordSuccess");
                }
                else
                {
                    ModelState.AddModelError("", "The current password is incorrect or the new password is invalid.");
                }
            }

            // If we got this far, something failed, redisplay form
            return View(changePasswordModel);
        }

        //
        // GET: /Account/ChangePasswordSuccess

        public ActionResult ChangePasswordSuccess()
        {
            return View();
        }

        //
        // GET: /Members/Details/5
        [Authorize]
        public ActionResult Details()
        {
            Member member;
            member = accountService.GetMember((int)Session["MemberID"]);
            return View(member);
        }

        //
        // GET: /Members/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /Members/Create

        [HttpPost]
        public ActionResult Create(Member member)
        {
            if (ModelState.IsValid)
            {
                accountService.CreateMember(member);
                if (ValidateUser(member.EmailAddress, member.Password))
                {
                    FormsAuthentication.SetAuthCookie(member.EmailAddress, false);
                    return RedirectToAction("Details", "Account");
                }
                else
                    return RedirectToAction("CreateSuccess", "Account");
            }

            return View(member);
        }

        //
        // GET: /Members/CreateSuccess

        public ActionResult CreateSuccess()
        {
            return View();
        }

        //
        // GET: /Members/Edit/5
        [Authorize]
        public ActionResult Edit(string id)
        {
            EditMember editMember;

            editMember = accountService.GeEdittMember((int)Session["MemberID"]);
            return View(editMember);

        }

        //
        // POST: /Members/Edit/5

        [HttpPost]
        public ActionResult Edit(EditMember editMember)
        {


            if (ModelState.IsValid)
            {
                accountService.UpdateMember(editMember);
                return RedirectToAction("Details", "Account");

            }
            return View(editMember);
        }


        //
        // GET: /Members/Registration/5

        public ActionResult Registration(string id)
        {
            Member member = accountService.GetMember(id);
            return View(member);

        }

        //
        // POST: /Members/Registration/5

        [HttpPost]
        public ActionResult Registration(Member member)
        {
            if (ModelState.IsValid)
            {
                accountService.RegisterMember(member);
                return RedirectToAction("RegistrationSuccess", "Account");

            }
            return View(member);
        }

        //
        // GET: /Members/Registration/5

        public ActionResult RegistrationSuccess()
        {
            return View();
        }

        //
        // GET: /Members/RequestRecoverPassword

        public ActionResult RequestRecoverPassword()
        {
            
            return View();

        }

        [HttpPost]
        public ActionResult RequestRecoverPassword(RequestRecoverPasswordModel requestRecoverPasswordModel)
        {

            if (!ReCaptcha.Validate(privateKey: "6LccQ-ISAAAAADW9ZY30RQnCWpCHCYUrNulK9JWz"))
            {
                ModelState.AddModelError("_FORM", "You did not type the verification word correctly. Please try again.");
            }

            if (ModelState.IsValid)
            {
                string passwordVerifiationToken = accountService.SelectPasswordVerifiationToken(requestRecoverPasswordModel.EmailAddress);
                string url = "http://www2.betteryinc.com/Account/RecoverPassword/" + passwordVerifiationToken;

                // Add any tokens you want to find/replace within your template file
                var tokens = new Dictionary<string, string>() { { "##EMAILADDRESS##", requestRecoverPasswordModel.EmailAddress }, 
                                                                            { "##URL##", url } };

                // Fetch template body from disk
                var template = TemplateHelper.GetEmailTemplate("http://www2.betteryinc.com/content/htmlpasswordrecovery.htm");
                //var template = TemplateHelper.GetEmailTemplate("http://127.0.0.1:444/content/htmlpasswordrecovery.htm");

                // Specify addresses (CC and BCC are optional)
                var to = new MailAddress(requestRecoverPasswordModel.EmailAddress);
                var fr = new MailAddress("support@BETTERYinc.com");
                var bcc = new MailAddress("support@BETTERYinc.com");
                // Send the mail
                TemplateHelper.Send(to, fr, bcc, "Thank you for using BETTERY", tokens, template, true);

                return RedirectToAction("LogIn", "Account");
            }

            return View();
        }

        //
        // GET: /Members/RecoverPassword
        public ActionResult RecoverPassword()
        {

            return View();

        }

        //
        // GET: /Members/Delete/5

        public ActionResult Delete()
        {
            //Member member = db.Members.Find(id);
            //return View(member);
            return View();
        }

        //
        // POST: /Members/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed()
        {

            accountService.DeleteMember((int)Session["MemberID"]);

            return RedirectToAction("LogIn", "Account");
        }













        public static ServiceSoapClient ServiceSoapClient
        {
            get
            {
                if (serviceSoapClient == null)
                {
                    serviceSoapClient = new ServiceSoapClient();
                }

                return serviceSoapClient;
            }
        }
        /// <summary>
        /// Creates the customer profile.
        /// </summary>
        /// <param name="emailAddress">The email address.</param>
        /// <returns></returns>
        public static long CreateCustomerProfile(string emailAddress)
        {
            long customerProfileId = 0;
            try
            {
                CustomerProfileType m_new_cust = new CustomerProfileType();
                m_new_cust.email = emailAddress;
                m_new_cust.description = "Website customer " + DateTime.Now.ToShortTimeString();
                CreateCustomerProfileResponseType response = ServiceSoapClient.CreateCustomerProfile(MerchantAuthentication, m_new_cust, ValidationModeEnum.none);

                customerProfileId = response.customerProfileId;

                if (response.resultCode == MessageTypeEnum.Error)
                {
                    throw new Exception(string.Format("Could not create customer profile for email: {0}.", emailAddress));
                }
            }
            catch (Exception ex)
            {
                throw;
            }

            return customerProfileId;
        }

        /// <summary>
        /// Creates the customer payment profile.
        /// </summary>
        /// <param name="profile_id">The profile_id.</param>
        /// <param name="ccNumber">The cc number.</param>
        /// <param name="expire_date">The expire_date.</param>
        /// <returns></returns>
        public static long CreateCustomerPaymentProfile(long profile_id, string ccNumber, string expire_date, string firstName, string lastName, string zipCode)
        {
            long customerPaymentProfileId = 0;
            try
            {
                CustomerPaymentProfileType new_payment_profile = new CustomerPaymentProfileType();
                PaymentType new_payment = new PaymentType();
                CreditCardType new_card = new CreditCardType();
                new_card.cardNumber = ccNumber;
                new_card.expirationDate = expire_date;
                new_payment.Item = new_card;
                new_payment_profile.payment = new_payment;
                new_payment_profile.billTo = new CustomerAddressType
                {
                    firstName = firstName,
                    lastName = lastName,
                    zip = zipCode
                };

                CreateCustomerPaymentProfileResponseType response = ServiceSoapClient.CreateCustomerPaymentProfile(MerchantAuthentication, profile_id, new_payment_profile, ValidationModeEnum.testMode);
                customerPaymentProfileId = response.customerPaymentProfileId;

                if (response.resultCode == MessageTypeEnum.Error)
                {
                    throw new Exception(string.Format("Could not create customer payment profile for profile id: {0}.", profile_id));
                }
            }

            catch (Exception ex)
            {
                throw;
            }

            return customerPaymentProfileId;
        }
        private bool ValidateUser(string UserName, string Password)
        {

            bool isMember = false;
            HashUtils hashUtils = new HashUtils();

            SqlConnection sqlConn = new SqlConnection(ConfigurationManager.ConnectionStrings["MemberDBContext"].ToString());

            SqlCommand sqlCMD = new SqlCommand("GetPasswordDigest", sqlConn);
            sqlCMD.CommandType = CommandType.StoredProcedure;
            sqlCMD.Parameters.Add(new SqlParameter("@UserName", UserName));

            sqlConn.Open();
            string passwordDigest = sqlCMD.ExecuteScalar().ToString();
            sqlConn.Close();

            
            if (!String.IsNullOrEmpty(passwordDigest))
            {

                string salt = passwordDigest.Substring(passwordDigest.Length - 8);
                string hash = passwordDigest.Substring(0, passwordDigest.Length - 8);
                if (isMember = hashUtils.VerifyHashString(Password, hash, salt))
                {
                    int? MemberID = null;
                    int GroupID;
                    sqlCMD = new SqlCommand("SwapStationAuthenticate", sqlConn);
                    sqlCMD.CommandType = CommandType.StoredProcedure;
                    sqlCMD.Parameters.Add(new SqlParameter("@UserName", UserName));
                    sqlCMD.Parameters.Add(new SqlParameter("@Password", hash + salt));

                    sqlConn.Open();

                    SqlDataReader sqlReader = sqlCMD.ExecuteReader();
                    while (sqlReader.Read())
                    {
                        MemberID = (int)sqlReader["MemberID"];

                        if (sqlReader["GroupID"] != DBNull.Value)
                        {
                            GroupID = (int)sqlReader["GroupID"];
                            if (GroupID == 1)
                                Session["isAdmin"] = true;
                        }


                        Session["MemberID"] = MemberID;
                    }

                    if (MemberID != null)
                    {
                        Session["MemberID"] = MemberID;
                        Session["UserName"] = UserName;
                        Session["Email"] = UserName;
                        isMember = true;

                    }
                    sqlConn.Close();
                }

            }


            return isMember;
        }

        #region Status Codes
        private static string ErrorCodeToString(MembershipCreateStatus createStatus)
        {
            // See http://go.microsoft.com/fwlink/?LinkID=177550 for
            // a full list of status codes.
            switch (createStatus)
            {
                case MembershipCreateStatus.DuplicateUserName:
                    return "Email already exists. Please enter a different email.";

                case MembershipCreateStatus.DuplicateEmail:
                    return "A user name for that e-mail address already exists. Please enter a different e-mail address.";

                case MembershipCreateStatus.InvalidPassword:
                    return "The password provided is invalid. Please enter a valid password value.";

                case MembershipCreateStatus.InvalidEmail:
                    return "The e-mail address provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidAnswer:
                    return "The password retrieval answer provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidQuestion:
                    return "The password retrieval question provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidUserName:
                    return "The email provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.ProviderError:
                    return "The authentication provider returned an error. Please verify your entry and try again. If the problem persists, please contact your system administrator.";

                case MembershipCreateStatus.UserRejected:
                    return "The user creation request has been canceled. Please verify your entry and try again. If the problem persists, please contact your system administrator.";

                default:
                    return "An unknown error occurred. Please verify your entry and try again. If the problem persists, please contact your system administrator.";
            }
        }
        #endregion
    }
}

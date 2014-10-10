using System;
using System.Diagnostics;
using AuthorizeNet;
using Bettery.Kiosk.Common;
using Bettery.Kiosk.Entities;
using System.Configuration;
using System.Threading;

namespace Bettery.Kiosk.Controllers
{
    public class CheckOutController
    {
        private static AuthorizeNet.MerchantAuthenticationType merchantAuthenticationType = null;
        private static AuthorizeNet.ServiceSoapClient serviceSoapClient = null;

        public static AuthorizeNet.MerchantAuthenticationType MerchantAuthentication
        {
            get
            {
                if (merchantAuthenticationType == null)
                {
                    merchantAuthenticationType = new AuthorizeNet.MerchantAuthenticationType();
                    merchantAuthenticationType.name = BaseController.ApiLogin;
                    merchantAuthenticationType.transactionKey = BaseController.TransactionKey;
                }

                return merchantAuthenticationType;
            }
        }

        public static AuthorizeNet.ServiceSoapClient ServiceSoapClient
        {
            get
            {
                if (serviceSoapClient == null)
                {
                    serviceSoapClient = new AuthorizeNet.ServiceSoapClient();
                }

                return serviceSoapClient;
            }
        }

        /// <summary>
        /// Checks the out.
        /// </summary>
        /// <param name="cardInfo">The card info.</param>
        /// <param name="zipCode">The zip code.</param>
        /// <returns>Error message</returns>
        public static string CheckOut(string cardInfo, string zipCode)
        {
            try
            {
                if (BaseController.SelectedBettery != null)
                {
                    if (BaseController.CurrentTransaction == null)
                    {
                        BaseController.CurrentTransaction = new Entities.TransactionQueueData();
                    }

                    // Gen order number
                    BaseController.SelectedBettery.AaVendRemaining = BaseController.SelectedBettery.AaVend;
                    BaseController.SelectedBettery.AaaVendRemaining = BaseController.SelectedBettery.AaaVend;
                    BaseController.CurrentTransaction.AaVend = BaseController.SelectedBettery.AaVend;
                    BaseController.CurrentTransaction.AaaVend = BaseController.SelectedBettery.AaaVend;
                    BaseController.CurrentTransaction.AaReturn = BaseController.SelectedBettery.AaReturn;
                    BaseController.CurrentTransaction.AaaReturn = BaseController.SelectedBettery.AaaReturn;
                    BaseController.CurrentTransaction.SubTotalAmount = BaseController.SelectedBettery.SubTotalAmount;
                    BaseController.CurrentTransaction.TaxAmount = BaseController.SelectedBettery.TotalTaxAmount;
                    BaseController.CurrentTransaction.ChargeAmount = BaseController.SelectedBettery.TotalAmount;
                    if (BaseController.LoggedOnUser != null)
                    {
                        BaseController.CurrentTransaction.Email = BaseController.LoggedOnUser.UserName;
                        BaseController.CurrentTransaction.BatteryPacksCheckedOut = BaseController.LoggedOnUser.BatteriesCheckedOut + BaseController.SelectedBettery.NewCartridges;
                    }

                    BaseController.CurrentTransaction.AaForgotVend = BaseController.SelectedBettery.AaForgotDrainedVend;
                    BaseController.CurrentTransaction.AaaForgotVend = BaseController.SelectedBettery.AaaForgotDrainedVend;

                    BaseController.CurrentTransaction.PromoCode = BaseController.SelectedBettery.PromotionCode;
                    BaseController.CurrentTransaction.PromoCodeAmount = BaseController.SelectedBettery.PromotionalAmount;
                    BaseController.CurrentTransaction.OrderNumber = Guid.NewGuid().ToString();
                    BaseController.CurrentTransaction.ZipCode = zipCode;

                    // Use the Credit Card Helper to parse cc fields
                    CreditCard cc = new CreditCard(cardInfo);

                    if (BaseController.LoggedOnUser != null)
                        Logger.Log(EventLogEntryType.Information, "User Transaction:  Sending to Authorize.NET Id = " + BaseController.CurrentTransaction.OrderNumber.ToString() + " Card Name = " + cc.Name + " Logged on User = " + BaseController.LoggedOnUser.UserName + " Transaction Amount = " + BaseController.SelectedBettery.TotalAmount.ToString(), BaseController.StationId);
                    else
                        Logger.Log(EventLogEntryType.Information, "User Transaction:  Sending to Authorize.NET Id = " + BaseController.CurrentTransaction.OrderNumber.ToString() + " Card Name = " + cc.Name + " Transaction Amount = " + BaseController.SelectedBettery.TotalAmount.ToString(), BaseController.StationId);

                    if (BaseController.SelectedBettery.TotalAmount > 0)
                    {
                        // TODO: process for credit case

                        int retries = 3;
                        while (true)
                        {
                            try
                            {
                                BaseController.CurrentTransaction.Authorization = ProcessCC(cc.Number, cc.ExpDate, BaseController.SelectedBettery.TotalAmount, BaseController.CurrentTransaction.OrderNumber, zipCode, cc.Name);
                                break; // success!
                            }
                            catch
                            {
                                if (--retries == 0) throw;
                                else
                                {
                                    Logger.Log(EventLogEntryType.Warning, "ProcessCC exception, connection to authorize.net, retrying", BaseController.StationId);
                                    Thread.Sleep(5000);
                                }
                            }
                        }
                        BaseController.CurrentTransaction.CardInfo = cc.Number.Substring(cc.Number.Length - 4);
                        BaseController.CurrentTransaction.NameOnCard = cc.Name;

                        // We use the default SHA-256 & 4 byte length
                        HashUtils hashUtils = new HashUtils();

                        // We have a password, which will generate a Hash and Salt
                        string Hash;

                        // Gen a hash with Card Name and last 4 of card number
                        hashUtils.GetHashString(cc.Name + cc.Number.Substring(cc.Number.Length - 4), out Hash);
                        BaseController.CurrentTransaction.CardHash = Hash; 
                    }
                }

                return string.Empty;
            }
            catch (KioskException kioskException)
            {
                AlertController.TransactionFailureAlert(kioskException.CustomMessage);
                Logger.Log(EventLogEntryType.Error, kioskException, BaseController.StationId);
                // BaseController.RaiseOnThrowExceptionEvent();
                return kioskException.CustomMessage;
            }
            catch (Exception ex)
            {
                AlertController.TransactionFailureAlert(ex.Message);
                Logger.Log(EventLogEntryType.Error, ex, BaseController.StationId);
                // BaseController.RaiseOnThrowExceptionEvent();
                return Constants.Messages.YourCreditCardWasNotAuthorized;
            }
        }

        /// <summary>
        /// Verifies the membership credit.  When creating new Membership.
        /// </summary>
        /// <param name="cardInfo">The card info.</param>
        /// <returns></returns>
        /// 
        public static string VerifyMembershipCredit(string cardInfo)
        {
            try
            {
                if (BaseController.RegistrationUser != null)
                {
                    if (BaseController.CurrentTransaction == null)
                    {
                        BaseController.CurrentTransaction = new Entities.TransactionQueueData();
                    }

                    BaseController.CurrentTransaction.Email = BaseController.RegistrationUser.Email;
                    BaseController.CurrentTransaction.OrderNumber = Guid.NewGuid().ToString();
                    BaseController.CurrentTransaction.ZipCode = BaseController.RegistrationUser.ZipCode;

                    // Use the Credit Card Helper to parse cc fields
                    CreditCard cc = new CreditCard(cardInfo,CreditCard.ExpireDateFormat.YYYY_MM);

                    long customerProfileId = CreateCustomerProfile(BaseController.CurrentTransaction.Email);
                    long customerPaymentProfileId = CreateCustomerPaymentProfile(customerProfileId, cc.Number, cc.ExpDate, BaseController.RegistrationUser.MemberFirstName, BaseController.RegistrationUser.MemberLastName, BaseController.RegistrationUser.ZipCode);

                    BaseController.CurrentTransaction.CustomerProfileId = customerProfileId.ToString();
                    BaseController.CurrentTransaction.PaymentProfileID = customerPaymentProfileId.ToString();

                    int subscriptionPlanId = BaseController.GetSubscriptionPlanId(BaseController.RegistrationUser.BatteriesInPlan);
                    if (subscriptionPlanId != 0)
                    {
                        decimal amount = BaseController.GetPriceBySubscriptionPlanId(subscriptionPlanId);
                        if (amount > 0)
                        {
                            CreateTransaction(customerProfileId, customerPaymentProfileId, amount);
                        }
                    }
                }

                return string.Empty;
            }
            catch (KioskException kioskException)
            {
                AlertController.TransactionFailureAlert(kioskException.CustomMessage);
                Logger.Log(EventLogEntryType.Error, kioskException, BaseController.StationId);
                BaseController.RaiseOnThrowExceptionEvent();
                return kioskException.CustomMessage;
            }
            catch (Exception ex)
            {
                AlertController.TransactionFailureAlert(ex.Message);
                Logger.Log(EventLogEntryType.Error, ex, BaseController.StationId);
                BaseController.RaiseOnThrowExceptionEvent();
                return Constants.Messages.YourCreditCardWasNotAuthorized;
            }
        }

        /// <summary>
        /// Verifies the membership credit.
        /// </summary>
        /// <returns></returns>
        public static string CheckOutForSubscriptionPlan(long customerProfileId, long paymentProfileId)
        {
            try
            {
                if (BaseController.LoggedOnUser != null)
                {
                    if (BaseController.CurrentTransaction == null)
                    {
                        BaseController.CurrentTransaction = new Entities.TransactionQueueData();
                    }

                    BaseController.CurrentTransaction.Email = BaseController.LoggedOnUser.Email;
                    BaseController.CurrentTransaction.OrderNumber = Guid.NewGuid().ToString();
                    BaseController.CurrentTransaction.ZipCode = BaseController.LoggedOnUser.ZipCode;

                    BaseController.CurrentTransaction.CustomerProfileId = customerProfileId.ToString();
                    BaseController.CurrentTransaction.PaymentProfileID = paymentProfileId.ToString();

                    if (BaseController.LoggedOnUser.BatteriesInPlan == 0)
                    {
                        int subscriptionPlanId = BaseController.GetSubscriptionPlanId(BaseController.LoggedOnUser.NewBatteriesInPlan);
                        decimal price = BaseController.GetPriceBySubscriptionPlanId(subscriptionPlanId);
                        decimal amount = price - BaseController.LoggedOnUser.OutstandingCredit;
                        decimal newCreditAmount = 0;

                        if (amount > 0)
                        {
                            CreateTransaction(customerProfileId, paymentProfileId, price);
                            newCreditAmount = 0;
                        }
                        else
                        {
                            newCreditAmount = Math.Abs(amount);
                        }

                        MembershipRegistrationController.UpdateMembershipProfile(BaseController.LoggedOnUser.MemberId, BaseController.LoggedOnUser.CustomerProfileId, BaseController.LoggedOnUser.PaymentProfileId, BaseController.LoggedOnUser.NewBatteriesInPlan, newCreditAmount, true, false);
                    }
                    else
                    {
                        MembershipRegistrationController.UpdateMembershipProfile(BaseController.LoggedOnUser.MemberId, BaseController.LoggedOnUser.CustomerProfileId, BaseController.LoggedOnUser.PaymentProfileId, BaseController.LoggedOnUser.NewBatteriesInPlan, BaseController.LoggedOnUser.OutstandingCredit, true, true);
                    }
                }

                return string.Empty;
            }
            catch (KioskException kioskException)
            {
                AlertController.TransactionFailureAlert(kioskException.CustomMessage);
                Logger.Log(EventLogEntryType.Error, kioskException, BaseController.StationId);
                BaseController.RaiseOnThrowExceptionEvent();
                return kioskException.CustomMessage;
            }
            catch (Exception ex)
            {
                AlertController.TransactionFailureAlert(ex.Message);
                Logger.Log(EventLogEntryType.Error, ex, BaseController.StationId);
                BaseController.RaiseOnThrowExceptionEvent();
                return Constants.Messages.YourCreditCardWasNotAuthorized;
            }
        }

        /// <summary>
        /// Checks the out for subscription plan.
        /// </summary>
        /// <param name="cardInfo">The card info.</param>
        /// <returns></returns>
        public static string CheckOutForSubscriptionPlan(string cardInfo)
        {
            try
            {
                if (BaseController.LoggedOnUser != null)
                {
                    if (BaseController.CurrentTransaction == null)
                    {
                        BaseController.CurrentTransaction = new Entities.TransactionQueueData();
                    }

                    BaseController.CurrentTransaction.Email = BaseController.LoggedOnUser.Email;
                    BaseController.CurrentTransaction.OrderNumber = Guid.NewGuid().ToString();
                    BaseController.CurrentTransaction.ZipCode = BaseController.LoggedOnUser.ZipCode;

                    // Use the Credit Card Helper to parse cc fields
                    CreditCard cc = new CreditCard(cardInfo, CreditCard.ExpireDateFormat.YYYY_MM);

                    long customerProfileId = CreateCustomerProfile(BaseController.LoggedOnUser.Email);
                    long customerPaymentProfileId = CreateCustomerPaymentProfile(customerProfileId, cc.Number, cc.ExpDate, BaseController.RegistrationUser.MemberFirstName, BaseController.RegistrationUser.MemberLastName, BaseController.RegistrationUser.ZipCode);

                    BaseController.CurrentTransaction.CustomerProfileId = customerProfileId.ToString();
                    BaseController.CurrentTransaction.PaymentProfileID = customerPaymentProfileId.ToString();

                    if (BaseController.LoggedOnUser.BatteriesInPlan == 0)
                    {
                        int subscriptionPlanId = BaseController.GetSubscriptionPlanId(BaseController.LoggedOnUser.NewBatteriesInPlan);
                        decimal price = BaseController.GetPriceBySubscriptionPlanId(subscriptionPlanId);
                        decimal amount = price - BaseController.LoggedOnUser.OutstandingCredit;
                        decimal newCreditAmount = 0;

                        if (amount > 0)
                        {
                            CreateTransaction(customerProfileId, customerPaymentProfileId, price);
                            newCreditAmount = 0;
                        }
                        else
                        {
                            newCreditAmount = Math.Abs(amount);
                        }

                        MembershipRegistrationController.UpdateMembershipProfile(BaseController.LoggedOnUser.MemberId, BaseController.LoggedOnUser.CustomerProfileId, BaseController.LoggedOnUser.PaymentProfileId, BaseController.LoggedOnUser.NewBatteriesInPlan, newCreditAmount, true, false);
                    }
                    else
                    {
                        MembershipRegistrationController.UpdateMembershipProfile(BaseController.LoggedOnUser.MemberId, BaseController.LoggedOnUser.CustomerProfileId, BaseController.LoggedOnUser.PaymentProfileId, BaseController.LoggedOnUser.NewBatteriesInPlan, BaseController.LoggedOnUser.OutstandingCredit, true, true);
                    }
                }

                return string.Empty;
            }
            catch (KioskException kioskException)
            {
                AlertController.TransactionFailureAlert(kioskException.CustomMessage);
                Logger.Log(EventLogEntryType.Error, kioskException, BaseController.StationId);
                BaseController.RaiseOnThrowExceptionEvent();
                return kioskException.CustomMessage;
            }
            catch (Exception ex)
            {
                AlertController.TransactionFailureAlert(ex.Message);
                Logger.Log(EventLogEntryType.Error, ex, BaseController.StationId);
                BaseController.RaiseOnThrowExceptionEvent();
                return Constants.Messages.YourCreditCardWasNotAuthorized;
            }
        }
                /// <summary>
        /// Saves Members Card on Files.
        /// </summary>
        /// <returns></returns>
        public static void MembershipSaveCardOnFile(string cardInfo)
        {

            // Use the Credit Card Helper to parse cc fields
            CreditCard cc = new CreditCard(cardInfo, CreditCard.ExpireDateFormat.YYYY_MM);
            long customerProfileId = CreateCustomerProfile(BaseController.LoggedOnUser.Email);
            long customerPaymentProfileId = CreateCustomerPaymentProfile(customerProfileId, cc.Number, cc.ExpDate, BaseController.LoggedOnUser.MemberFirstName, BaseController.LoggedOnUser.MemberLastName, BaseController.LoggedOnUser.ZipCode);

            BaseController.CurrentTransaction.CustomerProfileId = customerProfileId.ToString();
            BaseController.CurrentTransaction.PaymentProfileID = customerPaymentProfileId.ToString();
            
        }

        /// <summary>
        /// Memberships the checkout.
        /// </summary>
        /// <returns></returns>
        public static string MembershipCheckout()
        {
            try
            {
                if (BaseController.LoggedOnUser != null)
                {
                    if (BaseController.CurrentTransaction == null)
                    {
                        BaseController.CurrentTransaction = new Entities.TransactionQueueData();
                    }

                    BaseController.CurrentTransaction.Email = BaseController.LoggedOnUser.Email;
                    BaseController.CurrentTransaction.OrderNumber = Guid.NewGuid().ToString();
                    BaseController.CurrentTransaction.ZipCode = BaseController.LoggedOnUser.ZipCode;
                    decimal price = 0;
                    if (BaseController.SelectedBettery != null)
                    {
                        BaseController.SelectedBettery.AaVendRemaining = BaseController.SelectedBettery.AaVend;
                        BaseController.SelectedBettery.AaaVendRemaining = BaseController.SelectedBettery.AaaVend;
                        BaseController.CurrentTransaction.AaVend = BaseController.SelectedBettery.AaVend;
                        BaseController.CurrentTransaction.AaaVend = BaseController.SelectedBettery.AaaVend;
                        BaseController.CurrentTransaction.AaReturn = BaseController.SelectedBettery.AaReturn;
                        BaseController.CurrentTransaction.AaaReturn = BaseController.SelectedBettery.AaaReturn;
                        BaseController.CurrentTransaction.SubTotalAmount = BaseController.SelectedBettery.SubTotalAmount;
                        if (BaseController.SelectedBettery.CalculatedReturnedAmount > 0)
                            BaseController.CurrentTransaction.ChargeAmount = -BaseController.SelectedBettery.CalculatedReturnedAmount;
                        else
                            BaseController.CurrentTransaction.ChargeAmount = BaseController.SelectedBettery.TotalAmount;

                        BaseController.CurrentTransaction.AaForgotVend = BaseController.SelectedBettery.AaForgotDrainedVend;
                        BaseController.CurrentTransaction.AaaForgotVend = BaseController.SelectedBettery.AaaForgotDrainedVend;

                        BaseController.CurrentTransaction.PromoCode = BaseController.SelectedBettery.PromotionCode;
                        BaseController.CurrentTransaction.PromoCodeAmount = BaseController.SelectedBettery.PromotionalAmount;
                        price = BaseController.SelectedBettery.TotalAmount;
                        BaseController.CurrentTransaction.TaxAmount = BaseController.SelectedBettery.TotalTaxAmount;
                        if (BaseController.LoggedOnUser.CCOnFileLastFourDigits != null)
                            BaseController.CurrentTransaction.CardInfo = BaseController.LoggedOnUser.CCOnFileLastFourDigits;
                        else
                            BaseController.CurrentTransaction.CardInfo = String.Empty;

                        BaseController.CurrentTransaction.BatteryPacksCheckedOut = BaseController.LoggedOnUser.BatteriesCheckedOut + BaseController.SelectedBettery.NewCartridges;
                    }
                    else
                    {
                        BaseController.CurrentTransaction.BatteryPacksCheckedOut = BaseController.LoggedOnUser.BatteriesCheckedOut;
                    }

                    long customerProfileId = 0, customerPaymentProfileId = 0;
                    bool result;
                    result = long.TryParse(BaseController.LoggedOnUser.CustomerProfileId, out customerProfileId);
                    if (result && customerProfileId > 0)
                    {
                        result = long.TryParse(BaseController.LoggedOnUser.PaymentProfileId, out customerPaymentProfileId);

                        if (result && customerPaymentProfileId > 0)
                        {
                            BaseController.CurrentTransaction.CustomerProfileId = BaseController.LoggedOnUser.CustomerProfileId;
                            BaseController.CurrentTransaction.PaymentProfileID = BaseController.LoggedOnUser.PaymentProfileId;

                            if (price > 0)
                            {
                                CreateTransaction(customerProfileId, customerPaymentProfileId, price);
                            }
                            //else
                            //{
                            //    price = Math.Abs(price);
                            //}

                            //MembershipRegistrationController.UpdateMembershipProfile(BaseController.LoggedOnUser.MemberId, BaseController.LoggedOnUser.CustomerProfileId, BaseController.LoggedOnUser.PaymentProfileId, BaseController.LoggedOnUser.BatteriesInPlan, price, true);
                        }
                    }
                }

                return string.Empty;
            }
            catch (KioskException kioskException)
            {
                AlertController.TransactionFailureAlert(kioskException.CustomMessage);
                Logger.Log(EventLogEntryType.Error, kioskException, BaseController.StationId);
                BaseController.RaiseOnThrowExceptionEvent();
                return kioskException.CustomMessage;
            }
            catch (Exception ex)
            {
                AlertController.TransactionFailureAlert(ex.Message);
                Logger.Log(EventLogEntryType.Error, ex, BaseController.StationId);
                BaseController.RaiseOnThrowExceptionEvent();
                return Constants.Messages.YourCreditCardWasNotAuthorized;
            }
        }

        /// <summary>
        /// Memberships the checkout.
        /// </summary>
        /// <param name="cardInfo">The card info.</param>
        /// <returns></returns>
        public static string MembershipCheckout(string cardInfo)
        {
            try
            {
                if (BaseController.LoggedOnUser != null)
                {
                    if (BaseController.CurrentTransaction == null)
                    {
                        BaseController.CurrentTransaction = new Entities.TransactionQueueData();
                    }

                    BaseController.CurrentTransaction.Email = BaseController.LoggedOnUser.Email;
                    BaseController.CurrentTransaction.OrderNumber = Guid.NewGuid().ToString();
                    BaseController.CurrentTransaction.ZipCode = BaseController.LoggedOnUser.ZipCode;
                    decimal amount = 0;
                    if (BaseController.SelectedBettery != null)
                    {
                        BaseController.SelectedBettery.AaVendRemaining = BaseController.SelectedBettery.AaVend;
                        BaseController.SelectedBettery.AaaVendRemaining = BaseController.SelectedBettery.AaaVend;
                        BaseController.CurrentTransaction.AaVend = BaseController.SelectedBettery.AaVend;
                        BaseController.CurrentTransaction.AaaVend = BaseController.SelectedBettery.AaaVend;
                        BaseController.CurrentTransaction.AaReturn = BaseController.SelectedBettery.AaReturn;
                        BaseController.CurrentTransaction.AaaReturn = BaseController.SelectedBettery.AaaReturn;
                        BaseController.CurrentTransaction.SubTotalAmount = BaseController.SelectedBettery.SubTotalAmount;
                        if (BaseController.SelectedBettery.CalculatedReturnedAmount > 0)
                            BaseController.CurrentTransaction.ChargeAmount = -BaseController.SelectedBettery.CalculatedReturnedAmount;
                        else
                            BaseController.CurrentTransaction.ChargeAmount = BaseController.SelectedBettery.TotalAmount;
                        BaseController.CurrentTransaction.AaForgotVend = BaseController.SelectedBettery.AaForgotDrainedVend;
                        BaseController.CurrentTransaction.AaaForgotVend = BaseController.SelectedBettery.AaaForgotDrainedVend;

                        BaseController.CurrentTransaction.PromoCode = BaseController.SelectedBettery.PromotionCode;
                        BaseController.CurrentTransaction.PromoCodeAmount = BaseController.SelectedBettery.PromotionalAmount;
                        amount = BaseController.SelectedBettery.TotalAmount;
                        BaseController.CurrentTransaction.TaxAmount = BaseController.SelectedBettery.TotalTaxAmount;
                        

                        BaseController.CurrentTransaction.BatteryPacksCheckedOut = BaseController.LoggedOnUser.BatteriesCheckedOut + BaseController.SelectedBettery.NewCartridges;
                    }
                    else
                    {
                        BaseController.CurrentTransaction.BatteryPacksCheckedOut = BaseController.LoggedOnUser.BatteriesCheckedOut;
                    }

                    // Use the Credit Card Helper to parse cc fields
                    CreditCard cc = new CreditCard(cardInfo, CreditCard.ExpireDateFormat.YYYY_MM);
                    BaseController.CurrentTransaction.CardInfo = cc.Number.Substring(cc.Number.Length - 4);

                    long customerProfileId = CreateCustomerProfile(BaseController.CurrentTransaction.Email);
                    long customerPaymentProfileId = CreateCustomerPaymentProfile(customerProfileId, cc.Number, cc.ExpDate, BaseController.LoggedOnUser.MemberFirstName, BaseController.LoggedOnUser.MemberLastName, BaseController.LoggedOnUser.ZipCode);

                    BaseController.CurrentTransaction.CustomerProfileId = customerProfileId.ToString();
                    BaseController.CurrentTransaction.PaymentProfileID = customerPaymentProfileId.ToString();

                    BaseController.LoggedOnUser.CustomerProfileId = customerProfileId.ToString();
                    BaseController.LoggedOnUser.PaymentProfileId = customerPaymentProfileId.ToString();

                    if (amount > 0)
                    {
                        CreateTransaction(customerProfileId, customerPaymentProfileId, amount);
                    }

                    MembershipRegistrationController.UpdateMembershipProfile(BaseController.LoggedOnUser.MemberId,
                           BaseController.LoggedOnUser.CustomerProfileId,
                           BaseController.LoggedOnUser.PaymentProfileId);
                }

                return string.Empty;
            }
            catch (KioskException kioskException)
            {
                AlertController.TransactionFailureAlert(kioskException.CustomMessage);
                Logger.Log(EventLogEntryType.Error, kioskException, BaseController.StationId);
                BaseController.RaiseOnThrowExceptionEvent();
                return kioskException.CustomMessage;
            }
            catch (Exception ex)
            {
                AlertController.TransactionFailureAlert(ex.Message);
                Logger.Log(EventLogEntryType.Error, ex, BaseController.StationId);
                BaseController.RaiseOnThrowExceptionEvent();
                return Constants.Messages.YourCreditCardWasNotAuthorized;
            }
        }

        /// <summary>
        /// Processes the CC.
        /// </summary>
        /// <param name="ccNumber">The cc number.</param>
        /// <param name="ccExpDate">The cc exp date.</param>
        /// <param name="chargeAmount">The charge amount.</param>
        /// <param name="orderNumber">The order number.</param>
        /// <param name="zipCode">The zip code.</param>
        /// <returns></returns>
        private static string ProcessCC(string ccNumber, string ccExpDate, decimal chargeAmount, string orderNumber, string zipCode, string name)
        {
            try
            {
                if (chargeAmount > 0)
                {
                    var request = new AuthorizationRequest(ccNumber, ccExpDate, chargeAmount, "Bettery Charge");
                    request.Zip = zipCode;

                    //  Store the Swap Station ID in the CustID field
                    request.CustId = ConfigurationManager.AppSettings[Constants.SettingKeys.StationId];
                    
                    //  Add customer info if user is logged in.
                    //if (BaseController.CurrentTransaction.Email != null || BaseController.CurrentTransaction.Email != String.Empty)
                    //    request.Email = BaseController.CurrentTransaction.Email;

                    if (BaseController.LoggedOnUser != null)
                    {
                        request.AddCustomer(BaseController.LoggedOnUser.MemberId.ToString(), BaseController.LoggedOnUser.MemberFirstName, BaseController.LoggedOnUser.MemberLastName, "", "", BaseController.CurrentTransaction.ZipCode);
                    }
                    else
                    {
                        request.FirstName = name;
                        request.Zip = BaseController.CurrentTransaction.ZipCode;

                    }
                        

                    // Get Test Request setting from config file
                    request.TestRequest = ConfigurationManager.AppSettings["TestTransaction"].ToString();

                    
                    // Set Test Request true for admins
                    if (BaseController.LoggedOnUser != null && (BaseController.LoggedOnUser.GroupID == Constants.Group.SuperUser || BaseController.LoggedOnUser.GroupID == Constants.Group.SwapStationAdmin || BaseController.LoggedOnUser.GroupID == Constants.Group.CompanyAccount))
                        request.TestRequest = "TRUE";

                    //order number
                    request.AddInvoice(orderNumber);

                    var gate = new CardPresentGateway(BaseController.ApiLogin, BaseController.TransactionKey, false);

                    //step 3 - make some money
                    var response = gate.Send(request);
                    if (!response.Approved)
                    {
                        BaseController.CurrentTransaction.ZipCode = null;
                        throw new KioskException(Constants.Messages.NonApprovedRequest);
                    }
                    BaseController.CurrentTransaction.TransactionID = response.TransactionID;
                    return response.AuthorizationCode;
                }
                else
                    return "";
            }
            catch (KioskException kioskException)
            {
                AlertController.AuthorizeDotNetAlert(kioskException);
                throw kioskException;
            }
            catch (Exception ex)
            {
                KioskException kioskException = new KioskException(Constants.Messages.CouldNotConnectToAuthorize);
                kioskException.OriginalException = ex;

                AlertController.AuthorizeDotNetAlert(ex);
                Logger.Log(EventLogEntryType.Error, "Could Not Connect to Authorize.NET or other Authorize.NET failure", BaseController.StationId);
                throw kioskException;
            }
        }

        /// <summary>
        /// Get the profile for the specified customer
        /// </summary>
        /// <param name="profileId">The ID for the customer that we are retrieving</param>
        /// <returns>The profile retrieved</returns>
        public static AuthorizeNet.CustomerProfileMaskedType GetCustomerProfile(long profileId)
        {
            AuthorizeNet.CustomerProfileMaskedType profile = null;

            try
            {
                AuthorizeNet.GetCustomerProfileResponseType responseType = ServiceSoapClient.GetCustomerProfile(MerchantAuthentication, profileId);

                profile = responseType.profile;

                if (responseType.resultCode == AuthorizeNet.MessageTypeEnum.Error)
                {
                    throw new KioskException(string.Format("Could not get customer profile for profile id: {0}.", profileId));
                }
            }
            catch (KioskException kioskException)
            {
                AlertController.AuthorizeDotNetAlert(kioskException);
                throw kioskException;
            }
            catch (Exception ex)
            {
                KioskException kioskException = new KioskException(Constants.Messages.CouldNotConnectToAuthorize);
                kioskException.OriginalException = ex;

                AlertController.AuthorizeDotNetAlert(ex);
                throw kioskException;
            }

            return profile;
        }

        /// <summary>
        /// Gets the customer payment profile.
        /// </summary>
        /// <param name="profileId">The profile id.</param>
        /// <param name="paymentProfileId">The profile payment id.</param>
        /// <returns></returns>
        public static AuthorizeNet.CustomerPaymentProfileMaskedType GetCustomerPaymentProfile(long profileId, long paymentProfileId)
        {
            AuthorizeNet.CustomerPaymentProfileMaskedType paymentProfile = null;

            try
            {
                AuthorizeNet.GetCustomerPaymentProfileResponseType responseType = ServiceSoapClient.GetCustomerPaymentProfile(MerchantAuthentication, profileId, paymentProfileId);
                paymentProfile = responseType.paymentProfile;
                if (responseType.resultCode == AuthorizeNet.MessageTypeEnum.Error)
                {
                    throw new KioskException(string.Format("Could not get customer payment profile for profile id/ payment profile Id: {0}/{1}.", profileId, paymentProfileId));
                }
            }
            catch (KioskException kioskException)
            {
                AlertController.AuthorizeDotNetAlert(kioskException);
                throw kioskException;
            }
            catch (Exception ex)
            {
                KioskException kioskException = new KioskException(Constants.Messages.CouldNotConnectToAuthorize);
                kioskException.OriginalException = ex;

                AlertController.AuthorizeDotNetAlert(ex);
                throw kioskException;
            }

            return paymentProfile;
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
                AuthorizeNet.CustomerProfileType m_new_cust = new AuthorizeNet.CustomerProfileType();
                m_new_cust.email = emailAddress;
                m_new_cust.description = "Kiosk customer " + DateTime.Now.ToShortTimeString();
                AuthorizeNet.CreateCustomerProfileResponseType response = ServiceSoapClient.CreateCustomerProfile(MerchantAuthentication, m_new_cust, AuthorizeNet.ValidationModeEnum.none);

                customerProfileId = response.customerProfileId;

                if (response.resultCode == AuthorizeNet.MessageTypeEnum.Error)
                {
                    throw new KioskException(string.Format("Could not create customer profile for email: {0}.", emailAddress));
                }
            }
            catch (KioskException kioskException)
            {
                AlertController.AuthorizeDotNetAlert(kioskException);
                throw kioskException;
            }
            catch (Exception ex)
            {
                KioskException kioskException = new KioskException(Constants.Messages.CouldNotConnectToAuthorize);
                kioskException.OriginalException = ex;

                AlertController.AuthorizeDotNetAlert(ex);
                throw kioskException;
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
                AuthorizeNet.CustomerPaymentProfileType new_payment_profile = new AuthorizeNet.CustomerPaymentProfileType();
                AuthorizeNet.PaymentType new_payment = new AuthorizeNet.PaymentType();
                AuthorizeNet.CreditCardType new_card = new AuthorizeNet.CreditCardType();
                new_card.cardNumber = ccNumber;
                new_card.expirationDate = expire_date;
                new_payment.Item = new_card;
                new_payment_profile.payment = new_payment;
                new_payment_profile.billTo = new AuthorizeNet.CustomerAddressType
                {
                    firstName = firstName,
                    lastName = lastName,
                    zip = zipCode
                };

                AuthorizeNet.CreateCustomerPaymentProfileResponseType response = ServiceSoapClient.CreateCustomerPaymentProfile(MerchantAuthentication, profile_id, new_payment_profile, AuthorizeNet.ValidationModeEnum.testMode);
                customerPaymentProfileId = response.customerPaymentProfileId;

                if (response.resultCode == AuthorizeNet.MessageTypeEnum.Error)
                {
                    throw new KioskException(string.Format("Could not create customer payment profile for profile id: {0}.", profile_id));
                }
            }
            catch (KioskException kioskException)
            {
                AlertController.AuthorizeDotNetAlert(kioskException);
                throw kioskException;
            }
            catch (Exception ex)
            {
                KioskException kioskException = new KioskException(Constants.Messages.CouldNotConnectToAuthorize);
                kioskException.OriginalException = ex;

                AlertController.AuthorizeDotNetAlert(ex);
                throw kioskException;
            }

            return customerPaymentProfileId;
        }

        /// <summary>
        /// Creates the transaction.
        /// </summary>
        /// <param name="profile_id">The profile_id.</param>
        /// <param name="payment_profile_id">The payment_profile_id.</param>
        /// <param name="amount">The amount.</param>
        public static void CreateTransaction(long profile_id, long payment_profile_id, decimal amount)
        {
            try
            {
                AuthorizeNet.ProfileTransAuthCaptureType auth_capture = new AuthorizeNet.ProfileTransAuthCaptureType();
                auth_capture.customerProfileId = profile_id;
                auth_capture.customerPaymentProfileId = payment_profile_id;
                auth_capture.amount = amount;
                auth_capture.order = new AuthorizeNet.OrderExType();
                auth_capture.order.invoiceNumber = "invoice" + DateTime.Now.ToShortTimeString();
                AuthorizeNet.ProfileTransactionType trans = new AuthorizeNet.ProfileTransactionType();
                trans.Item = auth_capture;

                AuthorizeNet.CreateCustomerProfileTransactionResponseType response = ServiceSoapClient.CreateCustomerProfileTransaction(MerchantAuthentication, trans, null);

                if (response.resultCode == AuthorizeNet.MessageTypeEnum.Error)
                {
                    throw new KioskException(string.Format("Could not create transaction for profile id/ payment profile id: {0}/{1}.", profile_id, payment_profile_id));
                }

            }
            catch (KioskException kioskException)
            {
                AlertController.AuthorizeDotNetAlert(kioskException);
                throw kioskException;
            }
            catch (Exception ex)
            {
                KioskException kioskException = new KioskException(Constants.Messages.CouldNotConnectToAuthorize);
                kioskException.OriginalException = ex;

                AlertController.AuthorizeDotNetAlert(ex);
                throw kioskException;
            }
        }
    }
}

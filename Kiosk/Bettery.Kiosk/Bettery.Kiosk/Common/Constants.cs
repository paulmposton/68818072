namespace Bettery.Kiosk.Common
{
    /// <summary>
    /// Class Constants
    /// </summary>
    public class Constants
    {
        /// <summary>
        /// Class Bettery Price
        /// </summary>
        public class BetteryPrice
        {
            public const decimal SwapPrice = 2.50M;
            public const decimal NewPrice = 7.50M;
            public const decimal ReturnPrice = 8.00M;  // Unused?  CK
        }


        /// <summary>
        /// Class Bettery Price
        /// </summary>
        public class Group
        {
            public const int SuperUser = 1;
            public const int SwapStationAdmin = 2;
            public const int CompanyAccount = 3;
        }
                /// <summary>
        /// Class Bettery Product
        /// </summary>
        public class PromotionType
        {
            /// <summary>
            /// Get Swap Promo type
            /// </summary>
            public const int Swap = 5;


            /// <summary>
            /// Get Purchase Promo type
            /// </summary>
            public const int Purchase = 6;

            /// <summary>
            /// Get Swap/Purchase Promo type
            /// </summary>
            public const int SwapAndPurchase = 7;
            

        }

        /// <summary>
        /// Class Bettery Product
        /// </summary>
        public class BetteryProduct
        {
            /// <summary>
            /// Get max Aa / exchange
            /// </summary>
            public const int AaMax = 9;

            /// <summary>
            /// Get max Aaa / exchange
            /// </summary>
            public const int AaaMax = 9;

            /// <summary>
            /// Get max Aaa return / exchange
            /// </summary>
            public const int AaReturnMax = 9;

            /// <summary>
            /// Get max Aaa return / exchange
            /// </summary>
            public const int AaaReturnMax = 9;

            /// <summary>
            /// Get max of empty cases in the kiosk
            /// </summary>
            public const int EmptyCasesReturnMax = 9;

            /// <summary>
            /// Get max of Bin capacity
            /// </summary>
            public const int BinFullMax = 12;

            /// <summary>
            /// AAA Product ID
            /// </summary>
            public const int AaaID = 1;

            /// <summary>
            /// AA Product ID
            /// </summary>
            public const int AaID = 2;

            /// <summary>
            /// CASE Product ID
            /// </summary>
            public const int CaseID = 3;


            /// <summary>
            /// AAA Product ID
            /// </summary>
            public const string AaaDesc = "AAA";

            /// <summary>
            /// AA Product ID
            /// </summary>
            public const string AaDesc = "AA";

            /// <summary>
            /// CASE Product ID
            /// </summary>
            public const string CaseDesc = "CASE";


        }

        /// <summary>
        /// Class Views Name
        /// </summary>
        public class ViewName
        {
            public const string Checkout = "CheckoutUserControl";
            public const string Confirmation = "ConfirmationUserControl";

            public const string CreditSwap = "CreditSwapUserControl";
            public const string EmailReceipt = "EmailReceiptUserControl";
            public const string Exchange = "ExchangeUserControl";
            public const string Faq = "FAQUserControl";
            public const string LearnMore = "LearnMoreUserControl";
            public const string LearnMoreHow = "LearnMoreHowUserControl";
            public const string LearnMorePerformance = "LearnMorePerformanceUserControl";
            public const string LearnMoreSavings = "LearnMoreSavingsUserControl";
            public const string LearnMoreEnvironment = "LearnMoreEnvironmentUserControl";
            public const string Recycle = "RecycleUserControl";

            public const string GetBatteries = "GetBatteriesUserControl";

            public const string GetCase = "GetCaseUserControl";
            public const string Login = "LoginUserControl";
            public const string ErrorMessage = "ErrorMessageUserControl";
            public const string MembershipRegistration = "MembershipRegistrationUserControl";
            public const string MembershipSubscription = "MembershipSubscriptionUserControl";

            public const string MembershipVerification = "MembershipVerificationUserControl";
            public const string Privacy = "PrivacyUserControl";
            public const string Terms = "TermsUserControl";
            public const string AnonymousTerms = "AnonymousTermsUserControl";

            public const string PromoCodes = "PromoCodesUserControl";
            public const string ReturnSummary = "ReturnSummaryUserControl";
            public const string Selection = "SelectionUserControl";
            public const string Start = "StartUserControl";
            public const string ThankYou = "ThankYouUserControl";
            public const string TransactionSummary = "TransactionSummaryUserControl";
            public const string Vending = "VendingUserControl";
            public const string VendingContinue = "VendingContinueUserControl";
            public const string Welcome = "WelcomeUserControl";
            public const string ZipCode = "ZipCodeUserControl";
            public const string UserProfile = "UserProfileUserControl";
            public const string InventoryAdmin = "InventoryAdminControl";
            public const string CardChoice = "CardChoiceControl";
            public const string AddCardToAccount = "AddCardToAccount";

        }

        /// <summary>
        /// Class Messages
        /// </summary>
        public class Messages
        {
            public const string DefaultWaitPopup = "PLEASE WAIT";
            public const string PriceMessage = "${0:N2}";
            public const string TaxMessage = "${0:N2}";
            public const string ReturnPriceMessage = "${0:N2}";
            public const string OutOfAABatteries = " This BETTERY Swap Station is out of AA batteries, would you like to continue?";
            public const string OutOfAAABatteries = " This BETTERY Swap Station is out of AAA batteries, would you like to continue?";
            public const string OutOfBatteries = " This BETTERY Swap Station is out of batteries, would you like to continue?";
            public const string OutOfService = "This BETTERY Swap Station is out of service.  Please come back later, visit www.BETTERYinc.com for other locations, or call (800)758-1339.";
            public const string SerialPortMessage = "!{0,3:000}_";
            public const string LoginWarning = "*minimum 6 characters";
            public const string LoginFaith = "Invalid email address and/or password.";
            public const string RegisteredEmailError = "You or someone already registered an account with this email address.";
            public const string BetteriesReturned = "You have inserted {0} battery pack(s).";
            public const string MemberCreditSwap = "Hello, {0} you are asking us to vend fewer battery packs than you have returned. Do you want additional battery packs, or would you like BETTERY to credit your account with a deposit on file of {1} packs of Bettery batteries, to be applied toward future swaps?";  //Unsed
            public const string GuestCreditSwap = "Hello, you are asking us to vend fewer battery packs than as you have returned.  You can either increase the number of battery packs that you are asking for or create an account so that BETTERY can apply a credit that you can use for future swaps. Creating an account is quick and easy. Bettery will credit your account with deposits for {0} packs of Bettery batteries, to be applied toward future purchases.";  //Unused
            
            public const string SwapForgotBatteryNotice = "You are committed to bringing {0} pack(s) of BETTERY AA batteries, {1} pack(s) of drained BETTERY AAA batteries back to kiosk within 48 hours"; //Unused
            public const string YouHaveReturnMessage = "";
            public const string YouHaveCheckedOutMessage = "You have checked out";
            public const string ValueReturnMessage = "You have returned {0} pack(s) of BETTERY batteries.";
            public const string NewMessage = "";
            public const string SelectionGetMessage = "Get a charged 4-pack of batteries for ${0:N2}, plus a $5 one-time deposit.";
            public const string SelectionGetMessageSpecial = "Get a charged 4-pack of batteries for ${0:N2} (regularly $2.50), plus a $5 one-time deposit.";
            public const string SelectionSwapMessage = "Swap a drained 4-pack for a charged one, for ${0:N2}.";
            public const string SelectionSwapMessageSpecial = "Swap a drained 4-pack for a charged one, for ${0:N2} (regularly $2.50).";
            //public const string SelectionSpecialMessage = "Now with limited time extra savings over our regular price of $2.50 per 4-pack.";
            public const string NewSelectMessage = "Swap up to {0} pack(s) of batteries for ${1:N2} per 4-pack.";
            public const string BuyNewMessage = "Get 4-packs for ${0:N2} per pack, with a one-time $5 deposit per pack.";
            public const string BuyNewAdditionalMessage = "Get additional 4-packs for ${0:N2} per pack, with a one-time $5 deposit per pack.";
            public const string LimitedBatteries = "You have reached the available limit at this time.";
            public const string LimitedBatteriesInPlan = "You have reached the limit of your plan. Please touch the Profile button below to upgrade to a bigger plan.";  //Unused
            public const string LimitedBatteriesCheckedOut = "You have reached the limit of your plan.";  //Unused?
            public const string SubscriptionPlanStatus = "You have {0} battery pack(s) checked out.  You have {1} battery pack(s) left on your subscription.";  //Unused?
            public const string NonApprovedRequest = "Your card was not approved, please try a different card.";
            public const string NoSelectedPlan = "You have not signed up for a subscription plan";
            public const string CurrentPlan = "Your current subscription plan is [{0}]";  //Unused
            public const string SelectedPlan = "Your have selected the [{0}] plan";  //Unused
            public const string UserProfileAccountCreditAmount = "Your account credit is ${0:N2}";  //Unused?
            // CreaditSwap
            public const string KeepDepositOnFile = "Keep deposit on file";

            public const string CreateCustomerAccount = "Create Customer Account";

            // messages in the Checkout control
            public const string SwappedPaymentSummary = "{0} pack(s) of four batteries swapped @ {1:N2} = ${2:N2}\n";

            public const string ForgotPaymentSummary = "{0} 4-pack(s) of swapped for forgot pack(s) @ {1:N2} = ${2:N2}\n";  //Not enabled at this time
            public const string AddNewPaymentSummary = "{0} new 4-pack(s) @ {1:N2} = ${2:N2}\n\n";
            public const string ReturnedPaymentSummary = "{0} 4-pack(s) returned @ -{1:N2} = -${2:N2}\n\n";
            public const string TaxCheckout = "Sales Tax = ${0:N2}\n\n";
            public const string TotalCheckout = "Your total = ${0:N2}\n\n";
            public const string CreditCheckout = "Your credit = ${0:N2}\n\n";
            public const string YourAccountCredit = "Your account credit = ${0:N2}\n\n";
            public const string YourPromotionCredit = "Your promotion credit = ${0:N2}\n\n";
            public const string YourCreditCardWasNotAuthorized = "Your card was not approved, please try a different card.";
            public const string CouldNotConnectToAuthorize = "We cannot contact the credit/debit card authorization service at this time, please try again.";

            // Vending Messages
            public const string VendingEmptyCasesMessage = "Vending {0} of {1} empty case(s)";
            //public const string VendingMessage = "Vending package {0} of {1} AA battery packs\nVending package {2} of {3} AAA battery packs";
            public const string VendingMessage = "Please wait...\nVending pack {0} of {1}";

            public const string GetCaseMessage = "You have selected {0} of {1} free case(s)";
            public const string LimittedEmptyPackages = "You have {0} free cases in your BETTERY membership. Call BETTERY at 800-759-1339 for more.";
            public const string LimittedInventory = "You have reached the limit of the Swap Station inventory of this product available at this time.";

            // Confirmation
            public const string ReturnBatteriesConfirmMessage = "You have inserted battery packs. Starting over will cause you to lose the credit for those packs.  Are you sure you want to start over?";

            public const string GetOrForgotBatteriesConfirmMessage = "You are about to remove your previous selections, are you sure you want to start over?";

            public const string MatchEmailPattern =
                @"^(([\w-]+\.)+[\w-]+|([a-zA-Z]{1}|[\w-]{2,}))@"
                + @"((([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])\.([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])\."
                + @"([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])\.([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])){1}|"
                + @"([a-zA-Z]+[\w-]+\.)+[a-zA-Z]{2,4})$";

            // CountDown
            public const string DefaultMessage = "Do you need more time?";

            public const string ReturnBatteriesMessage = "You have inserted battery packs. The timeout will cause you to lose the credit for those packs. Do you need more time?";

            // Zip Code
            public const string Zipcode = "Your entered zip code is not valid.";

            // MembershipSubscription
            public const string UserIsNotLoggedIn = "Your user name is not recognized, please enter a new user name or try again.";

            public const string UserIsNotRegistered = "Your user information could not be recognized, please enter a new user name or try again.";
            public const string SubscriptionPlanMessage = "{0} batteries at a time ${1} per month (you are currently on this plan) This plan allows you to have up to {0} batteries checked out at one time and you can make an unlimited* number of swaps per month.";
            public const string CurrentSubscriptionPlan = "{0} batteries at a time ${1} per month";
            public const string SubscriptionPlanKey = "SubscriptionPlan{0}";

            //Promotion
            public const string InvalidPromotionCode =
                "The promotional code you entered is either invalid or has been previously used.";

            public const string InvalidNewPurchasePromotionCode =
                "The promotional code you entered is can only be used for new battery pack purchases";

            public const string InvalidSwapPromotionCode =
                "The promotional code you entered can only be used for swapping battery packs";

            public const string TooManyInvalidPromotionCodeTries = "Sorry, but you've exceeded your tries for entering a valid promotional code.";
            public const string SelectedSubscriptionPlan = "You have selected the {0} plan.";
        }

        /// <summary>
        /// Class Setting Keys
        /// </summary>
        public sealed class SettingKeys
        {
            public const string PortName = "PortName";

            public const string ConnectionString = "ConnectionString";
            public const string ServiceBusconnectionString = "ServiceBusconnectionString";

            public const string VendPauseCount = "VendPauseCount";
            
            public const string ApiLogin = "ApiLogin";
            public const string TransactionKey = "TransactionKey";

            public const string ServiceCallRetries = "ServiceCallRetries";

            public const string RollBallSwitchInputIndex = "RollBallSwitchInputIndex";
            public const string ContactSwitchInputIndex = "ContactSwitchInputIndex";

            public const string MaxIdleTime = "MaxIdleTime";
            public const string ShowCountDown = "ShowCountDown";
            public const string StationId = "StationId";
            public const string RecycleBinCapacity = "RecycleBinCapacity";
            public const string ReturnBinCapacity = "ReturnBinCapacity";
            public const string SerialPortTimeout = "SerialPortTimeout";
            public const string SerialPortRetry = "SerialPortRetry";

            public const string MinAaRemainingAlert = "MinAARemainingAlert";
            public const string MinAaaRemainingAlert = "MinAAARemainingAlert";
            public const string MinCartridgeRemainingAlert = "MinCartridgeRemainingAlert";

            public const string AlertInterval = "AlertInterval";
            public const string MaxInvalidPromotionCodeAttempts = "MaxInvalidPromotionCodeAttempts";
        }
    }
}

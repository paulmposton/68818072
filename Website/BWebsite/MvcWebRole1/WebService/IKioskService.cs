using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using WebRole1.HelperClasses;

namespace MvcWebRole1.WebService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IKioskService" in both code and config file together.
    [ServiceContract]
    public interface IKioskService
    {
        [OperationContract]
        BetteryMember AuthenticateUser(string UserName, string Password);

        [OperationContract]
        bool AddBetteryMember(string FirstName, string LastName, string Email, string Password, string Zipcode, bool GetEmails, int SubscriptionPlan);

        [OperationContract(Name = "UpdateMembership")]
        void UpdateMembershipProfile(int MemberID, string CustomerProfileID, string PaymentProfileID, int BatteriesInPlan, decimal NewAccountBalance, bool isUpgrade);

        [OperationContract(Name = "UpdateProfile")]
        void UpdateMembershipProfile(int MemberID, string CustomerProfileID, string PaymentProfileID);

        [OperationContract]
        bool KioskPing();

        [OperationContract]
        bool CheckEmail(string EmailAddress);

        [OperationContract]
        Promo GetPromoCredit(string PromoCode);

        [OperationContract]
        void RedeemPromoCode(string PromoCode);

        [OperationContract]
        void ReturnBinFullAlert(string KioskID);

        [OperationContract]
        void ProductInventoryAlert(string KioskID, int ProductID);

        [OperationContract]
        void AuthorizeDotNetAlert(string KioskID, string ErrorMessage);

        [OperationContract]
        void TransactionFailureAlert(string KioskID, string ErrorMessage);

        [OperationContract]
        void EmptyCaseVend(int MemberID, int EmptyCases);
    }
}

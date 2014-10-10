using System;
using System.Diagnostics;
using Bettery.Kiosk.BService;
using Bettery.Kiosk.Common;
using Bettery.Kiosk.Entities;

namespace Bettery.Kiosk.Controllers
{
    /// <summary>
    /// Class Login Controller
    /// </summary>
    public class LoginController
    {
        /// <summary>
        /// Occurs when [on login].
        /// </summary>
        public static event EventHandler OnLogin;

        /// <summary>
        /// Authenticates the user.
        /// </summary>
        /// <param name="userName">Name of the user.</param>
        /// <param name="password">The password.</param>
        /// <returns></returns>
        public static bool? AuthenticateUser(string userName, string password)
        {
            BetteryMember betteryMember;
            using (KioskServiceClient bKioskService = new KioskServiceClient())
            {
                try
                {
                    betteryMember = bKioskService.AuthenticateUser(userName, password);
                }
                catch (Exception ex)
                {
                    Logger.Log(EventLogEntryType.Error, ex, BaseController.StationId);
                    BaseController.RaiseOnThrowExceptionEvent();

                    return null;
                }
                finally
                {
                    bKioskService.Close();
                }
            }

            if (betteryMember != null)
            {
                BaseController.LoggedOnUser = new BetteryUser(userName, password)
                {
                    BatteriesCheckedOut = betteryMember.BatteryPacksCheckedOut,
                    BatteriesInPlan = betteryMember.BatteryPacksInPlan,
                    CustomerProfileId = betteryMember.CustomerProfileID,
                    PaymentProfileId = betteryMember.PaymentProfileID,
                    FreeCasesRemaining = betteryMember.FreeCases,
                    Email = userName,
                    MemberFirstName = betteryMember.MemberFirstName,
                    MemberId = betteryMember.MemberID,
                    MemberLastName = betteryMember.MemberLastName,
                    MemberTotalBatteries = betteryMember.MemberTotalBatteries,
                    OutstandingCredit = betteryMember.AccountBalance,
                    CCOnFileExPireDate = betteryMember.CCExPireDate,
                    CCOnFileLastFourDigits = betteryMember.CCLastFourDigits,
                    GroupID = betteryMember.GroupID
                    
                };

                BaseController.LoggedOnUser.NewBatteriesInPlan = BaseController.LoggedOnUser.BatteriesInPlan;

                if (OnLogin != null)
                {
                    //Reset selection amount
                    if (BaseController.LoggedOnUser.BatteriesInPlan > 0)
                    {
                        if (BaseController.SelectedBettery != null)
                        {
                            BaseController.CalcCharges(0, 0, BaseController.GetBatteriesMode);

                            if (BaseController.RecentViewOfCurrentFlow == Constants.ViewName.Checkout || BaseController.RecentViewOfCurrentFlow == Constants.ViewName.TransactionSummary || BaseController.RecentViewOfCurrentFlow == Constants.ViewName.CreditSwap)
                            {
                                if (BaseController.SelectedBettery.ReturnedCartridges > 0)
                                {
                                    BaseController.RecentViewOfCurrentFlow = Constants.ViewName.GetBatteries;
                                }
                                else
                                {
                                    BaseController.RecentViewOfCurrentFlow = Constants.ViewName.Selection;
                                }
                            }
                        }
                    }

                    OnLogin.Invoke(null, EventArgs.Empty);
                }

                return true;
            }

            return false;
        }
    }
}
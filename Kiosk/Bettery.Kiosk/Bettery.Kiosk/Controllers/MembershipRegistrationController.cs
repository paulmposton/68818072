using System;
using System.Diagnostics;
using Bettery.Kiosk.BService;
using Bettery.Kiosk.Common;
using Bettery.Kiosk.Entities;

namespace Bettery.Kiosk.Controllers
{
    /// <summary>
    /// Class MembershipRegistration Controller
    /// </summary>
    public class MembershipRegistrationController
    {
        /// <summary>
        /// Saves the registration user.
        /// </summary>
        /// <returns>Save registration user of state</returns>
        public static bool? SaveRegistrationUser()
        {
            bool? isSuccess = true;

            BetteryUser user = BaseController.RegistrationUser;

            using (KioskServiceClient client = new KioskServiceClient())
            {
                try
                {
                    string Hash;
                    string Salt;
                    HashUtils hashUtils = new HashUtils();
                    hashUtils.GetHashAndSaltString(user.Password, out Hash, out Salt);
                    user.PasswordDigest = Hash + Salt;
                    isSuccess = client.AddBetteryMember(user.MemberFirstName, user.MemberLastName, user.Email, user.PasswordDigest, user.ZipCode, user.GetEmails, 0);
                }
                catch (Exception ex)
                {
                    Logger.Log(EventLogEntryType.Error, ex, BaseController.StationId);
                    BaseController.RaiseOnThrowExceptionEvent();
                    isSuccess = null;
                }
                finally
                {
                    client.Close();
                }
            }

            return isSuccess;
        }

        /// <summary>
        /// Checks the email.
        /// </summary>
        /// <param name="email">The email.</param>
        /// <returns>Is Valid Email</returns>
        public static bool CheckEmail(string email)
        {
            bool isSuccess;
            using (KioskServiceClient client = new KioskServiceClient())
            {
                try
                {
                    isSuccess = !client.CheckEmail(email);
                }
                catch (Exception ex)
                {
                    Logger.Log(EventLogEntryType.Error, ex, BaseController.StationId);
                    BaseController.RaiseOnThrowExceptionEvent();
                    isSuccess = false;
                }
                finally
                {
                    client.Close();
                }
            }

            return isSuccess;
        }

        /// <summary>
        /// Updates the membership profile.
        /// </summary>
        /// <param name="MemberID">The member ID.</param>
        /// <param name="CustomerProfileID">The customer profile ID.</param>
        /// <param name="PaymentProfileID">The payment profile ID.</param>
        /// <param name="BatteriesInPlan">The batteries in plan.</param>
        /// <param name="newAccountBalance">The new account balance.</param>
        /// <param name="raiseEvent">if set to <c>true</c> [raise event].</param>
        /// <param name="isUpgrade">if set to <c>true</c> [is upgrade].</param>
        public static void UpdateMembershipProfile(int MemberID, string CustomerProfileID, string PaymentProfileID, int BatteriesInPlan, decimal newAccountBalance, bool raiseEvent, bool isUpgrade)
        {
            using (KioskServiceClient client = new KioskServiceClient())
            {
                try
                {
                    client.UpdateMembership(MemberID, CustomerProfileID, PaymentProfileID, BatteriesInPlan, newAccountBalance, isUpgrade);
                }
                catch (Exception ex)
                {
                    if (raiseEvent)
                    {
                        throw new KioskException(ex.Message, ex);
                    }
                }
                finally
                {
                    client.Close();
                }
            }
        }

        /// <summary>
        /// Updates the membership profile.
        /// </summary>
        /// <param name="MemberID">The member ID.</param>
        /// <param name="CustomerProfileID">The customer profile ID.</param>
        /// <param name="PaymentProfileID">The payment profile ID.</param>
        public static void UpdateMembershipProfile(int MemberID, string CustomerProfileID, string PaymentProfileID)
        {
            using (KioskServiceClient client = new KioskServiceClient())
            {
                try
                {
                    client.UpdateProfile(MemberID, CustomerProfileID, PaymentProfileID);
                }
                catch (Exception ex)
                {
                    throw new KioskException(ex.Message, ex);
                }
                finally
                {
                    client.Close();
                }
            }
        }
    }
}

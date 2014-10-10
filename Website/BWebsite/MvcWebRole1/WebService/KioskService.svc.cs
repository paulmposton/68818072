using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Data.SqlClient;
using System.Configuration;
using WebRole1.HelperClasses;
using WebRole1.Common;
using System.Data;

namespace MvcWebRole1.WebService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "KioskService" in code, svc and config file together.
    public class KioskService : IKioskService
    {
        public BetteryMember AuthenticateUser(string UserName, string Password)
        {
            bool isMember = false;
            BetteryMember Member = new BetteryMember();

            SqlConnection sqlConn = new SqlConnection(ConfigurationManager.ConnectionStrings["MemberDBContext"].ToString());
            try
            {
                bool isAuthed = false;
                string passwordDigest = String.Empty; 
                HashUtils hashUtils = new HashUtils();

                SqlCommand sqlCMD = new SqlCommand("GetPasswordDigest", sqlConn);
                sqlCMD.CommandType = CommandType.StoredProcedure;

                sqlCMD.Parameters.Add(new SqlParameter("@UserName", UserName));

                sqlConn.Open();
                if (sqlCMD.ExecuteScalar() != null)
                    passwordDigest = sqlCMD.ExecuteScalar().ToString();

                sqlConn.Close();
                if (passwordDigest != String.Empty)
                {
                    string salt = passwordDigest.Substring(passwordDigest.Length - 8);
                    string hash = passwordDigest.Substring(0, passwordDigest.Length - 8);

                    isAuthed = hashUtils.VerifyHashString(Password, hash, salt);

                }

                if (isAuthed)
                {
                    sqlCMD = new SqlCommand("SwapStationAuthenticate", sqlConn);
                    sqlCMD.CommandType = CommandType.StoredProcedure;

                    sqlCMD.Parameters.Add(new SqlParameter("@UserName", UserName));
                    sqlCMD.Parameters.Add(new SqlParameter("@Password", passwordDigest));

                    sqlConn.Open();
                    SqlDataReader reader = sqlCMD.ExecuteReader();

                    while (reader.Read())
                    {
                        isMember = true;
                        Member.MemberFirstName = reader["FirstName"].ToString();
                        Member.MemberLastName = reader["LastName"].ToString();
                        Member.MemberID = (int)reader["MemberID"];
                        if (Convert.IsDBNull(reader["AccountBalance"]))
                        {
                            Member.AccountBalance = 0m;
                        }
                        else
                        {
                            Member.AccountBalance = reader.GetDecimal(reader.GetOrdinal("AccountBalance"));
                        }

                        if (reader["CustomerProfileID"] != System.DBNull.Value)
                            Member.CustomerProfileID = reader["CustomerProfileID"].ToString();
                        if (reader["PaymentProfileID"] != System.DBNull.Value)
                            Member.PaymentProfileID = reader["PaymentProfileID"].ToString();
                        if (reader["BatteryPacksInPlan"] != System.DBNull.Value)
                            Member.BatteryPacksInPlan = (int)reader["BatteryPacksInPlan"];
                        if (reader["BatteryPacksCheckedOut"] != System.DBNull.Value)
                            Member.BatteryPacksCheckedOut = (int)reader["BatteryPacksCheckedOut"];
                        if (reader["MemberTotalBatteries"] != System.DBNull.Value)
                            Member.MemberTotalBatteries = (int)reader["MemberTotalBatteries"];
                        if (reader["FreeCases"] != System.DBNull.Value)
                            Member.FreeCases = (int)reader["FreeCases"];
                        if (reader["CCExpireDate"] != System.DBNull.Value)
                            Member.CCExPireDate = reader["CCExpireDate"].ToString();
                        if (reader["CCLastFourDigits"] != System.DBNull.Value)
                            Member.CCLastFourDigits = reader["CCLastFourDigits"].ToString();
                        if (reader["CCLastFourDigits"] != System.DBNull.Value)
                            Member.CCLastFourDigits = reader["CCLastFourDigits"].ToString();
                        if (reader["GroupID"] != System.DBNull.Value)
                            Member.GroupID = (int)reader["GroupID"];
                        if (reader["CorpAccountID"] != System.DBNull.Value)
                            Member.GroupID = Common.Constants.Group.CorpAccount;

                    }

                }


                if (isMember)
                    return Member;
                else
                    return null;
            }
            catch
            {
                //  TODO: Log Error
                throw;
            }
            finally
            {
                sqlConn.Close();
            }

        }

        public bool KioskPing()
        {
            return true;
        }

        public bool AddBetteryMember(string FirstName,
                                    string LastName,
                                    string EmailAddress,
                                    string Password,
                                    string Zipcode,
                                    bool GetEmails,
                                    int SubscriptionPlan)
        {
            if (!CheckEmail(EmailAddress))
            {


                SqlConnection sqlConn = new SqlConnection(ConfigurationManager.ConnectionStrings["MemberDBContext"].ToString());
                try
                {
                    SqlCommand sqlCMD = new SqlCommand("AddMember", sqlConn);
                    sqlCMD.CommandType = CommandType.StoredProcedure;

                    sqlCMD.Parameters.Add(new SqlParameter("@CustomerProfileID", ""));
                    sqlCMD.Parameters.Add(new SqlParameter("@Firstname", FirstName));
                    sqlCMD.Parameters.Add(new SqlParameter("@LastName", LastName));
                    sqlCMD.Parameters.Add(new SqlParameter("@EmailAddress", EmailAddress));
                    sqlCMD.Parameters.Add(new SqlParameter("@Zipcode", Zipcode));
                    sqlCMD.Parameters.Add(new SqlParameter("@Password", Password));
                    sqlCMD.Parameters.Add(new SqlParameter("@EmailOptIn", GetEmails));
                    sqlCMD.Parameters.Add(new SqlParameter("@BatteryPacksInPlan", SubscriptionPlan));

                    sqlConn.Open();
                    sqlCMD.ExecuteNonQuery();

                    return true;

                }
                catch
                {
                    //  TODO: Log Error
                    throw;
                }
                finally
                {
                    sqlConn.Close();
                }

            }
            else
                return false;

        }
        public void UpdateMembershipProfile(int MemberID, string CustomerProfileID, string PaymentProfileID)
        {
            SqlConnection sqlConn = new SqlConnection(ConfigurationManager.ConnectionStrings["MemberDBContext"].ToString());
            try
            {
                SqlCommand sqlCMD = new SqlCommand("UpdateMembershipProfileID", sqlConn);
                sqlCMD.CommandType = CommandType.StoredProcedure;
                sqlCMD.Parameters.Add(new SqlParameter("@MemberID", MemberID));
                sqlCMD.Parameters.Add(new SqlParameter("@CustomerProfileID", CustomerProfileID));
                sqlCMD.Parameters.Add(new SqlParameter("@PaymentProfileID", PaymentProfileID));

                sqlConn.Open();
                sqlCMD.ExecuteNonQuery();

            }
            catch
            {
                //  TODO: Log Error
                throw;
            }
            finally
            {
                sqlConn.Close();
            }

        }
        public void UpdateMembershipProfile(int MemberID, string CustomerProfileID, string PaymentProfileID, int BatteriesInPlan, decimal NewAccountBalance, bool IsUpgrade)
        {
            SqlConnection sqlConn = new SqlConnection(ConfigurationManager.ConnectionStrings["MemberDBContext"].ToString());
            try
            {
                SqlCommand sqlCMD = new SqlCommand("UpdateMembershipProfile", sqlConn);
                sqlCMD.CommandType = CommandType.StoredProcedure;
                sqlCMD.Parameters.Add(new SqlParameter("@MemberID", MemberID));
                sqlCMD.Parameters.Add(new SqlParameter("@CustomerProfileID", CustomerProfileID));
                sqlCMD.Parameters.Add(new SqlParameter("@PaymentProfileID", PaymentProfileID));
                sqlCMD.Parameters.Add(new SqlParameter("@BatteryPacksInPlan", BatteriesInPlan));
                sqlCMD.Parameters.Add(new SqlParameter("@NewAccountBalance", NewAccountBalance));
                sqlCMD.Parameters.Add(new SqlParameter("@IsUpgrade", IsUpgrade));

                sqlConn.Open();
                sqlCMD.ExecuteNonQuery();

            }
            catch
            {
                //  TODO: Log Error
                throw;
            }
            finally
            {
                sqlConn.Close();
            }


        }

        public bool CheckEmail(string EmailAddress)
        {

            SqlConnection sqlConn = new SqlConnection(ConfigurationManager.ConnectionStrings["MemberDBContext"].ToString());
            try
            {
                SqlCommand sqlCMD = new SqlCommand("CheckEmail", sqlConn);
                sqlCMD.CommandType = CommandType.StoredProcedure;

                sqlCMD.Parameters.Add(new SqlParameter("@EmailAddress", EmailAddress));

                sqlConn.Open();
                return bool.Parse(sqlCMD.ExecuteScalar().ToString());

            }
            catch
            {
                //  TODO: Log Error
                throw;
            }
            finally
            {
                sqlConn.Close();
            }

        }

        public Promo GetPromoCredit(string PromoCode)
        {

            Promo promo = new Promo();

            SqlConnection sqlConn = new SqlConnection(ConfigurationManager.ConnectionStrings["MemberDBContext"].ToString());
            try
            {
                SqlCommand sqlCMD = new SqlCommand("GetPromoCredit", sqlConn);
                sqlCMD.CommandType = CommandType.StoredProcedure;

                sqlCMD.Parameters.Add(new SqlParameter("@PromoCode", PromoCode));

                sqlConn.Open();
                SqlDataReader reader = sqlCMD.ExecuteReader();
                promo.Amount = 0m;
                promo.PromoType = 0;


                while (reader.Read())
                {
                    promo.PromoType = int.Parse(reader["PromoCodeType"].ToString());
                    if (Convert.IsDBNull(reader["Amount"]))
                    {
                        promo.Amount = 0m;
                    }
                    else
                    {
                        promo.Amount = (decimal)reader["Amount"];
                    }

                }
                sqlConn.Close();

                return promo;


            }
            catch
            {
                //  TODO: Log Error
                throw;
            }
            finally
            {
                sqlConn.Close();
            }


        }
        public void RedeemPromoCode(string PromoCode)
        {
            SqlConnection sqlConn = new SqlConnection(ConfigurationManager.ConnectionStrings["MemberDBContext"].ToString());
            try
            {
                SqlCommand sqlCMD = new SqlCommand("RedeemPromoCode", sqlConn);
                sqlCMD.CommandType = CommandType.StoredProcedure;

                sqlCMD.Parameters.Add(new SqlParameter("@PromoCode", PromoCode));

                sqlConn.Open();
                sqlCMD.ExecuteNonQuery();

            }
            catch
            {
                //  TODO: Log Error
                throw;
            }
            finally
            {
                sqlConn.Close();
            }


        }
        public void ReturnBinFullAlert(string KioskID)
        {
            SqlConnection sqlConn = new SqlConnection(ConfigurationManager.ConnectionStrings["MemberDBContext"].ToString());
            try
            {
                SqlCommand sqlCMD = new SqlCommand("AddAlert", sqlConn);
                sqlCMD.CommandType = CommandType.StoredProcedure;

                sqlCMD.Parameters.Add(new SqlParameter("@KioskID", KioskID));
                sqlCMD.Parameters.Add(new SqlParameter("@AlertID", Constants.AlertType.ReturnBinFull));
                sqlCMD.Parameters.Add(new SqlParameter("@ProductID", 999));
                sqlCMD.Parameters.Add(new SqlParameter("@ErrorMessage", "Return Bin Full Alert"));

                sqlConn.Open();
                sqlCMD.ExecuteNonQuery();

            }
            catch
            {
                //  Log Error
            }
            finally
            {
                sqlConn.Close();
            }

        }
        public void ProductInventoryAlert(string KioskID, int ProductID)
        {
            SqlConnection sqlConn = new SqlConnection(ConfigurationManager.ConnectionStrings["MemberDBContext"].ToString());
            try
            {
                SqlCommand sqlCMD = new SqlCommand("AddAlert", sqlConn);
                sqlCMD.CommandType = CommandType.StoredProcedure;

                sqlCMD.Parameters.Add(new SqlParameter("@KioskID", KioskID));
                sqlCMD.Parameters.Add(new SqlParameter("@AlertID", Constants.AlertType.ProductInventoryAlert));
                sqlCMD.Parameters.Add(new SqlParameter("@ProductID", ProductID));
                sqlCMD.Parameters.Add(new SqlParameter("@ErrorMessage", "Product Inventory Alert"));

                sqlConn.Open();
                sqlCMD.ExecuteNonQuery();

            }
            catch
            {
                //  Log Error
            }
            finally
            {
                sqlConn.Close();
            }

        }
        public void AuthorizeDotNetAlert(string KioskID, string ErrorMessage)
        {
            SqlConnection sqlConn = new SqlConnection(ConfigurationManager.ConnectionStrings["MemberDBContext"].ToString());
            try
            {
                SqlCommand sqlCMD = new SqlCommand("AddAlert", sqlConn);
                sqlCMD.CommandType = CommandType.StoredProcedure;

                sqlCMD.Parameters.Add(new SqlParameter("@KioskID", KioskID));
                sqlCMD.Parameters.Add(new SqlParameter("@AlertID", Constants.AlertType.ProductInventoryAlert));
                sqlCMD.Parameters.Add(new SqlParameter("@ProductID", 999));
                sqlCMD.Parameters.Add(new SqlParameter("@ErrorMessage", ErrorMessage));

                sqlConn.Open();
                sqlCMD.ExecuteNonQuery();

            }
            catch
            {
                //  Log Error
            }
            finally
            {
                sqlConn.Close();
            }


        }

        public void TransactionFailureAlert(string KioskID, string ErrorMessage)
        {
            SqlConnection sqlConn = new SqlConnection(ConfigurationManager.ConnectionStrings["MemberDBContext"].ToString());
            try
            {
                SqlCommand sqlCMD = new SqlCommand("AddAlert", sqlConn);
                sqlCMD.CommandType = CommandType.StoredProcedure;

                sqlCMD.Parameters.Add(new SqlParameter("@KioskID", KioskID));
                sqlCMD.Parameters.Add(new SqlParameter("@AlertID", Constants.AlertType.TransactionFailureAlert));
                sqlCMD.Parameters.Add(new SqlParameter("@ProductID", 999));
                sqlCMD.Parameters.Add(new SqlParameter("@ErrorMessage", ErrorMessage));

                sqlConn.Open();
                sqlCMD.ExecuteNonQuery();

            }
            catch
            {
                //  Log Error
            }
            finally
            {
                sqlConn.Close();
            }

        }

        public void EmptyCaseVend(int MemberID, int EmptyCases)
        {
            SqlConnection sqlConn = new SqlConnection(ConfigurationManager.ConnectionStrings["MemberDBContext"].ToString());

            try
            {
                SqlCommand sqlCMD = new SqlCommand("EmptyCaseVend", sqlConn);
                sqlCMD.CommandType = CommandType.StoredProcedure;

                sqlCMD.Parameters.Add(new SqlParameter("@MemberID", MemberID));
                sqlCMD.Parameters.Add(new SqlParameter("@EmptyCases", EmptyCases));

                sqlConn.Open();
                sqlCMD.ExecuteNonQuery();
                sqlConn.Close();
            }
            catch
            {
                //  Log Error
            }
            finally
            {
                sqlConn.Close();
            }


        }

    }
}

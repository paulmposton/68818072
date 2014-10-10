using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Threading;
using Microsoft.ServiceBus;
using Microsoft.ServiceBus.Messaging;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.Diagnostics;
using Microsoft.WindowsAzure.ServiceRuntime;
using Microsoft.WindowsAzure.StorageClient;
using System.Data.SqlClient;
using System.Data;
using System.Net.Mail;
using ServiceBusWorkerRole.Common;
using System.Text;

namespace Bettery.ServiceBusWorkerRole
{
    public class WorkerRole : RoleEntryPoint
    {
        // The name of your queue
        const string QueueName = "TransactionQueue";

        // QueueClient is thread-safe. Recommended that you cache 
        // rather than recreating it on every request
        QueueClient Client;
        bool IsStopped;

        public override void Run()
        {
            while (!IsStopped)
            {
                try
                {
                    // Receive the message
                    BrokeredMessage receivedMessage = null;
                    receivedMessage = Client.Receive();

                    if (receivedMessage != null)
                    {
                        // Process the message
                        //Trace.WriteLine("Processing", receivedMessage.SequenceNumber.ToString());
                        string MemberID = "";
                        try
                        {
                            //  TODO: store connection string somewhere
                            SqlConnection sqlConn = new SqlConnection("Server=tcp:f4rxi5n7af.database.windows.net,1433;Database=Bettery;User ID=BetteryAdmin@f4rxi5n7af;Password=Bi15GR8!;Trusted_Connection=False;Encrypt=True;Connection Timeout=30");
                            SqlCommand sqlCMD = new SqlCommand("AddKioskTransaction", sqlConn);
                            sqlCMD.CommandType = CommandType.StoredProcedure;


                            sqlCMD.Parameters.Add(new SqlParameter("@OrderNumber", receivedMessage.Properties["OrderNumber"]));
                            
                            sqlCMD.Parameters.Add(new SqlParameter("@MemberID", receivedMessage.Properties["MemberID"]));

                            sqlCMD.Parameters.Add(new SqlParameter("@TotalAmount", receivedMessage.Properties["TotalAmount"]));

                            if (receivedMessage.Properties["Authorization"] == null)
                                sqlCMD.Parameters.Add(new SqlParameter("@AuthCode", DBNull.Value));
                            else
                                sqlCMD.Parameters.Add(new SqlParameter("@AuthCode", receivedMessage.Properties["Authorization"]));

                            if (receivedMessage.Properties["EmailAddress"] == null)
                                sqlCMD.Parameters.Add(new SqlParameter("@EmailAddress", DBNull.Value));
                            else
                                sqlCMD.Parameters.Add(new SqlParameter("@EmailAddress", receivedMessage.Properties["EmailAddress"]));

                            if (receivedMessage.Properties["CustomerProfileID"] == null)
                                sqlCMD.Parameters.Add(new SqlParameter("@CustomerProfileID", DBNull.Value));
                            else
                                sqlCMD.Parameters.Add(new SqlParameter("@CustomerProfileID", receivedMessage.Properties["CustomerProfileID"]));
                            
                            if (receivedMessage.Properties["PaymentProfileID"] == null)
                                sqlCMD.Parameters.Add(new SqlParameter("@PaymentProfileID", DBNull.Value));
                            else
                                sqlCMD.Parameters.Add(new SqlParameter("@PaymentProfileID", receivedMessage.Properties["PaymentProfileID"]));
                            
                            sqlCMD.Parameters.Add(new SqlParameter("@AAVend", receivedMessage.Properties["AAVend"]));
                            sqlCMD.Parameters.Add(new SqlParameter("@AAAVend", receivedMessage.Properties["AAAVend"]));
                            sqlCMD.Parameters.Add(new SqlParameter("@AAReturn", receivedMessage.Properties["AAReturn"]));
                            sqlCMD.Parameters.Add(new SqlParameter("@AAAReturn", receivedMessage.Properties["AAAReturn"]));
                            if (receivedMessage.Properties["PromoCode"] != null)
                                sqlCMD.Parameters.Add(new SqlParameter("@PromoCode", receivedMessage.Properties["PromoCode"]));
                            else
                                sqlCMD.Parameters.Add(new SqlParameter("@PromoCode", ""));
                            sqlCMD.Parameters.Add(new SqlParameter("@PromoCodeAmount", receivedMessage.Properties["PromoCodeAmount"]));
                            sqlCMD.Parameters.Add(new SqlParameter("@BatteryPacksCheckedOut", receivedMessage.Properties["BatteryPacksCheckedOut"]));

                            sqlCMD.Parameters.Add(new SqlParameter("@StationID", receivedMessage.Properties["StationID"]));

                            sqlCMD.Parameters.Add(new SqlParameter("@TaxAmount", receivedMessage.Properties["TaxAmount"]));

                            sqlCMD.Parameters.Add(new SqlParameter("@SubTotalAmount", receivedMessage.Properties["SubTotalAmount"]));

                            if (receivedMessage.Properties["NameOnCard"] == null)
                                sqlCMD.Parameters.Add(new SqlParameter("@NameOnCard", DBNull.Value));
                            else
                                sqlCMD.Parameters.Add(new SqlParameter("@NameOnCard", receivedMessage.Properties["NameOnCard"]));


                            if (receivedMessage.Properties["CardInfo"] == null)
                                sqlCMD.Parameters.Add(new SqlParameter("@CardInfo", DBNull.Value));
                            else
                                sqlCMD.Parameters.Add(new SqlParameter("@CardInfo", receivedMessage.Properties["CardInfo"]));

                            if (receivedMessage.Properties["CardHash"] == null)
                                sqlCMD.Parameters.Add(new SqlParameter("@CardHash", DBNull.Value));
                            else
                                sqlCMD.Parameters.Add(new SqlParameter("@CardHash", receivedMessage.Properties["CardHash"]));

                            if (receivedMessage.Properties["TransactionID"] == null)
                                sqlCMD.Parameters.Add(new SqlParameter("@CCTransactionID", DBNull.Value));
                            else
                                sqlCMD.Parameters.Add(new SqlParameter("@CCTransactionID", receivedMessage.Properties["TransactionID"]));


                            if (receivedMessage.Properties["SwapPrice"] == null)
                                sqlCMD.Parameters.Add(new SqlParameter("@SwapPrice", DBNull.Value));
                            else
                                sqlCMD.Parameters.Add(new SqlParameter("@SwapPrice", receivedMessage.Properties["SwapPrice"]));

                            if (receivedMessage.Properties["PurchasePrice"] == null)
                                sqlCMD.Parameters.Add(new SqlParameter("@PurchasePrice", DBNull.Value));
                            else
                                sqlCMD.Parameters.Add(new SqlParameter("@PurchasePrice", receivedMessage.Properties["PurchasePrice"]));


                            sqlConn.Open();
                            MemberID = sqlCMD.ExecuteScalar().ToString();
                            sqlConn.Close();

                            // Remove message from queue    
                            receivedMessage.Complete();
                        }
                        catch (Exception e)
                        {
                            // Indicate a problem, unlock message in queue
                            receivedMessage.Abandon();
                            // TODO: Log Error
                            // Swallow Error

                        }
                        try
                        {


                            string storeName = string.Empty;
                            string storeURL = string.Empty;
                            string fullName = string.Empty ;
                            string address1 = string.Empty;
                            string address2 = string.Empty;
                            string city = string.Empty;
                            string state = string.Empty;
                            string zipcode = string.Empty;
                            string transactionID = string.Empty ;
                            string registrationID = string.Empty;
                            string editProfileLink = string.Empty;

                            SqlConnection sqlConn = new SqlConnection("Server=tcp:f4rxi5n7af.database.windows.net,1433;Database=Bettery;User ID=BetteryAdmin@f4rxi5n7af;Password=Bi15GR8!;Trusted_Connection=False;Encrypt=True;Connection Timeout=30");
                            SqlCommand sqlCMD = new SqlCommand("GetReceiptInfo", sqlConn);
                            sqlCMD.CommandType = CommandType.StoredProcedure;


                            sqlCMD.Parameters.Add(new SqlParameter("@OrderNumber", receivedMessage.Properties["OrderNumber"]));
                            sqlConn.Open();
                            MemberID = sqlCMD.ExecuteScalar().ToString();


                            SqlDataReader reader = sqlCMD.ExecuteReader();

                            while (reader.Read())
                            {

                                storeName = reader["Store"].ToString();
                                storeURL = reader["StoreURL"].ToString();
                                address1 = reader["Address1"].ToString();
                                address2 = reader["Address2"].ToString();
                                city = reader["City"].ToString();
                                state = reader["State"].ToString();
                                zipcode = reader["Zipcode"].ToString();
                                transactionID = reader["TransactionID"].ToString();
                                if (reader["FirstName"].ToString() == String.Empty)
                                    fullName = receivedMessage.Properties["EmailAddress"].ToString();
                                else
                                    fullName = reader["FirstName"] + " " + reader["LastName"];
                                transactionID = reader["TransactionID"].ToString() ;
                                if (reader["RegistrationID"] != null && reader["RegistrationID"].ToString() != String.Empty)
                                    editProfileLink = "Thanks for using BETTERY.  To receive all of the benefits of a BETTERY Member, including discounts, please <a href=\"http://www2.betteryinc.com/account/Registration/" + reader["RegistrationID"].ToString() + "\">click here</a> to register online.";

                            }

                            sqlConn.Close();

                            // TODO: get all this stuff in config files 
                            // TODO: needs to use AuthSMTP;

                            // Fetch template body from disk
                            var template = TemplateHelper.GetEmailTemplate("http://www2.betteryinc.com/content/htmlreceipt.htm");

                            TimeZoneInfo pstZone = TimeZoneInfo.FindSystemTimeZoneById("Pacific Standard Time");

                            decimal purchasePrice = (decimal)receivedMessage.Properties["PurchasePrice"];
                            decimal swapPrice = (decimal)receivedMessage.Properties["SwapPrice"];
                            //int remainingReturns = 0;
                            int aaVend = (int)receivedMessage.Properties["AAVend"];
                            int aaaVend = (int)receivedMessage.Properties["AAAVend"];
                            int aaReturn = (int)receivedMessage.Properties["AAReturn"];
                            int aaaReturn = (int)receivedMessage.Properties["AAAReturn"];
                            

                            int depositUnits = (aaVend + aaaVend) - aaReturn;

                            decimal aaVendAmount = aaVend * swapPrice;
                            decimal aaaVendAmount = aaaVend * swapPrice;
                            
                            decimal depositAmount;

                            if (depositUnits < 0)
                                depositAmount = 0;
                            else
                                depositAmount = depositUnits * (purchasePrice - swapPrice);

                            decimal subTotal = aaVendAmount + aaaVendAmount;

                            decimal promoCodeAmount = (decimal)receivedMessage.Properties["PromoCodeAmount"];
                            string promoTitle = "";
                            string promoAmount = "";

                            if (promoCodeAmount > 0)
                            {
                                promoTitle = "Promotional Discount";
                                promoAmount = "- " + String.Format("{0:c}", promoCodeAmount);
                            }

                            decimal taxAmount = (decimal)receivedMessage.Properties["TaxAmount"];

                            string taxTitle = "";
                            string tax = "";

                            if (taxAmount > 0)
                            {
                                taxTitle = "Tax";
                                tax = String.Format("{0:c}", taxAmount);
                            }

                            //  Build Item Detail HTML
                            StringBuilder itemDetail = new StringBuilder();

                            itemDetail.Append("<tr><td></td><td style='font-family:Arial'>" + aaVend.ToString() + " 4-PACKS of AA Batteries:</td><td style='font-family:Arial;text-align:Right;color:Gray'>" + String.Format("{0:c}", aaVendAmount) + "</td></tr>");
                            itemDetail.Append("<tr><td></td><td style='font-family:Arial'>" + aaaVend.ToString() + " 4-PACKS of AAA Batteries:</td><td style='font-family:Arial;text-align:Right;color:Gray'>" + String.Format("{0:c}", aaaVendAmount) + "</td></tr>");
                                
                            // Add any tokens you want to find/replace within your template file
                            var tokens = new Dictionary<string, string>() { { "##FULLNAME##", fullName }, 
                                                                            { "##CARDINFO##", receivedMessage.Properties["CardInfo"].ToString() },
                                                                            { "##TAXTITLE##", taxTitle },
                                                                            { "##TAXAMOUNT##", tax },
                                                                            { "##ITEMDETAIL##", itemDetail.ToString() },
                                                                            { "##SUBTOTAL##", String.Format("{0:c}", subTotal) },
                                                                            { "##DEPOSITS##", String.Format("{0:c}", depositAmount) },
                                                                            { "##PROMOTITLE##", promoTitle },
                                                                            { "##PROMOAMOUNT##", promoAmount },
                                                                            { "##STORENAME##", storeName },
                                                                            { "##STOREURL##", storeURL },
                                                                            { "##ADDRESS1##", address1 },
                                                                            { "##CITY##", city },
                                                                            { "##STATE##", state },
                                                                            { "##ZIPCODE##", zipcode },
                                                                            { "##RECEIPTDATE##", String.Format("{0:g}", TimeZoneInfo.ConvertTime(DateTime.Now, pstZone)) },
                                                                            { "##TRANSACTIONID##", transactionID },
                                                                            { "##EDITPROFILELINK##", editProfileLink },
                                                                            { "##TOTAL##", String.Format("{0:c}", receivedMessage.Properties["TotalAmount"]) } };


                            // Specify addresses (CC and BCC are optional)
                            var to = new MailAddress(receivedMessage.Properties["EmailAddress"].ToString());
                            var fr = new MailAddress("CustomerCare@BETTERYinc.com");
                            var bcc = new MailAddress("support@BETTERYinc.com");
                            // Send the mail
                            TemplateHelper.Send(to, fr, bcc, "Thank you for using BETTERY", tokens, template, true);



                        }
                        catch (Exception)
                        {
                            // TODO: Log Error
                            // Swallow Error
                        }
                       
                    }
                }
                catch (MessagingException e)
                {
                    if (!e.IsTransient)
                    {
                        Trace.WriteLine(e.Message);
                        throw;
                    }

                    Thread.Sleep(10000);
                }
                catch (OperationCanceledException e)
                {
                    if (!IsStopped)
                    {
                        Trace.WriteLine(e.Message);
                        throw;
                    }
                }
            }
        }

        public override bool OnStart()
        {
            // Set the maximum number of concurrent connections 
            ServicePointManager.DefaultConnectionLimit = 12;

            // Create the queue if it does not exist already
            string connectionString = CloudConfigurationManager.GetSetting("Microsoft.ServiceBus.ConnectionString");
            var namespaceManager = NamespaceManager.CreateFromConnectionString(connectionString);
            if (!namespaceManager.QueueExists(QueueName))
            {
                namespaceManager.CreateQueue(QueueName);
            }

            // Initialize the connection to Service Bus Queue
            Client = QueueClient.CreateFromConnectionString(connectionString, QueueName);
            IsStopped = false;
            return base.OnStart();
        }

        public override void OnStop()
        {
            // Close the connection to Service Bus Queue
            IsStopped = true;
            Client.Close();
            base.OnStop();
        }
    }
}

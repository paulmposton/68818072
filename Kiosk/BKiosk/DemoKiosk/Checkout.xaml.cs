using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using AuthorizeNet;
using BKiosk.HelperClasses;
using Microsoft.ServiceBus;
using Microsoft.ServiceBus.Messaging;

namespace Bettery.Kiosk
{
    /// <summary>
    /// Interaction logic for Checkout.xaml
    /// </summary>
    public partial class Checkout : Page
    {
        private int _aaReturn;
        private int _aaaReturn;
        private int _aaVend = 0;
        private int _aaaVend = 0;
        private decimal _ChargeAmount;


        public Checkout(int aaCartridge, int aaVend, int aaaVend)
        {
            try
            {

            }
            catch (Exception ex)
            {
                // TODO: Log Error message
                throw;
            }
            InitializeComponent();
            _aaReturn = aaCartridge;
            _aaaReturn = 0;
            _aaVend = aaVend;
            _aaaVend = aaaVend;
            
            ccNumber.Focus();

            //  Calculate transaction charge/credit
            _ChargeAmount = CalcCharges();
                  
        }
        private void ccSubmitButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Use the Credit Card Helper to parse cc fields
                CreditCard cc = new CreditCard(ccNumber.Text);

                // Gen order number
                string orderNumber = Guid.NewGuid().ToString();

                // Process transaction
                string Authorization = ProcessCC(cc.ccNumber, cc.ccExpDate, _ChargeAmount, orderNumber);

                // TODO:  This MUST be changed to use Auth.net CIM to identify a Member.  For now, we're just matching last 4 digits.  This MUST by changed
                string CustomerProfileID = cc.ccNumber.Substring(cc.ccNumber.Length - 4);

                Membership page = new Membership(orderNumber, _ChargeAmount, CustomerProfileID, _aaVend, _aaaVend, _aaReturn, _aaaReturn, Authorization);
                this.NavigationService.Navigate(page);

            }
            catch (Exception ex)
            {
                // TODO: Log Error message
                throw;                
            }
            
        }
        private void FAQButton_Click(object sender, RoutedEventArgs e)
        {

        }
        private void LogInButton_Click(object sender, RoutedEventArgs e)
        {

        }
        private void StartOverButton_Click(object sender, RoutedEventArgs e)
        {
            Start page = new Start();
            this.NavigationService.Navigate(page);
        }
        private void Terms_PrivacyButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            Welcome page = new Welcome();
            this.NavigationService.Navigate(page);
        }

        private string ProcessCC(string ccNumber, string ccExpDate, decimal chargeAmount, string orderNumber)
        {

            try
            {
                if (chargeAmount > 0)
                {
                    var request = new AuthorizationRequest(ccNumber, ccExpDate, chargeAmount, "Bettery Charge");

                    // TODO: Add the following once the Membership Cache is in place.
                    //request.AddCardCode("321");

                    //Customer info - this is used for Fraud Detection
                    //request.AddCustomer("id", "first", "last", "address", "state", "zip");

                    //Custom values that will be returned with the response
                    //request.AddMerchantValue("merchantValue", "value");

                    //Shipping Address
                    //request.AddShipping("id", "first", "last", "address", "state", "zip");

                    //order number
                    request.AddInvoice(orderNumber);

                    //  
                    string apiLogin = Application.Current.Properties["apiLogin"].ToString();
                    string transactionKey = Application.Current.Properties["transactionKey"].ToString();

                    var gate = new Gateway(apiLogin, transactionKey, true);

                    //step 3 - make some money
                    var response = gate.Send(request);
                    return response.AuthorizationCode;

                }
                else
                    return "";

            }
            catch (Exception ex)
            {
                // TODO: Log Error message
                throw;

            }
            
        }

        private decimal CalcCharges()
        {
            int TotalCartridges = _aaVend + _aaaVend;
            int ReturnedCartridges = _aaReturn + _aaaReturn;
            int NewCartridges = TotalCartridges - ReturnedCartridges;
            int NewBatteries = TotalCartridges * 4;
            int ReturnedBatteries = ReturnedCartridges * 4;
            int Swapped = 0;
            int New = 0;
            int Returned = 0;
            decimal swappedAmount = 0;
            decimal returnedAmount = 0;
            decimal newAmount = 0;
            decimal totalAmount = 0;

            try
            {
                // Get Battery info (prices)
                Battery battery = new Battery();

                // Calc Swapped Charges
                if (TotalCartridges > 0 && ReturnedCartridges > 0)
                {
                    if (TotalCartridges > ReturnedCartridges)
                        Swapped = ReturnedCartridges;
                    else
                        Swapped = TotalCartridges;
                    totalAmount = Swapped * battery.SwapPrice;
                    swappedAmount = Swapped * battery.SwapPrice;
                    // Display Exchanged
                    PaymentSummary.Content += Swapped.ToString() + " pack(s) of four batteries swapped @ 1.99 = $" + String.Format("{0:0.00}", swappedAmount) + "\n";
                }
                //
                // Calc additional new Cartridges charges and Refund for returned cartridges if any
                if (TotalCartridges > ReturnedCartridges)
                {
                    totalAmount = (NewCartridges * battery.NewPrice) + (ReturnedCartridges * battery.SwapPrice);
                    newAmount = NewCartridges * battery.NewPrice;
                    New = TotalCartridges - ReturnedCartridges;
                    // Display Additional
                    PaymentSummary.Content += New.ToString() + " additional pack(s) of four batteries @ 9.99 = $" + String.Format("{0:0.00}", newAmount) + "\n";
                }
                else if (ReturnedCartridges > TotalCartridges)
                {
                    totalAmount = (NewCartridges * battery.ReturnPrice) + (TotalCartridges * battery.SwapPrice);
                    returnedAmount = NewCartridges * battery.ReturnPrice;
                    Returned = ReturnedCartridges - TotalCartridges;
                    // Display Returned
                    PaymentSummary.Content += Returned.ToString() + " pack(s) of four batteries returned @ -8.00 = $" + String.Format("{0:0.00}", returnedAmount) + "\n";
                }
                // Display Total
                if (totalAmount > 0)
                    PaymentSummary.Content += "Your total = $" + String.Format("{0:0.00}", totalAmount) + "\n";
                else
                    PaymentSummary.Content += "your credit = $" + String.Format("{0:0.00}", Math.Abs(totalAmount)) + "\n";

                return totalAmount;
            }
            catch(Exception ex)
            {
                // TODO: Log Error message
                throw;
            }
            
            
        }

    }
}

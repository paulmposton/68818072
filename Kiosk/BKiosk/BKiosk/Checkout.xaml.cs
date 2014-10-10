using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using AuthorizeNet;
using Bettery.Kiosk;
using BKiosk.HelperClasses;

namespace BKiosk
{
    /// <summary>
    /// Interaction logic for Checkout.xaml
    /// </summary>
    public partial class Checkout : Page
    {
        private int _aaReturn;
        private int _aaaReturn;
        private int _aaVend;
        private int _aaaVend;
        private decimal _chargeAmount;

        private string _orderNumber;
        private string _customerProfileID;
        private string _authorization;

        private BackgroundWorker _worker = new BackgroundWorker();

        /// <summary>
        /// Initializes a new instance of the <see cref="Checkout" /> class.
        /// </summary>
        /// <param name="aaCartridge">The aa cartridge.</param>
        /// <param name="aaVend">The aa vend.</param>
        /// <param name="aaaVend">The aaa vend.</param>
        public Checkout(int aaCartridge, int aaVend, int aaaVend)
        {
            InitializeComponent();
            BaseController.CurrentPage = this;

            _aaReturn = aaCartridge;
            _aaaReturn = 0;
            _aaVend = aaVend;
            _aaaVend = aaaVend;

            _worker.WorkerSupportsCancellation = true;
            _worker.RunWorkerCompleted += PopulateCharges_RunWorkerCompleted;
            _worker.DoWork += PopulateCharges_DoWork;
        }

        /// <summary>
        /// Handles the Loaded event of the Page control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs" /> instance containing the event data.</param>
        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            BetteryBusyIndicator.IsBusy = true;
            _worker.RunWorkerAsync();
        }

        /// <summary>
        /// Handles the DoWork event of the PopulateCharges control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="DoWorkEventArgs" /> instance containing the event data.</param>
        private void PopulateCharges_DoWork(object sender, DoWorkEventArgs e)
        {
            //Calculate charges
            BetteryVend betteryVend = Calculations.CalcCharges(_aaReturn, _aaaReturn, _aaVend, _aaaVend);
            e.Result = betteryVend;
        }

        /// <summary>
        /// Handles the RunWorkerCompleted event of the PopulateCharges control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RunWorkerCompletedEventArgs" /> instance containing the event data.</param>
        private void PopulateCharges_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            BetteryVend betteryVend = e.Result as BetteryVend;

            if (betteryVend != null)
            {
                // Update view
                // Calc Swapped Charges
                if (betteryVend.TotalCartridges > 0 && betteryVend.ReturnedCartridges > 0)
                {
                    // Display Exchanged
                    PaymentSummary.Content += betteryVend.Swapped + " pack(s) of four batteries swapped @ 1.99 = $" + string.Format("{0:0.00}", betteryVend.SwappedAmount) + "\n";
                }

                // Calc additional new Cartridges charges and Refund for returned cartridges if any
                if (betteryVend.TotalCartridges > betteryVend.ReturnedCartridges)
                {
                    // Display Additional
                    PaymentSummary.Content += betteryVend.CalculatedNew + " additional pack(s) of four batteries @ 9.99 = $" + string.Format("{0:0.00}", betteryVend.CalculatedNewAmount) + "\n";
                }
                else if (betteryVend.ReturnedCartridges > betteryVend.TotalCartridges)
                {
                    // Display Returned
                    PaymentSummary.Content += betteryVend.CalculatedReturned + " pack(s) of four batteries returned @ -8.00 = -$" + string.Format("{0:0.00}", Math.Abs(betteryVend.CalculatedReturnedAmount)) + "\n";
                }

                // Display Total
                if (betteryVend.TotalAmount > 0)
                {
                    PaymentSummary.Content += "Your total = $" + string.Format("{0:0.00}", betteryVend.TotalAmount) + "\n";
                }
                else
                {
                    PaymentSummary.Content += "your credit = $" + string.Format("{0:0.00}", Math.Abs(betteryVend.TotalAmount)) + "\n";
                }
            }

            if (BaseController.IsLoggedOnUser)
            {
                Login.Content = "Logout";
            }

            BetteryBusyIndicator.IsBusy = false;
            ccNumber.Focus();
        }

        /// <summary>
        /// Handles the Click event of the ccSubmitButton control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs" /> instance containing the event data.</param>
        private void ccSubmitButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Use the Credit Card Helper to parse cc fields
                CreditCard cc = new CreditCard(ccNumber.Text);

                // Gen order number
                _orderNumber = Guid.NewGuid().ToString();
                _authorization = ProcessCC(cc.ccNumber, cc.ccExpDate, _chargeAmount, _orderNumber);

                // TODO:  This MUST be changed to use Auth.net CIM to identify a Member.  For now, we're just matching last 4 digits.  This MUST by changed
                _customerProfileID = cc.ccNumber.Substring(cc.ccNumber.Length - 4);

                EmailReceipt page = new EmailReceipt(_orderNumber, _chargeAmount, _customerProfileID, _aaVend, _aaaVend, _aaReturn, _aaaReturn, _authorization);
                this.NavigationService.Navigate(page);
            }
            catch (Exception ex)
            {
                // TODO: Log Error message
            }
        }

        /// <summary>
        /// Handles the Click event of the FAQButton control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs" /> instance containing the event data.</param>
        private void FAQButton_Click(object sender, RoutedEventArgs e)
        {
            BaseController.PreviousPage = this;
        }

        /// <summary>
        /// Handles the Click event of the LogInButton control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs" /> instance containing the event data.</param>
        private void LogInButton_Click(object sender, RoutedEventArgs e)
        {
            if (BaseController.IsLoggedOnUser)
            {
                BaseController.Logout();
            }
            else
            {
                BaseController.PreviousPage = this;
                Login page = new Login();
                this.NavigationService.Navigate(page);
            }
        }

        /// <summary>
        /// Handles the Click event of the StartOverButton control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs" /> instance containing the event data.</param>
        private void StartOverButton_Click(object sender, RoutedEventArgs e)
        {
            BaseController.PreviousPage = this;
            Start page = new Start();
            this.NavigationService.Navigate(page);
        }

        /// <summary>
        /// Handles the Click event of the Terms_PrivacyButton control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs" /> instance containing the event data.</param>
        private void Terms_PrivacyButton_Click(object sender, RoutedEventArgs e)
        {
            BaseController.PreviousPage = this;
        }

        /// <summary>
        /// Handles the Click event of the BackButton control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs" /> instance containing the event data.</param>
        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            BaseController.PreviousPage = this;
            Welcome page = new Welcome(_aaReturn, _aaaReturn);
            this.NavigationService.Navigate(page);
        }

        #region Override

        /// <summary>
        /// Invoked when an unhandled <see cref="E:System.Windows.Input.Mouse.PreviewMouseDown"/> attached routed event reaches an element in its route that is derived from this class. Implement this method to add class handling for this event.
        /// </summary>
        /// <param name="e">The <see cref="T:System.Windows.Input.MouseButtonEventArgs"/> that contains the event data. The event data reports that one or more mouse buttons were pressed.</param>
        protected override void OnPreviewMouseDown(MouseButtonEventArgs e)
        {
            base.OnPreviewMouseDown(e);
            BaseController.ResetLastActiveTime();
        }

        /// <summary>
        /// Provides class handling for the <see cref="E:System.Windows.UIElement.PreviewTouchDown"/> routed event that occurs when a touch presses this element.
        /// </summary>
        /// <param name="e">A <see cref="T:System.Windows.Input.TouchEventArgs"/> that contains the event data.</param>
        protected override void OnPreviewTouchDown(TouchEventArgs e)
        {
            base.OnPreviewTouchDown(e);
            BaseController.ResetLastActiveTime();
        }

        /// <summary>
        /// Invoked when an unhandled <see cref="E:System.Windows.Input.Mouse.PreviewMouseMove"/> attached event reaches an element in its route that is derived from this class. Implement this method to add class handling for this event.
        /// </summary>
        /// <param name="e">The <see cref="T:System.Windows.Input.MouseEventArgs"/> that contains the event data.</param>
        protected override void OnPreviewMouseMove(MouseEventArgs e)
        {
            base.OnPreviewMouseMove(e);
            BaseController.ResetLastActiveTime();
        }

        #endregion Override

        /// <summary>
        /// Processes the CC.
        /// </summary>
        /// <param name="ccNumber">The cc number.</param>
        /// <param name="ccExpDate">The cc exp date.</param>
        /// <param name="chargeAmount">The charge amount.</param>
        /// <param name="orderNumber">The order number.</param>
        /// <returns></returns>
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
    }
}
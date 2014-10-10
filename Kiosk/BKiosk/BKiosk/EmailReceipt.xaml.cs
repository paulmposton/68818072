using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Bettery.Kiosk;
using Microsoft.ServiceBus;
using Microsoft.ServiceBus.Messaging;

namespace BKiosk
{
    /// <summary>
    /// Interaction logic for EmailReceipt.xaml
    /// </summary>
    public partial class EmailReceipt : Page
    {
        private string _orderNumber = "";
        private decimal _chargeAmount;
        private string _customerProfileID = "";
        private int _aaVend;
        private int _aaaVend;
        private int _aaReturn;
        private int _aaaReturn;
        private string _authorization = "";
        private BackgroundWorker _worker = new BackgroundWorker();

        /// <summary>
        /// Initializes a new instance of the <see cref="EmailReceipt" /> class.
        /// </summary>
        /// <param name="OrderNumber">The order number.</param>
        /// <param name="ChargeAmount">The charge amount.</param>
        /// <param name="CustomerProfileID">The customer profile ID.</param>
        /// <param name="AAVend">The AA vend.</param>
        /// <param name="AAAVend">The AAA vend.</param>
        /// <param name="AAReturn">The AA return.</param>
        /// <param name="AAAReturn">The AAA return.</param>
        /// <param name="Authorization">The authorization.</param>
        public EmailReceipt(string OrderNumber, decimal ChargeAmount, string CustomerProfileID, int AAVend, int AAAVend, int AAReturn, int AAAReturn, string Authorization)
        {
            _orderNumber = OrderNumber;
            _chargeAmount = ChargeAmount;
            _customerProfileID = CustomerProfileID;
            _aaVend = AAVend;
            _aaaVend = AAAVend;
            _aaReturn = AAReturn;
            _aaaReturn = AAAReturn;
            _authorization = Authorization;

            InitializeComponent();
            BaseController.CurrentPage = this;

            _worker.WorkerSupportsCancellation = true;
            _worker.RunWorkerCompleted += TransactionQueueSend_RunWorkerCompleted;
            _worker.DoWork += TransactionQueueSend_DoWork;
        }

        /// <summary>
        /// Handles the Loaded event of the Page control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs" /> instance containing the event data.</param>
        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            if (BaseController.IsLoggedOnUser)
            {
                Login.Content = "Logout";
            }
        }

        /// <summary>
        /// Handles the Click event of the DoneButton control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs" /> instance containing the event data.</param>
        private void DoneButton_Click(object sender, RoutedEventArgs e)
        {
            BetteryBusyIndicator.IsBusy = true;
            _worker.RunWorkerAsync();
        }

        /// <summary>
        /// Handles the DoWork event of the TransactionQueueSend control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="DoWorkEventArgs" /> instance containing the event data.</param>
        private void TransactionQueueSend_DoWork(object sender, DoWorkEventArgs e)
        {
            TransactionQueueSend(_orderNumber, _chargeAmount, Email.Text, _customerProfileID, _aaVend, _aaaVend, _aaReturn, _aaaReturn, _authorization);
        }

        /// <summary>
        /// Handles the RunWorkerCompleted event of the TransactionQueueSend control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RunWorkerCompletedEventArgs" /> instance containing the event data.</param>
        private void TransactionQueueSend_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            BetteryBusyIndicator.IsBusy = false;

            VendingPage page = new VendingPage();
            this.NavigationService.Navigate(page);
        }

        /// <summary>
        /// Handles the Click event of the Keyboard control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs" /> instance containing the event data.</param>
        private void Keyboard_Click(object sender, RoutedEventArgs e)
        {
            string key = ((Control)sender).Name;
            key = key.Replace("at", "@");
            key = key.Replace("period", ".");
            key = key.Replace("zero", "0");
            key = key.Replace("one", "1");
            key = key.Replace("two", "2");
            key = key.Replace("three", "3");
            key = key.Replace("four", "4");
            key = key.Replace("five", "5");
            key = key.Replace("six", "6");
            key = key.Replace("seven", "7");
            key = key.Replace("eight", "8");
            key = key.Replace("nine", "9");
            Email.Text += key;
        }

        /// <summary>
        /// Handles the Click event of the BackspaceButton control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs" /> instance containing the event data.</param>
        private void BackspaceButton_Click(object sender, RoutedEventArgs e)
        {
            // Remove last character if there's any left
            if (Email.Text.Length > 0)
                Email.Text = Email.Text.Substring(0, Email.Text.Length - 1);
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
            VendingPage page = new VendingPage();
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
        /// Transactions the queue send.
        /// </summary>
        /// <param name="TransactionId">The transaction ID.</param>
        /// <param name="Amount">The amount.</param>
        /// <param name="EmailAddress">The email address.</param>
        /// <param name="CustomerProfileID">The customer profile ID.</param>
        /// <param name="AAVend">The AA vend.</param>
        /// <param name="AAAVend">The AAA vend.</param>
        /// <param name="AAReturn">The AA return.</param>
        /// <param name="AAAReturn">The AAA return.</param>
        /// <param name="Authorization">The authorization.</param>
        private void TransactionQueueSend(string TransactionId,
                                    decimal Amount,
                                    string EmailAddress,
                                    string CustomerProfileID,
                                    int AAVend,
                                    int AAAVend,
                                    int AAReturn,
                                    int AAAReturn,
                                    string Authorization)
        {
            // Sends a Transaction Message to the Service Bus
            string ConnectionString = Application.Current.Properties["serviceBusconnectionString"].ToString();

            // Configure Queue Settings
            QueueDescription qd = new QueueDescription("TransactionQueue");
            qd.MaxSizeInMegabytes = 5120;
            qd.DefaultMessageTimeToLive = new TimeSpan(0, 1, 0);

            // Create a new Queue with custom settings
            var namespaceManager = NamespaceManager.CreateFromConnectionString(ConnectionString);
            if (!namespaceManager.QueueExists("TransactionQueue"))
            {
                namespaceManager.CreateQueue("TransactionQueue");
            }

            // Send a message
            MessagingFactory factory = MessagingFactory.CreateFromConnectionString(ConnectionString);

            MessageSender sender = factory.CreateMessageSender("TransactionQueue");

            // Create message, passing a string message for the body
            BrokeredMessage message = new BrokeredMessage("Kiosk Transaction");

            // Set some additional custom app-specific properties
            message.Properties["OrderNumber"] = _orderNumber;
            message.Properties["Amount"] = Amount;
            message.Properties["EmailAddress"] = EmailAddress;
            message.Properties["CustomerProfileID"] = CustomerProfileID;
            message.Properties["AAVend"] = AAVend;
            message.Properties["AAAVend"] = AAAVend;
            message.Properties["AAReturn"] = AAReturn;
            message.Properties["AAAReturn"] = AAAReturn;
            message.Properties["Authorization"] = Authorization;

            // Send message to the queue
            sender.Send(message);
        }
    }
}
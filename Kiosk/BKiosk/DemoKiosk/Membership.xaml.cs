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
using Microsoft.ServiceBus;
using Microsoft.ServiceBus.Messaging;

namespace Bettery.Kiosk
{
    /// <summary>
    /// Interaction logic for Membership.xaml
    /// </summary>
    public partial class Membership : Page
    {
        private string _orderNumber = "";
        private decimal _chargeAmount = 0M;
        private string _customerProfileID = "";
        private int _aaVend = 0;
        private int _aaaVend = 0;
        private int _aaReturn = 0;
        private int _aaaReturn = 0;
        private string _authorization = "";

        public Membership()
        {
            InitializeComponent();
        }
        public Membership(string OrderNumber, decimal ChargeAmount, string CustomerProfileID, int AAVend, int AAAVend, int AAReturn, int AAAReturn, string Authorization)
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

        }
        private void DoneButton_Click(object sender, RoutedEventArgs e)
        {
            TransactionQueueSend(_orderNumber, _chargeAmount, Email.Text, _customerProfileID, _aaVend, _aaaVend, _aaReturn, _aaaReturn, _authorization);
            Start page = new Start();
            this.NavigationService.Navigate(page);
        }

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

        private void BackspaceButton_Click(object sender, RoutedEventArgs e)
        {
            // Remove last character if there's any left
            if (Email.Text.Length > 0)
                Email.Text = Email.Text.Substring(0, Email.Text.Length - 1);
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

        private void TransactionQueueSend(string TransactionID,
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

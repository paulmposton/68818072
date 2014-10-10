using System;
using System.Windows;
using System.Windows.Controls;
using Bettery.Kiosk.Common;
using Bettery.Kiosk.Controllers;

namespace Bettery.Kiosk.UserControls
{
    /// <summary>
    /// Interaction logic for Membership.xaml
    /// </summary>
    public partial class CreditSwap : UserControl
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Membership"/> class.
        /// </summary>
        public CreditSwap()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Occurs when [on additional batteries button clicked].
        /// </summary>
        public event EventHandler OnAdditionalBatteriesButtonClicked;

        /// <summary>
        /// Occurs when [on keep deposit button clicked].
        /// </summary>
        public event EventHandler OnKeepDepositButtonClicked;

        /// <summary>
        /// Occurs when [on create customer account button clicked].
        /// </summary>
        public event EventHandler OnCreateCustomerAccountButtonClicked;

        /// <summary>
        /// Occurs when [on skip button clicked].
        /// </summary>
        public event EventHandler OnSkipButtonClicked;

        /// <summary>
        /// Handles the Click event of the AdditionalBatteriesButton control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.RoutedEventArgs"/> instance containing the event data.</param>
        private void AdditionalBatteriesButton_Click(object sender, RoutedEventArgs e)
        {
            RaiseEvent(OnAdditionalBatteriesButtonClicked);
        }

        /// <summary>
        /// Handles the Click event of the KeepDepositButton control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.RoutedEventArgs"/> instance containing the event data.</param>
        private void KeepDepositButton_Click(object sender, RoutedEventArgs e)
        {
            //if (BaseController.LoggedOnUser != null)
            //{
            //    RaiseEvent(OnKeepDepositButtonClicked);
            //}
            //else
            //{
            //    RaiseEvent(OnCreateCustomerAccountButtonClicked);
            //}
        }

        /// <summary>
        /// Handles the Click event of the SkipButton control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.RoutedEventArgs"/> instance containing the event data.</param>
        private void SkipButton_Click(object sender, RoutedEventArgs e)
        {
            RaiseEvent(OnSkipButtonClicked);
        }

        /// <summary>
        /// Raises the event.
        /// </summary>
        /// <param name="handler">The handler.</param>
        private void RaiseEvent(EventHandler handler)
        {
            if (handler != null)
            {
                Application.Current.Dispatcher.BeginInvoke(new Action(() => { handler(this, EventArgs.Empty); }));
            }
        }

        /// <summary>
        /// Loads this instance.
        /// </summary>
        public void Load(int batteryPackages)
        {
            if (BaseController.LoggedOnUser != null)
            {

                //
                //  CK 1/6/13 Both messages in both branches used to be computed.   I moved the text into the XAML since it's generic now, until we re-enable account credits.
                //

                //MessageTextBlock.Text = string.Format(Constants.Messages.MemberCreditSwap, BaseController.LoggedOnUser.MemberFirstName, batteryPackages);
                //KeepDepositButton.Content = Constants.Messages.KeepDepositOnFile;
                //SkipButton.Visibility = Visibility.Collapsed;
            }
            else
            {
                //MessageTextBlock.Text = string.Format(Constants.Messages.GuestCreditSwap, batteryPackages);
                //KeepDepositButton.Content = Constants.Messages.CreateCustomerAccount;
                SkipButton.Visibility = Visibility.Visible;
            }
        }
    }
}
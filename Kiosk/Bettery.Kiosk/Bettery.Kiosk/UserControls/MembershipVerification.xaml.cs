using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Bettery.Kiosk.Common;
using Bettery.Kiosk.Controllers;

namespace Bettery.Kiosk.UserControls
{
    /// <summary>
    /// Interaction logic for MembershipVerification.xaml
    /// </summary>
    public partial class MembershipVerification : UserControl
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Checkout" /> class.
        /// </summary>
        public MembershipVerification()
        {
            InitializeComponent();

        }

        /// <summary>
        /// Occurs when [on swipe card].
        /// </summary>
        public event EventHandler<OnSwipeCardEventArgs> OnSwipeCard;

        /// <summary>
        /// Gets the card info.
        /// </summary>
        public string CardInfo
        {
            get { return ccNumber.Text; }
        }

        /// <summary>
        /// Handles the Click event of the ccSubmitButton control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs" /> instance containing the event data.</param>
        public void ccSubmitButton_Click(object sender, RoutedEventArgs e)
        {
            EventHandler<OnSwipeCardEventArgs> handler = OnSwipeCard;
            if (handler != null)
            {
                Application.Current.Dispatcher.BeginInvoke(new Action(() => { handler(this, new OnSwipeCardEventArgs(ccNumber.Text)); }));
            }
        }

        /// <summary>
        /// Shows the message.
        /// </summary>
        /// <param name="message">The message.</param>
        public void ShowMessage(string message)
        {
            Message.Text = message;
        }

        /// <summary>
        /// Clears the message.
        /// </summary>
        public void ClearMessage()
        {
            Message.Text = string.Empty;
        }

        /// <summary>
        /// Clears this instance.
        /// </summary>
        public void Clear()
        {
            ccNumber.Clear();
            ccNumber.Focus();
        }

        /// <summary>
        /// Loads this instance.
        /// </summary>
        public void Load()
        {
            ccNumber.Text = string.Empty;
            if (BaseController.RegistrationUser != null && BaseController.RegistrationUser.BatteriesInPlan != 0)
            {
                PaymentSummary.Text = string.Format(Constants.Messages.SelectedPlan, BaseController.GetSelectedSubscriptionPlanMessage());
            }
            else
            {
                PaymentSummary.Text = string.Empty;
            }

            ClearMessage();
            ccNumber.DelayedFocus();
        }

        /// <summary>
        /// Class OnSwipeCard EventArgs
        /// </summary>
        public class OnSwipeCardEventArgs : EventArgs
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="OnSwipeCardEventArgs"/> class.
            /// </summary>
            /// <param name="cardInfo">The card info.</param>
            public OnSwipeCardEventArgs(string cardInfo)
            {
                CardInfo = cardInfo;
            }

            /// <summary>
            /// Gets or sets the card info.
            /// </summary>
            /// <value>
            /// The card info.
            /// </value>
            public string CardInfo { get; set; }
        }
    }
}

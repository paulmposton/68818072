using System;
using System.Windows;
using System.Windows.Controls;
using Bettery.Kiosk.Common;
using Bettery.Kiosk.Controllers;
using System.Configuration;

namespace Bettery.Kiosk.UserControls
{
    /// <summary>
    /// Interaction logic for Checkout.xaml
    /// </summary>
    public partial class Checkout : UserControl
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Checkout" /> class.
        /// </summary>
        public Checkout()
        {
            InitializeComponent();
            CardWrapper.MagTekUserControl magTekControl = new CardWrapper.MagTekUserControl();
            magTekControl.WrapperDataRecieved += new CardWrapper.MagTekUserControl.WrapperDataRecievedDelegate(magTekControl_WrapperDataRecieved);

            Message.Text = string.Empty;
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


        void magTekControl_WrapperDataRecieved(object sender, EventArgs e)
        {
            Type myObjectType = sender.GetType();

            //
            // Bail if we are not displaying the "checkout view" right now
            // thus, all card swipes will be ignored except during checkout
            //
            if (BaseController.CurrentView != Constants.ViewName.Checkout) return;

            System.Reflection.PropertyInfo pi = sender.GetType().GetProperty("CardData");
            string CardData = (string)(pi.GetValue(sender, null));

            EventHandler<OnSwipeCardEventArgs> handler = OnSwipeCard;
            if (handler != null)
            {
                Application.Current.Dispatcher.BeginInvoke(new Action(() => { handler(this, new OnSwipeCardEventArgs(CardData)); }));
            }
        }

        /// <summary>
        /// Handles the Click event of the ccSubmitButton control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs" /> instance containing the event data.</param>
        private void ccSubmitButton_Click(object sender, RoutedEventArgs e)
        {
            EventHandler<OnSwipeCardEventArgs> handler = OnSwipeCard;

            //
            // Bail if we are not displaying the checkout view right now
            // thus, all card swipes will be ignored except during checkout
            //
            if (BaseController.CurrentView != Constants.ViewName.Checkout) return;

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
            ResetMedia();
            PaymentSummary.Text = BaseController.GetChargesSummary();
            ClearMessage();

            if (ConfigurationManager.AppSettings["TestTransaction"] == "TRUE")
                ShowMessage("TEST MODE ENABLED:  CARD WILL NOT BE CHARGED");
            if (BaseController.LoggedOnUser != null)
            {
                if (BaseController.LoggedOnUser.GroupID == Constants.Group.SuperUser || BaseController.LoggedOnUser.GroupID == Constants.Group.SwapStationAdmin)
                    ShowMessage("TEST MODE ENABLED:  CARD WILL NOT BE CHARGED");
                if (BaseController.LoggedOnUser.GroupID == Constants.Group.CompanyAccount)
                    ShowMessage("COMPANY ACCOUNT:  CARD WILL NOT BE CHARGED, USED FOR ID ONLY");
            }

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

        /// <summary>
        /// Handles the MediaEnded event of the SwipeMedia control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.RoutedEventArgs"/> instance containing the event data.</param>
        private void SwipeMedia_MediaEnded(object sender, RoutedEventArgs e)
        {
            ResetMedia();
        }

        /// <summary>
        /// Resets the vending media.
        /// </summary>
        public void ResetMedia()
        {
            SwipeMedia.Position = TimeSpan.Zero;
            SwipeMedia.LoadedBehavior = MediaState.Play;
        }
    }
}
using System;
using System.Windows;
using System.Windows.Controls;
using System.Threading;
using System.Timers;
using Bettery.Kiosk.Common;

namespace Bettery.Kiosk.UserControls
{
    /// <summary>
    /// Interaction logic for Start.xaml
    /// </summary>
    public partial class Start : UserControl
    {
        //private readonly System.Timers.Timer _timer = new System.Timers.Timer();

        /// <summary>
        /// Gets a value indicating whether this instance is message box visible.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance is message box visible; otherwise, <c>false</c>.
        /// </value>
        public bool IsMessageBoxVisible
        {
            get { return CustomMessageBox.Visibility == Visibility.Visible; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Start"/> class.
        /// </summary>
        public Start()
        {
            InitializeComponent();
            CustomMessageBox.OnYesClicked += new EventHandler(CustomMessageBox_OnYesClicked);
            CustomMessageBox.OnNoClicked += new EventHandler(CustomMessageBox_OnNoClicked);
        }

        /// <summary>
        /// Occurs when [on get started button clicked].
        /// </summary>
        public event EventHandler OnGetStartedButtonClicked;

        /// <summary>
        /// Occurs when [on custom message box yes button clicked].
        /// </summary>
        public event EventHandler OnCustomMessageBoxYesButtonClicked;

        /// <summary>
        /// Handles the OnNoClicked event of the CustomMessageBox control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        void CustomMessageBox_OnNoClicked(object sender, EventArgs e)
        {
            CustomMessageBox.Visibility = Visibility.Collapsed;
        }

        /// <summary>
        /// Handles the OnYesClicked event of the CustomMessageBox control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        void CustomMessageBox_OnYesClicked(object sender, EventArgs e)
        {
            CustomMessageBox.Visibility = Visibility.Collapsed;
            EventHandler handler = OnCustomMessageBoxYesButtonClicked;
            if (handler != null)
            {
                Application.Current.Dispatcher.BeginInvoke(new Action(() => { handler(this, EventArgs.Empty); }));
            }
        }

        /// <summary>
        /// Handles the Click event of the GetStartedButton control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.RoutedEventArgs"/> instance containing the event data.</param>
        private void GetStartedButton_Click(object sender, RoutedEventArgs e)
        {
            EventHandler handler = OnGetStartedButtonClicked;
            if (handler != null)
            {
                Application.Current.Dispatcher.BeginInvoke(new Action(() => { handler(this, EventArgs.Empty); }));
            }
        }

        /// <summary>
        /// Handles the MediaEnded event of the IntroduceMedia control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.RoutedEventArgs"/> instance containing the event data.</param>
        private void IntroduceMedia_MediaEnded(object sender, RoutedEventArgs e)
        {

            // Just reset the media and comment out video collapse below, if no promo, just comment this out
            // IntroduceMedia.Visibility = Visibility.Collapsed;
            
            ResetMedia();

            // Don't start the timer if no promo screen required, just comment this out
            //_timer.Start();
            
            // Show other media of needed, here?
        }

        /// <summary>
        /// Resets the vending media.
        /// </summary>
        public void ResetMedia()
        {
            IntroduceMedia.Position = TimeSpan.Zero;
            IntroduceMedia.LoadedBehavior = MediaState.Play;
        }

        /// <summary>
        /// Loads this instance.
        /// </summary>
        public void Load()
        {
            ResetMedia();

            //_timer.Interval = 5000; // Five Seconds default.   Bump up if complicated screen.
            //_timer.Elapsed += PromoTimer_Elapsed;
 
            CustomMessageBox.Visibility = Visibility.Collapsed;
        }

        /// <summary>
        /// Shows the message.
        /// </summary>
        /// <param name="message">The message.</param>
        public void ShowMessage(string message)
        {
            CustomMessageBox.ShowMessage(message);
        }

        /// <summary>
        /// Handles the Elapsed event of the Timer control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="ElapsedEventArgs" /> instance containing the event data.</param>
        //private void PromoTimer_Elapsed(object sender, ElapsedEventArgs e)
        //{
            
        //    UIHelper.DispatchThread(this, main =>
        //    {
        //        _timer.Stop();
        //        IntroduceMedia.Visibility = Visibility.Visible;
        //    });
        //}
    }
}
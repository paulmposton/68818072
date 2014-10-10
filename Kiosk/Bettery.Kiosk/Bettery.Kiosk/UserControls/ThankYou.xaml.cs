using System;
using System.Windows;
using System.Windows.Controls;
using Bettery.Kiosk.Controllers;
using System.Timers;
using Bettery.Kiosk.Common;

namespace Bettery.Kiosk.UserControls
{
    /// <summary>
    /// Interaction logic for ThankYou.xaml
    /// </summary>
    public partial class ThankYou : UserControl
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ThankYou"/> class.
        /// </summary>
        public ThankYou()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Occurs when [on Done button clicked].
        /// </summary>
        public event EventHandler OnDoneButtonClicked;

        /// <summary>
        /// Occurs when Timer Elapses.
        /// </summary>
        public event EventHandler OnTimeout;

        /// <summary>
        /// Handles the Click event of the DoneButton control.  (Note, no more control, this is invoked in the override handler of the main window - CK)
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs" /> instance containing the event data.</param>
        private void DoneButton_Click(object sender, RoutedEventArgs e)
        {
            if (OnDoneButtonClicked != null)
            {
                // Clean up
                VendPackMedia.Stop();
                VendPackMedia.Visibility = Visibility.Hidden;
                ThankYouMedia.Stop();
                ThankYouMedia.Visibility = Visibility.Hidden;
                ResetMedia();
                
                //
                //  If we get a user touch, go to the main selection screen
                //
                OnDoneButtonClicked.Invoke(sender, e);
            }
        }

        /// <summary>
        /// Loads (and starts timer, if needed)
        /// </summary>
        public void Load()
        {
            ResetMedia();
            VendPackMedia.Visibility = Visibility.Visible;
            ThankYouMedia.Visibility = Visibility.Hidden;
            Message.Visibility = Visibility.Visible;

            VendPackMedia.Play();
        }
        /// <summary>
        /// Handles the MediaEnded event of the IntroduceMedia control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.RoutedEventArgs"/> instance containing the event data.</param>
        private void ThankYouMedia_MediaEnded(object sender, RoutedEventArgs e)
        {
            ThankYouMedia.Stop();
            ThankYouMedia.Visibility = Visibility.Hidden;
            ResetMedia();
            //
            // Invoke the code to navigate back to the main animation screen
            // This assumes the thank-you sequence is long enough.   If not, then
            // we'll use the timer elapsed method instead
            //
            OnTimeout.Invoke(sender, e);
        }
        /// <summary>
        /// Handles the MediaEnded event of the VendPackMedia control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.RoutedEventArgs"/> instance containing the event data.</param>
        private void VendPackMedia_MediaEnded(object sender, RoutedEventArgs e)
        {
            VendPackMedia.Stop();
            VendPackMedia.Visibility = Visibility.Hidden;
            
            ResetMedia();
                        
            Message.Visibility = Visibility.Hidden;

            ThankYouMedia.Visibility = Visibility.Visible;
            ThankYouMedia.Play();
        }

        /// <summary>
        /// Resets the vending media.
        /// </summary>
        public void ResetMedia()
        {
            VendPackMedia.Position = TimeSpan.Zero;
            ThankYouMedia.Position = TimeSpan.Zero;
        }
    }
}
using System;
using System.Windows;
using System.Windows.Controls;

namespace Bettery.Kiosk.UserControls
{
    /// <summary>
    /// Interaction logic for Vending.xaml
    /// </summary>
    public partial class Vending : UserControl
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Vending"/> class.
        /// </summary>
        public Vending()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Handles the MediaEnded event of the VendingMedia control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.RoutedEventArgs"/> instance containing the event data.</param>
        private void VendPackMedia_MediaEnded(object sender, RoutedEventArgs e)
        {
            ResetMedia();
        }

        /// <summary>
        /// Loads this instance.
        /// </summary>
        public void Load()
        {
            ResetMedia();
            ClearMessage();
        }

        /// <summary>
        /// Resets the vending media.
        /// </summary>
        public void ResetMedia()
        {
            VendPackMedia.Position = TimeSpan.Zero;
            VendPackMedia.LoadedBehavior = MediaState.Play;
        }

        /// <>
        /// Clears the message.
        /// </summary>
        public void ClearMessage()
        {
            Message.Text = string.Empty;
            Message1.Visibility = Visibility.Collapsed;
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
        /// Shows the done message.
        /// </summary>
        public void ShowDoneMessage()
        {
            //Message1.Visibility = Visibility.Visible;
        }
    }
}
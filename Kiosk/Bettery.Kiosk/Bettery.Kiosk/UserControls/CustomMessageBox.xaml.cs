using System;
using System.Windows;
using System.Windows.Controls;

namespace Bettery.Kiosk.UserControls
{
    /// <summary>
    /// Interaction logic for CustomMessageBox.xaml
    /// </summary>
    public partial class CustomMessageBox : UserControl
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CountDown" /> class.
        /// </summary>
        public CustomMessageBox()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Occurs when [on yes clicked].
        /// </summary>
        public event EventHandler OnYesClicked;

        /// <summary>
        /// Occurs when [on no clicked].
        /// </summary>
        public event EventHandler OnNoClicked;

        /// <summary>
        /// Handles the Click event of the Yes control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs" /> instance containing the event data.</param>
        private void Yes_Click(object sender, RoutedEventArgs e)
        {
            if (OnYesClicked != null)
            {
                OnYesClicked.Invoke(sender, e);
            }
        }

        /// <summary>
        /// Handles the Click event of the No control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs" /> instance containing the event data.</param>
        private void No_Click(object sender, RoutedEventArgs e)
        {
            if (OnNoClicked != null)
            {
                OnNoClicked.Invoke(sender, e);
            }
        }

        /// <summary>
        /// Shows the message.
        /// </summary>
        /// <param name="message">The message.</param>
        public void ShowMessage(string message)
        {
            Message.Text = message;
            this.Visibility = Visibility.Visible;
        }
    }
}
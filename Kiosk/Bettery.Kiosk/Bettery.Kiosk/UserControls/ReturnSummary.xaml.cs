using System;
using System.Windows;
using System.Windows.Controls;
using Bettery.Kiosk.Common;
using Bettery.Kiosk.Controllers;

namespace Bettery.Kiosk.UserControls
{
    /// <summary>
    /// Interaction logic for ReturnSumary.xaml
    /// </summary>
    public partial class ReturnSummary : UserControl
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ReturnSummary" /> class.
        /// </summary>
        public ReturnSummary()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Occurs when [on no button clicked].
        /// </summary>
        public event EventHandler OnNoButtonClicked;

        /// <summary>
        /// Occurs when [on yes button clicked].
        /// </summary>
        public event EventHandler OnYesButtonClicked;

        /// <summary>
        /// Handles the Click event of the NoButton control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void NoButton_Click(object sender, RoutedEventArgs e)
        {
            RaiseEvent(OnNoButtonClicked);
        }

        /// <summary>
        /// Handles the Click event of the YesButton control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void YesButton_Click(object sender, RoutedEventArgs e)
        {
            RaiseEvent(OnYesButtonClicked);
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
        public void Load(int aaCartridge)
        {
            Message.Text = string.Format(Constants.Messages.BetteriesReturned, aaCartridge);

            NoButton.IsEnabled = GetBatteriesController.HasTotalTransactionNotZero();
        }
    }
}
using System;
using System.Windows;
using System.Windows.Controls;
using Bettery.Kiosk.Common;
using Bettery.Kiosk.Controllers;

namespace Bettery.Kiosk.UserControls
{
    /// <summary>
    /// Interaction logic for Welcome.xaml
    /// </summary>
    public partial class Selection : UserControl
    {
        public Selection()
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
        /// Occurs when [on get case button button clicked].
        /// </summary>
        public event EventHandler OnGetCaseButtonButtonClicked;

        /// <summary>
        /// Occurs when [on get case button button clicked].
        /// </summary>
        public event EventHandler OnLearnMoreButtonButtonClicked;

        /// <summary>
        /// Occurs when [on forgot drained batterie button clicked].
        /// </summary>
        public event EventHandler OnForgotDrainedBatterieButtonClicked;

        /// <summary>
        /// Handles the Click event of the GetCaseButton control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.RoutedEventArgs"/> instance containing the event data.</param>
        private void GetCaseButton_Click(object sender, RoutedEventArgs e)
        {
            RaiseEvent(OnGetCaseButtonButtonClicked);
        }

        /// <summary>
        /// Handles the Click event of the LearnMoreButton control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.RoutedEventArgs"/> instance containing the event data.</param>
        private void LearnMoreButton_Click(object sender, RoutedEventArgs e)
        {
            RaiseEvent(OnLearnMoreButtonButtonClicked);
        }

        /// <summary>
        /// Handles the Click event of the ForgotDrainedBatteriesButton control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.RoutedEventArgs"/> instance containing the event data.</param>
        private void ForgotDrainedBatteriesButton_Click(object sender, RoutedEventArgs e)
        {
            //RaiseEvent(OnForgotDrainedBatterieButtonClicked);
        }

        /// <summary>
        /// Handles the Click event of the NoButton control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.RoutedEventArgs"/> instance containing the event data.</param>
        private void NoButton_Click(object sender, RoutedEventArgs e)
        {
            RaiseEvent(OnNoButtonClicked);
        }

        /// <summary>
        /// Handles the Click event of the YesButton control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.RoutedEventArgs"/> instance containing the event data.</param>
        private void YesButton_Click(object sender, RoutedEventArgs e)
        {
            RaiseEvent(OnYesButtonClicked);
        }

        /// <summary>
        /// Raises the event.
        /// </summary>
        /// <param name="eventHandler">The event handler.</param>
        private void RaiseEvent(EventHandler eventHandler)
        {
            EventHandler handler = eventHandler;
            if (handler != null)
            {
                Application.Current.Dispatcher.BeginInvoke(new Action(() => { handler(this, EventArgs.Empty); }));
            }
        }

        /// <summary>
        /// Loads this instance.
        /// </summary>
        public void Load()
        {
            if (BaseController.SwapPrice < (decimal)2.50)
            {
                SelectionGetMessage.Text = string.Format(Constants.Messages.SelectionGetMessageSpecial, BaseController.SwapPrice);
                SelectionSwapMessage.Text = string.Format(Constants.Messages.SelectionSwapMessageSpecial, BaseController.SwapPrice);
            }
            else
            {
                SelectionGetMessage.Text = string.Format(Constants.Messages.SelectionGetMessage, BaseController.SwapPrice);
                SelectionSwapMessage.Text = string.Format(Constants.Messages.SelectionSwapMessage, BaseController.SwapPrice);
            }
            //if (BaseController.SwapPrice < (decimal)2.50)
            //{
            //    SelectionSpecialMessage.Visibility = Visibility.Visible;
            //}
            //else
            //{
            //    SelectionSpecialMessage.Visibility = Visibility.Hidden;
            //}

            if (BaseController.LoggedOnUser != null)
            {
                GetCaseButton.IsEnabled = true;
            }
            else
            {
                GetCaseButton.IsEnabled = false;
            }
        }
    }
}
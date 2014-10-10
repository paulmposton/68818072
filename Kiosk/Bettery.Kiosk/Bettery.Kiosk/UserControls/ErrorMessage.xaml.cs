using System;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Bettery.Kiosk.Common;
using Bettery.Kiosk.Controllers;

namespace Bettery.Kiosk.UserControls
{
    /// <summary>
    /// Interaction logic for Membership.xaml
    /// </summary>
    public partial class ErrorMessage : UserControl
    {
        private readonly Timer _timerCountDown = new Timer();

        /// <summary>
        /// Initializes a new instance of the <see cref="ErrorMessage"/> class.
        /// </summary>
        public ErrorMessage()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Occurs when [on done button clicked].
        /// </summary>
        public event EventHandler OnDoneButtonClicked;

        /// <summary>
        /// Handles the Loaded event of the UserControl control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.RoutedEventArgs"/> instance containing the event data.</param>
        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            _timerCountDown.Interval = 15000;
            _timerCountDown.Elapsed += TimerCountDown_Elapsed;
        }

        /// <summary>
        /// Handles the Elapsed event of the TimerCountDown control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Timers.ElapsedEventArgs"/> instance containing the event data.</param>
        private void TimerCountDown_Elapsed(object sender, ElapsedEventArgs e)
        {
            _timerCountDown.Stop();
            if (OnDoneButtonClicked != null && BaseController.CurrentView == Constants.ViewName.ErrorMessage)
            {
                OnDoneButtonClicked.Invoke(this, EventArgs.Empty);
            }
        }

        /// <summary>
        /// Handles the Click event of the DoneButton control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.RoutedEventArgs"/> instance containing the event data.</param>
        private void DoneButton_Click(object sender, RoutedEventArgs e)
        {
            _timerCountDown.Stop();
            if (OnDoneButtonClicked != null)
            {
                OnDoneButtonClicked.Invoke(this, EventArgs.Empty);
            }
        }

        /// <summary>
        /// Loads this instance.
        /// </summary>
        /// <param name="message">The message.</param>
        public void Load(string message)
        {
            MessageTextBlock.Text = message;
            _timerCountDown.Start();
        }

        /// <summary>
        /// Loads this instance.
        /// </summary>
        public void Load()
        {
            MessageTextBlock.Text = Constants.Messages.OutOfService;
            _timerCountDown.Start();
        }
    }
}

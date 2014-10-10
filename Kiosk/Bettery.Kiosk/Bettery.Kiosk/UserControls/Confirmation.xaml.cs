using System;
using System.Windows;
using System.Windows.Controls;
using Bettery.Kiosk.Common;
using Bettery.Kiosk.Controllers;
using Bettery.Kiosk.Entities;

namespace Bettery.Kiosk.UserControls
{
    /// <summary>
    /// Interaction logic for Confirmation.xaml
    /// </summary>
    public partial class Confirmation : UserControl
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Confirmation" /> class.
        /// </summary>
        public Confirmation()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Occurs when [on ok clicked].
        /// </summary>
        public event EventHandler<RoutedEventArgs> OnOkClicked;

        /// <summary>
        /// Occurs when [on cancel clicked].
        /// </summary>
        public event EventHandler<RoutedEventArgs> OnCancelClicked;

        /// <summary>
        /// Handles the Click event of the Ok control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs" /> instance containing the event data.</param>
        private void Ok_Click(object sender, RoutedEventArgs e)
        {
            if (OnOkClicked != null)
            {
                OnOkClicked.Invoke(sender, e);
            }
        }

        /// <summary>
        /// Handles the Click event of the Cancel control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs" /> instance containing the event data.</param>
        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            if (OnCancelClicked != null)
            {
                OnCancelClicked.Invoke(sender, e);
            }
        }

        /// <summary>
        /// Loads this instance.
        /// </summary>
        public void Load()
        {
            BetteryVend selectedBettery = BaseController.SelectedBettery;
            if (selectedBettery.ReturnedCartridges > 0)
            {
                Message.Text = Constants.Messages.ReturnBatteriesConfirmMessage;
            }
            else
            {
                Message.Text = Constants.Messages.GetOrForgotBatteriesConfirmMessage;
            }
        }
    }
}
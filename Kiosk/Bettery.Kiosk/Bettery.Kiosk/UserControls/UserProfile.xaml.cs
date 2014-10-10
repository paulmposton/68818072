using System;
using System.Windows;
using System.Windows.Controls;
using Bettery.Kiosk.Controllers;
using Bettery.Kiosk.Common;

namespace Bettery.Kiosk.UserControls
{
    /// <summary>
    /// Interaction logic for Welcome.xaml
    /// </summary>
    public partial class UserProfile : UserControl
    {
        public UserProfile()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Occurs when [on continue transaction clicked].
        /// </summary>
        public event EventHandler OnContinueTransactionClicked;

        /// <summary>
        /// Occurs when [on cancel transaction clicked].
        /// </summary>
        public event EventHandler OnCancelTransactionClicked;

        /// <summary>
        /// Occurs when [on change subscription plan clicked].
        /// </summary>
        public event EventHandler OnChangeSubscriptionPlanClicked;

        /// <summary>
        /// Handles the Click event of the ContinueTransaction control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.RoutedEventArgs"/> instance containing the event data.</param>
        private void ContinueTransaction_Click(object sender, RoutedEventArgs e)
        {
            if (OnContinueTransactionClicked != null)
            {
                OnContinueTransactionClicked.Invoke(sender, e);
            }
        }

        /// <summary>
        /// Handles the Click event of the ChangeSubscriptionPlan control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.RoutedEventArgs"/> instance containing the event data.</param>
        private void ChangeSubscriptionPlan_Click(object sender, RoutedEventArgs e)
        {
            if (OnChangeSubscriptionPlanClicked != null)
            {
                OnChangeSubscriptionPlanClicked.Invoke(sender, e);
            }
        }

        /// <summary>
        /// Handles the Click event of the CancelTransaction control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.RoutedEventArgs"/> instance containing the event data.</param>
        //private void CancelTransaction_Click(object sender, RoutedEventArgs e)
        //{
        // if (OnCancelTransactionClicked != null)
        //    {
        //        OnCancelTransactionClicked.Invoke(sender, e);
        //    }
        //}

        /// <summary>
        /// Loads this instance.
        /// </summary>
        public void Load()
        {
            if (BaseController.LoggedOnUser != null)
            {
                this.Title.Text = "Hello " + BaseController.LoggedOnUser.MemberFirstName;
                this.AccountCreditAmount.Text = string.Format(Constants.Messages.UserProfileAccountCreditAmount, BaseController.LoggedOnUser.OutstandingCredit);

            ///    if (BaseController.LoggedOnUser.BatteriesInPlan > 0)
            ///    {
            ///        this.SubscriptionPlan.Text = string.Format(Constants.Messages.CurrentPlan , BaseController.GetCurrentSubscriptionPlanMessage());
            ///    }
            ///    else
            ///    {
            ///        this.SubscriptionPlan.Text = Constants.Messages.NoSelectedPlan;
            ///    }

                ContinueTransaction.Visibility = Visibility.Visible;
            }
        }
    }
}
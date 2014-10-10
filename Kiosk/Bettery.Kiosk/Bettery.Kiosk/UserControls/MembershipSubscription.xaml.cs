using System;
using System.Windows;
using System.Windows.Controls;
using Bettery.Kiosk.Controllers;

namespace Bettery.Kiosk.UserControls
{
    /// <summary>
    /// Interaction logic for MembershipSubcription.xaml
    /// </summary>
    public partial class MembershipSubscription : UserControl
    {
        private int _subscriptionPlan;

        /// <summary>
        /// Initializes a new instance of the <see cref="MembershipSubscription"/> class.
        /// </summary>
        public MembershipSubscription()
        {
            InitializeComponent();

            // Load subscription plan message
            SubscriptionPlan1.Text = MembershipSubscriptionController.SubscriptionPlan1Message;
            SubscriptionPlan2.Text = MembershipSubscriptionController.SubscriptionPlan2Message;
            SubscriptionPlan3.Text = MembershipSubscriptionController.SubscriptionPlan3Message;
            SubscriptionPlan4.Text = MembershipSubscriptionController.SubscriptionPlan4Message;
            SubscriptionPlan5.Text = MembershipSubscriptionController.SubscriptionPlan5Message;
            SubscriptionPlan6.Text = MembershipSubscriptionController.SubscriptionPlan6Message;
        }

        public delegate void DoneButtonEventHandler(int batteriesInPlan);

        /// <summary>
        /// Occurs when [on done button clicked].
        /// </summary>
        public event DoneButtonEventHandler OnDoneButtonClicked;

        /// <summary>
        /// Occurs when [on cancel clicked].
        /// </summary>
        public event EventHandler OnCancelClicked;

        /// <summary>
        /// Gets the subscription plan.
        /// </summary>
        public int SubscriptionPlan
        {
            get { return _subscriptionPlan; }
        }

        /// <summary>
        /// Called when [radio button_ checked].
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="RoutedEventArgs" /> instance containing the event data.</param>
        private void OneRadioButton_Checked(object sender, RoutedEventArgs e)
        {
            _subscriptionPlan = 1;
        }

        /// <summary>
        /// Handles the Checked event of the TwoRadioButton control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs" /> instance containing the event data.</param>
        private void TwoRadioButton_Checked(object sender, RoutedEventArgs e)
        {
            _subscriptionPlan = 2;
        }

        /// <summary>
        /// Handles the Checked event of the ThreeRadioButton control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs" /> instance containing the event data.</param>
        private void ThreeRadioButton_Checked(object sender, RoutedEventArgs e)
        {
            _subscriptionPlan = 3;
        }

        /// <summary>
        /// Handles the Checked event of the FourRadioButton control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs" /> instance containing the event data.</param>
        private void FourRadioButton_Checked(object sender, RoutedEventArgs e)
        {
            _subscriptionPlan = 4;
        }

        /// <summary>
        /// Handles the Checked event of the FiveRadioButton control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs" /> instance containing the event data.</param>
        private void FiveRadioButton_Checked(object sender, RoutedEventArgs e)
        {
            _subscriptionPlan = 5;
        }

        /// <summary>
        /// Handles the Checked event of the SixRadioButton control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs" /> instance containing the event data.</param>
        private void SixRadioButton_Checked(object sender, RoutedEventArgs e)
        {
            _subscriptionPlan = 6;
        }

        /// <summary>
        /// Handles the Click event of the DoneButton control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.RoutedEventArgs"/> instance containing the event data.</param>
        private void DoneButton_Click(object sender, RoutedEventArgs e)
        {
            RaiseOnDoneButtonClickedEvent();
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
        /// Raises the on done button clicked event.
        /// </summary>
        private void RaiseOnDoneButtonClickedEvent()
        {
            if (OnDoneButtonClicked != null)
            {
                OnDoneButtonClicked.Invoke(_subscriptionPlan);
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
        /// Loads the specified subscription plan id.
        /// </summary>
        /// <param name="subscriptionPlanId">The subscription plan id.</param>
        public void Load(int subscriptionPlanId)
        {
            _subscriptionPlan = subscriptionPlanId;
            switch (subscriptionPlanId)
            {
                case 1:
                    {
                        OneRadioButton.IsChecked = true;
                        break;
                    }
                case 2:
                    {
                        TwoRadioButton.IsChecked = true;
                        break;
                    }
                case 3:
                    {
                        ThreeRadioButton.IsChecked = true;
                        break;
                    }
                case 4:
                    {
                        FourRadioButton.IsChecked = true;
                        break;
                    }
                case 5:
                    {
                        FiveRadioButton.IsChecked = true;
                        break;
                    }
                case 6:
                    {
                        SixRadioButton.IsChecked = true;
                        break;
                    }
                default:
                    {
                        OneRadioButton.IsChecked = false;
                        TwoRadioButton.IsChecked = false;
                        ThreeRadioButton.IsChecked = false;
                        FourRadioButton.IsChecked = false;
                        FiveRadioButton.IsChecked = false;
                        SixRadioButton.IsChecked = false;
                        break;
                    }
            }
        }
    }
}
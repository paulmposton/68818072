using System;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Bettery.Kiosk;
using BKiosk.BService;
using BKiosk.HelperClasses;

namespace BKiosk
{
    /// <summary>
    /// Interaction logic for MembershipSubscription.xaml
    /// </summary>
    public partial class MembershipSubscription : Page
    {
        public MembershipSubscription()
        {
            InitializeComponent();
            BaseController.CurrentPage = this;
        }

        /// <summary>
        /// Handles the Loaded event of the Page control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs" /> instance containing the event data.</param>
        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
        }

        /// <summary>
        /// Handles the OnEnterButtonClicked event of the MembershipKeyboard control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        protected void MembershipKeyboard_OnEnterButtonClicked(object sender, EventArgs e)
        {
            Next_Click(this, null);
        }

        /// <summary>
        /// Handles the Click event of the Back control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs" /> instance containing the event data.</param>
        private void Back_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.GoBack();
        }

        /// <summary>
        /// Handles the Click event of the Next control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs" /> instance containing the event data.</param>
        private void Next_Click(object sender, RoutedEventArgs e)
        {
            User user = BaseController.LoggedOnUser;
            user.IsEmailSubscription = true;
            MembershipVerification page = new MembershipVerification();
            this.NavigationService.Navigate(page);
        }

        /// <summary>
        /// Handles the Click event of the StartOver control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs" /> instance containing the event data.</param>
        private void StartOver_Click(object sender, RoutedEventArgs e)
        {
            BaseController.PreviousPage = this;
            Start page = new Start();
            this.NavigationService.Navigate(page);
        }

        /// <summary>
        /// Handles the Click event of the FAQ control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs" /> instance containing the event data.</param>
        private void FAQ_Click(object sender, RoutedEventArgs e)
        {
            BaseController.PreviousPage = this;
            FAQ page = new FAQ();
            this.NavigationService.Navigate(page);
        }

        /// <summary>
        /// Handles the Click event of the Terms_Privacy control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs" /> instance containing the event data.</param>
        private void Terms_Privacy_Click(object sender, RoutedEventArgs e)
        {
            BaseController.PreviousPage = this;
            Privacy page = new Privacy();
            this.NavigationService.Navigate(page);
        }

        /// <summary>
        /// Handles the Click event of the NoButton control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs" /> instance containing the event data.</param>
        private void NoButton_Click(object sender, RoutedEventArgs e)
        {
            User user = BaseController.LoggedOnUser;
            user.IsEmailSubscription = false;
            user.SubscriptionPlan = 0;

            MembershipVerification page = new MembershipVerification();
            this.NavigationService.Navigate(page);
        }

        /// <summary>
        /// Called when [radio button_ checked].
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="RoutedEventArgs" /> instance containing the event data.</param>
        private void OneRadioButton_Checked(object sender, RoutedEventArgs e)
        {
            NextButton.IsEnabled = true;
            BaseController.LoggedOnUser.SubscriptionPlan = 1;
        }

        /// <summary>
        /// Handles the Checked event of the TwoRadioButton control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs" /> instance containing the event data.</param>
        private void TwoRadioButton_Checked(object sender, RoutedEventArgs e)
        {
            NextButton.IsEnabled = true;
            BaseController.LoggedOnUser.SubscriptionPlan = 2;
        }

        /// <summary>
        /// Handles the Checked event of the ThreeRadioButton control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs" /> instance containing the event data.</param>
        private void ThreeRadioButton_Checked(object sender, RoutedEventArgs e)
        {
            NextButton.IsEnabled = true;
            BaseController.LoggedOnUser.SubscriptionPlan = 3;
        }

        /// <summary>
        /// Handles the Checked event of the FourRadioButton control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs" /> instance containing the event data.</param>
        private void FourRadioButton_Checked(object sender, RoutedEventArgs e)
        {
            NextButton.IsEnabled = true;
            BaseController.LoggedOnUser.SubscriptionPlan = 4;
        }

        /// <summary>
        /// Handles the Checked event of the FiveRadioButton control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs" /> instance containing the event data.</param>
        private void FiveRadioButton_Checked(object sender, RoutedEventArgs e)
        {
            NextButton.IsEnabled = true;
            BaseController.LoggedOnUser.SubscriptionPlan = 5;
        }

        /// <summary>
        /// Handles the Checked event of the SixRadioButton control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs" /> instance containing the event data.</param>
        private void SixRadioButton_Checked(object sender, RoutedEventArgs e)
        {
            NextButton.IsEnabled = true;
            BaseController.LoggedOnUser.SubscriptionPlan = 6;
        }

        #region Override

        /// <summary>
        /// Invoked when an unhandled <see cref="E:System.Windows.Input.Mouse.PreviewMouseDown"/> attached routed event reaches an element in its route that is derived from this class. Implement this method to add class handling for this event.
        /// </summary>
        /// <param name="e">The <see cref="T:System.Windows.Input.MouseButtonEventArgs"/> that contains the event data. The event data reports that one or more mouse buttons were pressed.</param>
        protected override void OnPreviewMouseDown(MouseButtonEventArgs e)
        {
            base.OnPreviewMouseDown(e);
            BaseController.ResetLastActiveTime();
        }

        /// <summary>
        /// Provides class handling for the <see cref="E:System.Windows.UIElement.PreviewTouchDown"/> routed event that occurs when a touch presses this element.
        /// </summary>
        /// <param name="e">A <see cref="T:System.Windows.Input.TouchEventArgs"/> that contains the event data.</param>
        protected override void OnPreviewTouchDown(TouchEventArgs e)
        {
            base.OnPreviewTouchDown(e);
            BaseController.ResetLastActiveTime();
        }

        /// <summary>
        /// Invoked when an unhandled <see cref="E:System.Windows.Input.Mouse.PreviewMouseMove"/> attached event reaches an element in its route that is derived from this class. Implement this method to add class handling for this event.
        /// </summary>
        /// <param name="e">The <see cref="T:System.Windows.Input.MouseEventArgs"/> that contains the event data.</param>
        protected override void OnPreviewMouseMove(MouseEventArgs e)
        {
            base.OnPreviewMouseMove(e);
            BaseController.ResetLastActiveTime();
        }

        #endregion Override
    }
}
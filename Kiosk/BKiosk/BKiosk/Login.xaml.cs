using System;
using System.ComponentModel;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using BKiosk.BService;
using BKiosk.HelperClasses;
using BKiosk.UserControls;

namespace Bettery.Kiosk
{
    /// <summary>
    /// Interaction logic for Login.xaml
    /// </summary>
    public partial class Login : Page
    {
        private BackgroundWorker _worker = new BackgroundWorker();
        private InputControlNames _lastFocusedInputControlName = InputControlNames.UserName;

        public enum InputControlNames
        {
            UserName = 1,
            Password = 2,
        };

        public Login()
        {
            InitializeComponent();
            BaseController.CurrentPage = this;

            _worker.WorkerSupportsCancellation = true;
            _worker.RunWorkerCompleted += Login_RunWorkerCompleted;
            _worker.DoWork += Login_DoWork;

            LoginKeyboard.OnEnterButtonClicked += LoginKeyboard_OnEnterButtonClicked;
            LoginKeyboard.OnBackButtonClicked += LoginKeyboard_OnBackButtonClicked;
            LoginKeyboard.OnTextButtonClicked += LoginKeyboard_OnTextButtonClicked;
        }

        /// <summary>
        /// Handles the OnEnterButtonClicked event of the LoginKeyboard control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected void LoginKeyboard_OnEnterButtonClicked(object sender, EventArgs e)
        {
            SigninButton_Click(this, null);
        }

        /// <summary>
        /// Handles the OnBackButtonClicked event of the LoginKeyboard control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected void LoginKeyboard_OnBackButtonClicked(object sender, EventArgs e)
        {
            int selectionStart;
            int selectionLength;
            if (_lastFocusedInputControlName.Equals(InputControlNames.UserName))
            {
                selectionStart = UserName.SelectionStart;
                selectionLength = UserName.SelectionLength;

                if (selectionLength > 0)
                {
                    UserName.Text = UserName.Text.Remove(selectionStart, selectionLength);
                    UserName.SelectionStart = selectionStart;
                }
                else if (selectionStart > 0)
                {
                    UserName.Text = UserName.Text.Remove(selectionStart - 1, 1);
                    UserName.SelectionStart = selectionStart - 1;
                }

                UserName.Focus();
            }
            else if (_lastFocusedInputControlName.Equals(InputControlNames.Password))
            {
                Password.Password = string.Empty;
                Password.Focus();
            }
        }

        /// <summary>
        /// Handles the OnTextButtonClicked event of the LoginKeyboard control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="BKiosk.UserControls.TextButtonClickedEventArgs"/> instance containing the event data.</param>
        protected void LoginKeyboard_OnTextButtonClicked(object sender, TextButtonClickedEventArgs e)
        {
            int selectionStart;
            int selectionLength;
            if (_lastFocusedInputControlName.Equals(InputControlNames.UserName))
            {
                selectionStart = UserName.SelectionStart;
                selectionLength = UserName.SelectionLength;

                UserName.Text = UserName.Text.Insert(UserName.SelectionStart, e.Character);
                UserName.SelectionStart = selectionStart;
                if (!string.IsNullOrEmpty(e.Character))
                {
                    UserName.Text = UserName.Text.Remove(UserName.SelectionStart + 1, selectionLength);
                    UserName.SelectionStart = selectionStart + 1;
                }

                UserName.Focus();
            }
            else if (_lastFocusedInputControlName.Equals(InputControlNames.Password))
            {
                Password.Password = Password.Password + e.Character;
                SetSelection(Password, Password.Password.Length, 0);
                Password.Focus();
            }
        }

        /// <summary>
        /// Handles the Loaded event of the Password control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs" /> instance containing the event data.</param>
        private void Password_Loaded(object sender, RoutedEventArgs e)
        {
            UserName.Focus();
        }

        /// <summary>
        /// Handles the DoWork event of the Login control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="DoWorkEventArgs" /> instance containing the event data.</param>
        private void Login_DoWork(object sender, DoWorkEventArgs e)
        {
        }

        /// <summary>
        /// Handles the RunWorkerCompleted event of the Login control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RunWorkerCompletedEventArgs" /> instance containing the event data.</param>
        private void Login_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            BetteryBusyIndicator.IsBusy = false;
        }

        private void FAQButton_Click(object sender, RoutedEventArgs e)
        {
            BaseController.PreviousPage = this;
        }

        private void LogInButton_Click(object sender, RoutedEventArgs e)
        {
            if (BaseController.IsLoggedOnUser)
            {
                BaseController.Logout();
            }
            else
            {
                BaseController.PreviousPage = this;
                Login page = new Login();
                this.NavigationService.Navigate(page);
            }
        }

        private void StartOverButton_Click(object sender, RoutedEventArgs e)
        {
            BaseController.PreviousPage = this;
            Start page = new Start();
            this.NavigationService.Navigate(page);
        }

        private void Terms_PrivacyButton_Click(object sender, RoutedEventArgs e)
        {
            BaseController.PreviousPage = this;
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            BaseController.PreviousPage = this;
            Start page = new Start();
            this.NavigationService.Navigate(page);
        }

        private void SigninButton_Click(object sender, RoutedEventArgs e)
        {
            BetteryBusyIndicator.IsBusy = true;

            MessageText.Text = string.Empty;

            _worker.RunWorkerAsync();

            try
            {
                KioskServiceClient BKioskService = new KioskServiceClient();
                BetteryMember betteryMember = BKioskService.AuthenticateUser(UserName.Text, Password.Password);
                // TODO: Persist Logged-in Member data for use during checkout, etc.

                BaseController.LoggedOnUser = new User(UserName.Text, Password.Password)
                {
                    BatteriesCheckedOut = betteryMember.BatteriesCheckedOut,
                    BatteriesInPlan = betteryMember.BatteriesInPlan,
                    CustomerProfileID = betteryMember.CustomerProfileID,
                    FirstName = betteryMember.MemberFirstName,
                    MemberID = betteryMember.MemberID,
                    LastName = betteryMember.MemberLastName,
                    OutstandingCredit = betteryMember.OutstandingCredit,
                };

                this.NavigationService.GoBack();
            }
            catch (Exception)
            {
                BaseController.LoggedOnUser = null;
            }
        }

        /// <summary>
        /// Handles the TextChanged event of the UserName control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="TextChangedEventArgs" /> instance containing the event data.</param>
        private void UserName_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (UserName.Text.Length >= 1 && Password.Password.Length >= 6)
            {
                SigninButton.IsEnabled = true;
            }
            else
            {
                SigninButton.IsEnabled = false;
            }
        }

        /// <summary>
        /// Handles the PasswordChanged event of the Password control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs" /> instance containing the event data.</param>
        private void Password_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (UserName.Text.Length >= 1 && Password.Password.Length >= 6)
            {
                SigninButton.IsEnabled = true;
            }
            else
            {
                SigninButton.IsEnabled = false;
            }
        }

        /// <summary>
        /// Handles the GotFocus event of the UserName control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.RoutedEventArgs"/> instance containing the event data.</param>
        private void UserName_GotFocus(object sender, RoutedEventArgs e)
        {
            _lastFocusedInputControlName = InputControlNames.UserName;
        }

        /// <summary>
        /// Handles the GotFocus event of the Password control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.RoutedEventArgs"/> instance containing the event data.</param>
        private void Password_GotFocus(object sender, RoutedEventArgs e)
        {
            _lastFocusedInputControlName = InputControlNames.Password;
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

        /// <summary>
        /// Sets the selection.
        /// </summary>
        /// <param name="passwordBox">The password box.</param>
        /// <param name="start">The start.</param>
        /// <param name="length">The length.</param>
        private void SetSelection(PasswordBox passwordBox, int start, int length)
        {
            passwordBox.GetType().GetMethod("Select", BindingFlags.Instance | BindingFlags.NonPublic).Invoke(passwordBox, new object[] { start, length });
        }
    }
}
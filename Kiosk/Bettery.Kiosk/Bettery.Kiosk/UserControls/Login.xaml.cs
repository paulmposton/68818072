using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Bettery.Kiosk.Common;
using Bettery.Kiosk.Controllers;

namespace Bettery.Kiosk.UserControls
{
    /// <summary>
    /// Interaction logic for Login.xaml
    /// </summary>
    public partial class Login : UserControl
    {
        private UIElement _currentControl;

        /// <summary>
        /// Initializes a new instance of the <see cref="Login"/> class.
        /// </summary>
        public Login()
        {
            InitializeComponent();

            LoginKeyboard.OnEnterButtonClicked += LoginKeyboard_OnEnterButtonClicked;
            LoginKeyboard.OnBackButtonClicked += LoginKeyboard_OnBackButtonClicked;
            LoginKeyboard.OnTextButtonClicked += LoginKeyboard_OnTextButtonClicked;

            LoginKeyboard.OnCapsLock += LoginKeyboard_OnCapsLock;
        }

        /// <summary>
        /// Gets or sets the on signin button clicked.
        /// </summary>
        /// <value>
        /// The on signin button clicked.
        /// </value>
        public event EventHandler OnSigninButtonClicked;

        /// <summary>
        /// Gets or sets the on signup button clicked.
        /// </summary>
        /// <value>
        /// The on signup button clicked.
        /// </value>
        public event EventHandler OnSignupButtonClicked;

        /// <summary>
        /// Occurs when [on cancel clicked].
        /// </summary>
        public event EventHandler OnCancelClicked;

        /// <summary>
        /// Gets or sets the name of the bettery user.
        /// </summary>
        /// <value>
        /// The name of the bettery user.
        /// </value>
        public string BetteryUserName { get; set; }

        /// <summary>
        /// Gets or sets the bettery password.
        /// </summary>
        /// <value>
        /// The bettery password.
        /// </value>
        public string BetteryPassword { get; set; }

        /// <summary>
        /// Handles the OnEnterButtonClicked event of the LoginKeyboard control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected void LoginKeyboard_OnEnterButtonClicked(object sender, EventArgs e)
        {
            SigninButton_Click(sender, null);
        }

        /// <summary>
        /// Handles the OnBackButtonClicked event of the LoginKeyboard control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected void LoginKeyboard_OnBackButtonClicked(object sender, EventArgs e)
        {
            UIHelper.RaiseKeyEvent(_currentControl, Key.Back);

            _currentControl.Focus();
        }

        /// <summary>
        /// Handles the OnTextButtonClicked event of the LoginKeyboard control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="KeyBoard.TextButtonClickedEventArgs" /> instance containing the event data.</param>
        protected void LoginKeyboard_OnTextButtonClicked(object sender, KeyBoard.TextButtonClickedEventArgs e)
        {
            UIHelper.SendInput(_currentControl, e.Character);
            _currentControl.Focus();
        }

        /// <summary>
        /// Logins the keyboard_ on caps lock.
        /// </summary>
        /// <param name="isCapsLock">if set to <c>true</c> [is caps lock].</param>
        private void LoginKeyboard_OnCapsLock(bool isCapsLock)
        {
            if (isCapsLock)
            {
                CapsLockWarningStackPanel.Visibility = Visibility.Visible;
            }
            else
            {
                CapsLockWarningStackPanel.Visibility = Visibility.Hidden;
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
        /// Handles the TextChanged event of the UserName control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="TextChangedEventArgs" /> instance containing the event data.</param>
        private void UserName_TextChanged(object sender, TextChangedEventArgs e)
        {
            bool isValidUserName = BaseController.ValidateEmailAddress(UserName.Text);
            bool isValidPassword = Password.Password.Length >= 6;

            if (isValidUserName && isValidPassword)
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
            bool isValidUserName = BaseController.ValidateEmailAddress(UserName.Text);
            bool isValidPassword = Password.Password.Length >= 6;

            if (isValidUserName && isValidPassword)
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
            _currentControl = UserName;
        }

        /// <summary>
        /// Handles the GotFocus event of the Password control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.RoutedEventArgs"/> instance containing the event data.</param>
        private void Password_GotFocus(object sender, RoutedEventArgs e)
        {
            _currentControl = Password;
        }

        /// <summary>
        /// Handles the Click event of the SigninButton control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.RoutedEventArgs"/> instance containing the event data.</param>
        private void SigninButton_Click(object sender, RoutedEventArgs e)
        {
            BetteryUserName = UserName.Text;
            BetteryPassword = Password.Password;

            RaiseEvent(OnSigninButtonClicked);
        }

        /// <summary>
        /// Handles the Click event of the SignupButton control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.RoutedEventArgs"/> instance containing the event data.</param>
        private void SignupButton_Click(object sender, RoutedEventArgs e)
        {
            RaiseEvent(OnSignupButtonClicked);
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
        /// Sets the authentication faith.
        /// </summary>
        public void SetAuthenticationFaith(string message)
        {
            //Show invalid message
            ErrorMessageTextBlock.Text = message;

            //Reset field
            UserName.Text = string.Empty;
            Password.Password = string.Empty;

            UserName.DelayedFocus();
        }

        /// <summary>
        /// Loads this instance.
        /// </summary>
        public void Load()
        {
            ErrorMessageTextBlock.Text = Constants.Messages.LoginWarning;
            UserName.Text = string.Empty;
            Password.Password = string.Empty;
            LoginKeyboard.ResetSettings();

            UserName.DelayedFocus();
        }

        private void LoginKeyboard_Loaded(object sender, RoutedEventArgs e)
        {

        }
    }
}
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Bettery.Kiosk.Common;
using Bettery.Kiosk.Controllers;

namespace Bettery.Kiosk.UserControls
{
    /// <summary>
    /// Interaction logic for MembershipRegistration.xaml
    /// </summary>
    public partial class MembershipRegistration : UserControl
    {
        private UIElement _currentControl;
        private bool _isValidInput = true;

        /// <summary>
        /// Initializes a new instance of the <see cref="MembershipRegistration"/> class.
        /// </summary>
        public MembershipRegistration()
        {
            InitializeComponent();
            MembershipKeyboard.OnTextButtonClicked += MembershipKeyboard_OnTextButtonClicked;
            MembershipKeyboard.OnBackButtonClicked += MembershipKeyboard_OnBackButtonClicked;
            MembershipKeyboard.OnEnterButtonClicked += MembershipKeyboard_OnEnterButtonClicked;

            MembershipKeyboard.OnCapsLock += MembershipKeyboard_OnCapsLock;
        }

        /// <summary>
        /// Occurs when [on done button clicked].
        /// </summary>
        public event EventHandler<OnSignUpEventArgs> OnDoneButtonClicked;

        /// <summary>
        /// Occurs when [on subcription plan clicked].
        /// </summary>
        public event EventHandler<OnSignUpEventArgs> OnSubcriptionPlanClicked;

        /// <summary>
        /// Occurs when [on input field changed].
        /// </summary>
        public event EventHandler<OnInputFieldChangedEventArgs> OnInputFieldChanged;

        /// <summary>
        /// Occurs when [on cancel clicked].
        /// </summary>
        public event EventHandler OnCancelClicked;

        /// <summary>
        /// Handles the OnEnterButtonClicked event of the MembershipKeyboard control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="eventArgs">The <see cref="EventArgs" /> instance containing the event data.</param>
        private void MembershipKeyboard_OnEnterButtonClicked(object sender, EventArgs eventArgs)
        {
            RaiseOnDoneButtonClickedEvent();
        }

        /// <summary>
        /// Handles the OnBackButtonClicked event of the MembershipKeyboard control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="eventArgs">The <see cref="EventArgs" /> instance containing the event data.</param>
        private void MembershipKeyboard_OnBackButtonClicked(object sender, EventArgs eventArgs)
        {
            UIHelper.RaiseKeyEvent(_currentControl, Key.Back);
            _currentControl.Focus();
        }

        /// <summary>
        /// Handles the OnTextButtonClicked event of the MembershipKeyboard control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="KeyBoard.TextButtonClickedEventArgs" /> instance containing the event data.</param>
        protected void MembershipKeyboard_OnTextButtonClicked(object sender, KeyBoard.TextButtonClickedEventArgs e)
        {
            UIHelper.SendInput(_currentControl, e.Character);
            _currentControl.Focus();
        }

        /// <summary>
        /// Memberships the keyboard_ on caps lock.
        /// </summary>
        /// <param name="isCapsLock">if set to <c>true</c> [is caps lock].</param>
        private void MembershipKeyboard_OnCapsLock(bool isCapsLock)
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
        /// Handles the TextChanged event of the FirstNameTextBox control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="TextChangedEventArgs" /> instance containing the event data.</param>
        private void FirstNameTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            _isValidInput = ValidateMembershipInputs();
            RaiseOnInputFieldChangedEvent();
        }

        /// <summary>
        /// Handles the TextChanged event of the LastNameTextBox control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="TextChangedEventArgs" /> instance containing the event data.</param>
        private void LastNameTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            _isValidInput = ValidateMembershipInputs();
            RaiseOnInputFieldChangedEvent();
        }

        /// <summary>
        /// Handles the TextChanged event of the EmailTextBox control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="TextChangedEventArgs" /> instance containing the event data.</param>
        private void EmailTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            _isValidInput = ValidateMembershipInputs();
            RaiseOnInputFieldChangedEvent();
        }

        /// <summary>
        /// Handles the TextChanged event of the ZipCodeTextBox control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="TextChangedEventArgs" /> instance containing the event data.</param>
        private void ZipCodeTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            _isValidInput = ValidateMembershipInputs();
            RaiseOnInputFieldChangedEvent();
        }

        /// <summary>
        /// Handles the PasswordChanged event of the PasswordBox control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs" /> instance containing the event data.</param>
        private void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            _isValidInput = ValidateMembershipInputs();
            RaiseOnInputFieldChangedEvent();
        }

        /// <summary>
        /// Handles the PasswordChanged event of the ConfirmPasswordBox control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs" /> instance containing the event data.</param>
        private void ConfirmPasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            _isValidInput = ValidateMembershipInputs();
            RaiseOnInputFieldChangedEvent();
        }

        /// <summary>
        /// Handles the GotFocus event of the FirstNameTextBox control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs" /> instance containing the event data.</param>
        private void FirstNameTextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            _currentControl = FirstNameTextBox;
            MembershipKeyboard.Visibility = Visibility.Visible;
        }

        /// <summary>
        /// Handles the GotFocus event of the LastNameTextBox control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs" /> instance containing the event data.</param>
        private void LastNameTextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            _currentControl = LastNameTextBox;
            MembershipKeyboard.Visibility = Visibility.Visible;
        }

        /// <summary>
        /// Handles the GotFocus event of the EmailTextBox control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs" /> instance containing the event data.</param>
        private void EmailTextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            _currentControl = EmailTextBox;
            MembershipKeyboard.Visibility = Visibility.Visible;
        }

        /// <summary>
        /// Handles the GotFocus event of the ZipCodeTextBox control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs" /> instance containing the event data.</param>
        private void ZipCodeTextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            _currentControl = ZipCodeTextBox;
            MembershipKeyboard.Visibility = Visibility.Visible;
        }

        /// <summary>
        /// Handles the GotFocus event of the PasswordBox control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs" /> instance containing the event data.</param>
        private void PasswordBox_GotFocus(object sender, RoutedEventArgs e)
        {
            _currentControl = PasswordBox;
            MembershipKeyboard.Visibility = Visibility.Visible;
        }

        /// <summary>
        /// Handles the GotFocus event of the ConfirmPasswordBox control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs" /> instance containing the event data.</param>
        private void ConfirmPasswordBox_GotFocus(object sender, RoutedEventArgs e)
        {
            _currentControl = ConfirmPasswordBox;
            MembershipKeyboard.Visibility = Visibility.Visible;
        }

        /// <summary>
        /// Handles the GotFocus event of the GetEmailCheckBox control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs" /> instance containing the event data.</param>
        private void GetEmailCheckBox_GotFocus(object sender, RoutedEventArgs e)
        {
            _currentControl = GetEmailCheckBox;
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
        /// Handles the Click event of the SubscriptionPlan control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs" /> instance containing the event data.</param>
        private void SubscriptionPlan_Click(object sender, RoutedEventArgs e)
        {
            if (OnSubcriptionPlanClicked != null)
            {
                bool getSubscriptionPlan = GetEmailCheckBox.IsChecked ?? false;
                OnSignUpEventArgs signUpEventArgs = new OnSignUpEventArgs
                                                        {
                                                            FirstName = FirstNameTextBox.Text,
                                                            LastName = LastNameTextBox.Text,
                                                            Email = EmailTextBox.Text,
                                                            ZipCode = ZipCodeTextBox.Text,
                                                            Password = PasswordBox.Password,
                                                            ConfirmPassword = ConfirmPasswordBox.Password,
                                                            GetSubscriptionPlan = getSubscriptionPlan
                                                        };
                OnSubcriptionPlanClicked.Invoke(sender, signUpEventArgs);
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
        /// Raises the on enter button clicked.
        /// </summary>
        private void RaiseOnDoneButtonClickedEvent()
        {
            EventHandler<OnSignUpEventArgs> handler = OnDoneButtonClicked;
            if (handler != null)
            {
                bool getSubscriptionPlan = GetEmailCheckBox.IsChecked ?? false;
                OnSignUpEventArgs signUpEventArgs = new OnSignUpEventArgs
                {
                    FirstName = FirstNameTextBox.Text,
                    LastName = LastNameTextBox.Text,
                    Email = EmailTextBox.Text,
                    ZipCode = ZipCodeTextBox.Text,
                    Password = PasswordBox.Password,
                    ConfirmPassword = ConfirmPasswordBox.Password,
                    GetSubscriptionPlan = getSubscriptionPlan
                };

                Application.Current.Dispatcher.BeginInvoke(new Action(() => { handler(this, signUpEventArgs); }));
            }
        }

        /// <summary>
        /// Raises the on input field changed event.
        /// </summary>
        private void RaiseOnInputFieldChangedEvent()
        {
            DoneButton.IsEnabled = _isValidInput;
            SubscriptionPlan.IsEnabled = _isValidInput;

            EventHandler<OnInputFieldChangedEventArgs> handler = OnInputFieldChanged;
            if (handler != null)
            {
                Application.Current.Dispatcher.BeginInvoke(new Action(() => { handler(this, new OnInputFieldChangedEventArgs(_isValidInput)); }));
            }
        }

        /// <summary>
        /// Shows the message.
        /// </summary>
        /// <param name="message">The message.</param>
        public void ShowMessage(string message)
        {
            ErrorMessageTextBlock.Text = message;
            ErrorMessageTextBlock.Visibility = Visibility.Visible;
        }

        /// <summary>
        /// Shows the message.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="controlName">Name of the control.</param>
        public void ShowMessage(string message, ControlNames controlName)
        {
            if (!string.IsNullOrEmpty(message))
            {
                ErrorMessageTextBlock.Text = message;
                ErrorMessageTextBlock.Visibility = Visibility.Visible;
                switch (controlName)
                {
                    case ControlNames.ConfirmPassword:
                        {
                            ConfirmPasswordBox.DelayedFocus();
                            break;
                        }
                    case ControlNames.Password:
                        {
                            PasswordBox.DelayedFocus();
                            break;
                        }
                    case ControlNames.ZipCode:
                        {
                            ZipCodeTextBox.DelayedFocus();
                            break;
                        }
                    case ControlNames.LastName:
                        {
                            LastNameTextBox.DelayedFocus();
                            break;
                        }
                    case ControlNames.FirstName:
                        {
                            FirstNameTextBox.DelayedFocus();
                            break;
                        }
                    case ControlNames.Email:
                        {
                            EmailTextBox.DelayedFocus();
                            break;
                        }
                    case ControlNames.GetEmail:
                        {
                            GetEmailCheckBox.DelayedFocus();
                            break;
                        }
                }
            }
            else
            {
                ErrorMessageTextBlock.Visibility = Visibility.Collapsed;
            }
        }

        /// <summary>
        /// Clears this instance.
        /// </summary>
        public void Clear()
        {
            ErrorMessageTextBlock.Visibility = Visibility.Collapsed;
            FirstNameTextBox.Clear();
            LastNameTextBox.Clear();
            EmailTextBox.Clear();
            PasswordBox.Clear();
            ConfirmPasswordBox.Clear();
            ZipCodeTextBox.Clear();
        }

        /// <summary>
        /// Validates the membership inputs.
        /// </summary>
        /// <returns></returns>
        public bool ValidateMembershipInputs()
        {
            bool result = true;

            bool isValidFirstName = BaseController.ValidateFirstName(FirstNameTextBox.Text);

            if (isValidFirstName)
            {
                FirstNameValidate.Visibility = Visibility.Hidden;
            }
            else
            {
                result = false;
                FirstNameValidate.Visibility = Visibility.Visible;
            }

            bool isValidLastName = BaseController.ValidateLastName(LastNameTextBox.Text);
            if (isValidLastName)
            {
                LastNameValidate.Visibility = Visibility.Hidden;
            }
            else
            {
                result = false;
                LastNameValidate.Visibility = Visibility.Visible;
            }

            bool isValidEmail = BaseController.ValidateEmailAddress(EmailTextBox.Text);
            if (isValidEmail)
            {
                EmailValidate.Visibility = Visibility.Hidden;
            }
            else
            {
                result = false;
                EmailValidate.Visibility = Visibility.Visible;
            }

            bool isValidPassword = BaseController.ValidatePassword(PasswordBox.Password);
            if (isValidPassword)
            {
                PasswordValidate.Visibility = Visibility.Hidden;
            }
            else
            {
                result = false;
                PasswordValidate.Visibility = Visibility.Visible;
            }

            bool isValidConfirm = BaseController.ValidateConfirmPassword(PasswordBox.Password, ConfirmPasswordBox.Password);
            if (isValidConfirm)
            {
                ConfirmValidate.Visibility = Visibility.Hidden;
            }
            else
            {
                result = false;
                ConfirmValidate.Visibility = Visibility.Visible;
            }

            bool isValidZipcode = BaseController.ValidateZipcode(ZipCodeTextBox.Text);
            if (isValidZipcode)
            {
                ZipcodeValidate.Visibility = Visibility.Hidden;
            }
            else
            {
                result = false;
                ZipcodeValidate.Visibility = Visibility.Visible;
            }

            return result;
        }

        /// <summary>
        /// Loads this instance.
        /// </summary>
        public void Load()
        {
            ErrorMessageTextBlock.Visibility = Visibility.Collapsed;
            FirstNameTextBox.Text = string.Empty;
            LastNameTextBox.Text = string.Empty;
            EmailTextBox.Text = string.Empty;
            PasswordBox.Password = string.Empty;
            ConfirmPasswordBox.Password = string.Empty;
            ZipCodeTextBox.Text = string.Empty;
            GetEmailCheckBox.IsChecked = true;

            _isValidInput = ValidateMembershipInputs();
            MembershipKeyboard.ResetSettings();

            FirstNameTextBox.DelayedFocus();
        }

        /// <summary>
        /// Loads this instance.
        /// </summary>
        public void Load(string firstname, string lastname, string email, string password, string confirmPassword, string zipcode, bool isGetEmail)
        {
            ErrorMessageTextBlock.Visibility = Visibility.Collapsed;
            FirstNameTextBox.Text = firstname;
            LastNameTextBox.Text = lastname;
            EmailTextBox.Text = email;
            PasswordBox.Password = password;
            ConfirmPasswordBox.Password = confirmPassword;
            ZipCodeTextBox.Text = zipcode;
            GetEmailCheckBox.IsChecked = isGetEmail;

            _isValidInput = ValidateMembershipInputs();

            DoneButton.IsEnabled = _isValidInput;
            SubscriptionPlan.IsEnabled = _isValidInput;
            MembershipKeyboard.ResetSettings();

            FirstNameTextBox.DelayedFocus();
        }

        /// <summary>
        /// Enum Control Names
        /// </summary>
        public enum ControlNames
        {
            FirstName = 0,
            LastName = 1,
            Email = 2,
            Password = 3,
            ConfirmPassword = 4,
            ZipCode = 5,
            GetEmail = 6
        }

        /// <summary>
        /// Class OnInputFieldChanged EventArgs
        /// </summary>
        public class OnInputFieldChangedEventArgs : EventArgs
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="OnInputFieldChangedEventArgs"/> class.
            /// </summary>
            /// <param name="isValid">if set to <c>true</c> [is valid].</param>
            public OnInputFieldChangedEventArgs(bool isValid)
            {
                IsValid = isValid;
            }

            /// <summary>
            /// Gets or sets a value indicating whether this instance is valid.
            /// </summary>
            /// <value>
            ///   <c>true</c> if this instance is valid; otherwise, <c>false</c>.
            /// </value>
            public bool IsValid { get; set; }
        }

        /// <summary>
        /// Class OnSignUp EventArgs
        /// </summary>
        public class OnSignUpEventArgs : EventArgs
        {
            /// <summary>
            /// Gets or sets the first name.
            /// </summary>
            /// <value>
            /// The first name.
            /// </value>
            public string FirstName { get; set; }

            /// <summary>
            /// Gets or sets the last name.
            /// </summary>
            /// <value>
            /// The last name.
            /// </value>
            public string LastName { get; set; }

            /// <summary>
            /// Gets or sets the email.
            /// </summary>
            /// <value>
            /// The email.
            /// </value>
            public string Email { get; set; }

            /// <summary>
            /// Gets or sets the password.
            /// </summary>
            /// <value>
            /// The password.
            /// </value>
            public string Password { get; set; }

            /// <summary>
            /// Gets or sets the confirm password.
            /// </summary>
            /// <value>
            /// The confirm password.
            /// </value>
            public string ConfirmPassword { get; set; }

            /// <summary>
            /// Gets or sets the zip code.
            /// </summary>
            /// <value>
            /// The zip code.
            /// </value>
            public string ZipCode { get; set; }

            /// <summary>
            /// Gets or sets a value indicating whether [get subscription plan].
            /// </summary>
            /// <value>
            ///   <c>true</c> if [get subscription plan]; otherwise, <c>false</c>.
            /// </value>
            public bool GetSubscriptionPlan { get; set; }
        }
    }
}
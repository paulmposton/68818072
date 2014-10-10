using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Bettery.Kiosk;
using BKiosk.HelperClasses;
using BKiosk.UserControls;

namespace BKiosk
{
    /// <summary>
    /// Interaction logic for MembershipRegistration.xaml
    /// </summary>
    public partial class MembershipRegistration : Page
    {
        private UIElement _currentControl;

        public MembershipRegistration()
        {
            InitializeComponent();
            BaseController.CurrentPage = this;
            MembershipKeyboard.OnTextButtonClicked += MembershipKeyboard_OnTextButtonClicked;
            MembershipKeyboard.OnBackButtonClicked += MembershipKeyboard_OnBackButtonClicked;
            MembershipKeyboard.OnEnterButtonClicked += MembershipKeyboard_OnEnterButtonClicked;
        }

        /// <summary>
        /// Handles the Loaded event of the Page control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs" /> instance containing the event data.</param>
        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            FirstNameTextBox.Focus();
        }

        /// <summary>
        /// Handles the OnEnterButtonClicked event of the MembershipKeyboard control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="eventArgs">The <see cref="EventArgs" /> instance containing the event data.</param>
        private void MembershipKeyboard_OnEnterButtonClicked(object sender, EventArgs eventArgs)
        {
            Next_Click(sender, new RoutedEventArgs());
        }

        /// <summary>
        /// Handles the OnBackButtonClicked event of the MembershipKeyboard control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="eventArgs">The <see cref="EventArgs" /> instance containing the event data.</param>
        private void MembershipKeyboard_OnBackButtonClicked(object sender, EventArgs eventArgs)
        {
            RaiseKeyEvent(_currentControl, Key.Back, Keyboard.PrimaryDevice);
            _currentControl.Focus();
        }

        /// <summary>
        /// Handles the OnTextButtonClicked event of the MembershipKeyboard control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="TextButtonClickedEventArgs" /> instance containing the event data.</param>
        protected void MembershipKeyboard_OnTextButtonClicked(object sender, TextButtonClickedEventArgs e)
        {
            SendInput(_currentControl, e.Character);

            _currentControl.Focus();
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
            if (NextButton.IsEnabled)
            {
                string firstname = FirstNameTextBox.Text;
                string lastname = LastNameTextBox.Text;
                string email = EmailTextBox.Text;
                string password = PasswordBox.Password;
                string confirmPassword = ConfirmPasswordBox.Password;
                if (password != confirmPassword)
                {
                    return;
                }

                uint zipCode;
                uint.TryParse(ZipCodeTextBox.Text, out zipCode);

                bool getEmail = GetEmailCheckBox.IsChecked ?? false;

                User user = new User(email, password)
                                {
                                    FirstName = firstname,
                                    LastName = lastname,
                                    Email = email,
                                    Zipcode = zipCode,
                                    IsEmailSubscription = getEmail
                                };

                BaseController.LoggedOnUser = user;

                BaseController.PreviousPage = this;

                if (getEmail)
                {
                    MembershipSubscription page = new MembershipSubscription();
                    this.NavigationService.Navigate(page);
                }
                else
                {
                    MembershipVerification page = new MembershipVerification();
                    this.NavigationService.Navigate(page);
                }
            }
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
            FirstNameTextBox.Clear();
            LastNameTextBox.Clear();
            EmailTextBox.Clear();
            PasswordBox.Clear();
            ConfirmPasswordBox.Clear();
            ZipCodeTextBox.Clear();
            GetEmailCheckBox.IsChecked = null;

            this.NavigationService.GoBack();
        }

        /// <summary>
        /// Handles the PasswordChanged event of the PasswordBox control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs" /> instance containing the event data.</param>
        private void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            bool inputValid = BaseController.ValidateMembershipInputs(FirstNameTextBox.Text, LastNameTextBox.Text, EmailTextBox.Text, PasswordBox.Password, ConfirmPasswordBox.Password, ZipCodeTextBox.Text);
            if (inputValid)
            {
                NextButton.IsEnabled = true;
            }
            else
            {
                NextButton.IsEnabled = false;
            }
        }

        /// <summary>
        /// Handles the PasswordChanged event of the ConfirmPasswordBox control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs" /> instance containing the event data.</param>
        private void ConfirmPasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            bool inputValid = BaseController.ValidateMembershipInputs(FirstNameTextBox.Text, LastNameTextBox.Text, EmailTextBox.Text, PasswordBox.Password, ConfirmPasswordBox.Password, ZipCodeTextBox.Text);
            if (inputValid)
            {
                NextButton.IsEnabled = true;
            }
            else
            {
                NextButton.IsEnabled = false;
            }
        }

        /// <summary>
        /// Handles the TextChanged event of the ZipCodeTextBox control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="TextChangedEventArgs" /> instance containing the event data.</param>
        private void ZipCodeTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            bool inputValid = BaseController.ValidateMembershipInputs(FirstNameTextBox.Text, LastNameTextBox.Text, EmailTextBox.Text, PasswordBox.Password, ConfirmPasswordBox.Password, ZipCodeTextBox.Text);
            if (inputValid)
            {
                NextButton.IsEnabled = true;
            }
            else
            {
                NextButton.IsEnabled = false;
            }
        }

        /// <summary>
        /// Handles the GotFocus event of the FirstNameTextBox control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs" /> instance containing the event data.</param>
        private void FirstNameTextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            bool inputValid = BaseController.ValidateMembershipInputs(FirstNameTextBox.Text, LastNameTextBox.Text, EmailTextBox.Text, PasswordBox.Password, ConfirmPasswordBox.Password, ZipCodeTextBox.Text);
            if (inputValid)
            {
                NextButton.IsEnabled = true;
            }
            else
            {
                NextButton.IsEnabled = false;
            }

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
            bool inputValid = BaseController.ValidateMembershipInputs(FirstNameTextBox.Text, LastNameTextBox.Text, EmailTextBox.Text, PasswordBox.Password, ConfirmPasswordBox.Password, ZipCodeTextBox.Text);
            if (inputValid)
            {
                NextButton.IsEnabled = true;
            }
            else
            {
                NextButton.IsEnabled = false;
            }

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
            bool inputValid = BaseController.ValidateMembershipInputs(FirstNameTextBox.Text, LastNameTextBox.Text, EmailTextBox.Text, PasswordBox.Password, ConfirmPasswordBox.Password, ZipCodeTextBox.Text);
            if (inputValid)
            {
                NextButton.IsEnabled = true;
            }
            else
            {
                NextButton.IsEnabled = false;
            }

            _currentControl = EmailTextBox;
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
        /// Handles the GotFocus event of the GetEmailCheckBox control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs" /> instance containing the event data.</param>
        private void GetEmailCheckBox_GotFocus(object sender, RoutedEventArgs e)
        {
            _currentControl = GetEmailCheckBox;
            MembershipKeyboard.Visibility = Visibility.Collapsed;
        }

        /// <summary>
        /// Sends the input.
        /// </summary>
        /// <param name="element">The element.</param>
        /// <param name="text">The text.</param>
        public void SendInput(UIElement element, string text)
        {
            InputManager inputManager = InputManager.Current;
            InputDevice inputDevice = inputManager.PrimaryKeyboardDevice;
            TextComposition composition = new TextComposition(inputManager, element, text);
            TextCompositionEventArgs args = new TextCompositionEventArgs(inputDevice, composition);
            args.RoutedEvent = PreviewTextInputEvent;
            element.RaiseEvent(args);
            args.RoutedEvent = TextInputEvent;
            element.RaiseEvent(args);
        }

        /// <summary>
        /// Raises the key event.
        /// </summary>
        /// <param name="element">The element.</param>
        /// <param name="key">The key.</param>
        /// <param name="keyboardDevice">The keyboard device.</param>
        private void RaiseKeyEvent(UIElement element, Key key, KeyboardDevice keyboardDevice)
        {
            PresentationSource presentationSource = PresentationSource.FromVisual(element);
            int timestamp = Environment.TickCount;
            KeyEventArgs args = new KeyEventArgs(keyboardDevice, presentationSource, timestamp, key);

            args.RoutedEvent = Keyboard.PreviewKeyDownEvent;
            element.RaiseEvent(args);

            args.RoutedEvent = Keyboard.KeyDownEvent;
            element.RaiseEvent(args);

            args.RoutedEvent = Keyboard.PreviewKeyUpEvent;
            element.RaiseEvent(args);

            args.RoutedEvent = Keyboard.KeyUpEvent;
            element.RaiseEvent(args);
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
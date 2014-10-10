using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Bettery.Kiosk.Common;
using Bettery.Kiosk.Controllers;

namespace Bettery.Kiosk.UserControls
{
    /// <summary>
    /// Interaction logic for EmailReceipt.xaml
    /// </summary>
    public partial class EmailReceipt : UserControl
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EmailReceipt"/> class.
        /// </summary>
        public EmailReceipt()
        {
            InitializeComponent();
            EmailReceiptKeyBoard.OnTextButtonClicked += EmailReceiptKeyBoard_OnTextButtonClicked;
            EmailReceiptKeyBoard.OnBackButtonClicked += EmailReceiptKeyBoard_OnBackButtonClicked;
            EmailReceiptKeyBoard.OnEnterButtonClicked += EmailReceiptKeyBoard_OnEnterButtonClicked;

            EmailReceiptKeyBoard.OnCapsLock += EmailReceiptKeyBoard_OnCapsLock;
        }

        public delegate void DoneButtonEventHandler(string email);

        /// <summary>
        /// Occurs when [on done button clicked].
        /// </summary>
        public event DoneButtonEventHandler OnDoneButtonClicked;

        /// <summary>
        /// Occurs when [on skip button clicked].
        /// </summary>
        public event EventHandler<RoutedEventArgs> OnSkipButtonClicked;

        /// <summary>
        /// Gets or sets the email address.
        /// </summary>
        /// <value>
        /// The email address.
        /// </value>
        public string EmailAddress { get; set; }

        /// <summary>
        /// Handles the OnEnterButtonClicked event of the MembershipKeyboard control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="eventArgs">The <see cref="EventArgs" /> instance containing the event data.</param>
        private void EmailReceiptKeyBoard_OnEnterButtonClicked(object sender, EventArgs eventArgs)
        {
            RaiseOnDoneButtonClickedEvent();
        }

        /// <summary>
        /// Handles the OnBackButtonClicked event of the MembershipKeyboard control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="eventArgs">The <see cref="EventArgs" /> instance containing the event data.</param>
        private void EmailReceiptKeyBoard_OnBackButtonClicked(object sender, EventArgs eventArgs)
        {
            UIHelper.RaiseKeyEvent(Email, Key.Back);
            Email.Focus();
        }

        /// <summary>
        /// Handles the OnTextButtonClicked event of the MembershipKeyboard control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="KeyBoard.TextButtonClickedEventArgs" /> instance containing the event data.</param>
        protected void EmailReceiptKeyBoard_OnTextButtonClicked(object sender, KeyBoard.TextButtonClickedEventArgs e)
        {
            UIHelper.SendInput(Email, e.Character);
            Email.Focus();
        }

        /// <summary>
        /// Emails the receipt key board_ on caps lock.
        /// </summary>
        /// <param name="isCapsLock">if set to <c>true</c> [is caps lock].</param>
        private void EmailReceiptKeyBoard_OnCapsLock(bool isCapsLock)
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
        /// Handles the Click event of the DoneButton control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.RoutedEventArgs"/> instance containing the event data.</param>
        private void DoneButton_Click(object sender, RoutedEventArgs e)
        {
            RaiseOnDoneButtonClickedEvent();
        }

        /// <summary>
        /// Handles the Click event of the SkipButton control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs" /> instance containing the event data.</param>
        private void SkipButton_Click(object sender, RoutedEventArgs e)
        {
            if (OnSkipButtonClicked != null)
            {
                OnSkipButtonClicked.Invoke(sender, e);
            }
        }

        /// <summary>
        /// Raises the on done button clicked event.
        /// </summary>
        private void RaiseOnDoneButtonClickedEvent()
        {
            EmailAddress = Email.Text;

            bool isValidEmail = BaseController.ValidateEmailAddress(EmailAddress);

            if (isValidEmail)
            {
                if (OnDoneButtonClicked != null)
                {
                    OnDoneButtonClicked.Invoke(EmailAddress);
                }
            }
            else
            {
                Message.Visibility = Visibility.Visible;
            }
        }

        /// <summary>
        /// Loads the specified email address.
        /// </summary>
        /// <param name="emailAddress">The email address.</param>
        public void Load(string emailAddress)
        {
            Email.Text = emailAddress;
            EmailReceiptKeyBoard.ResetSettings();
            Message.Visibility = Visibility.Hidden;
            Email.DelayedFocus();
        }
    }
}
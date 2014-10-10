using System.Windows;
using System.Windows.Controls;
using Bettery.Kiosk.Common;
using Bettery.Kiosk.Controllers;

namespace Bettery.Kiosk.UserControls
{
    /// <summary>
    /// Interaction logic for ZipCode.xaml
    /// </summary>
    public partial class ZipCode : UserControl
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ZipCode"/> class.
        /// </summary>
        public ZipCode()
        {
            InitializeComponent();
            ZipcodeNumericKeyboard.OnTextButtonClicked += ZipcodeNumericKeyboard_OnTextButtonClicked;
        }

        public delegate void DoneButtonEventHandler(string zipcode);

        /// <summary>
        /// Occurs when [on done button clicked].
        /// </summary>
        public event DoneButtonEventHandler OnDoneButtonClicked;

        /// <summary>
        /// Gets or sets the user zip code.
        /// </summary>
        /// <value>
        /// The user zip code.
        /// </value>
        public string UserZipCode { get; set; }

        /// <summary>
        /// Handles the TextChanged event of the ZipCodeTextBox control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="TextChangedEventArgs" /> instance containing the event data.</param>
        private void ZipCodeTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            UserZipCode = ZipCodeTextBox.Text;
        }

        /// <summary>
        /// Handles the OnTextButtonClicked event of the ZipcodeNumericKeyboard control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="KeyBoard.TextButtonClickedEventArgs" /> instance containing the event data.</param>
        protected void ZipcodeNumericKeyboard_OnTextButtonClicked(object sender, KeyBoard.TextButtonClickedEventArgs e)
        {
            if (e.Character == "BACKSPACE")
            {
                if (ZipCodeTextBox.Text.Length > 0)
                {
                    ZipCodeTextBox.Text = ZipCodeTextBox.Text.Substring(0, ZipCodeTextBox.Text.Length - 1);
                    ZipCodeTextBox.Select(ZipCodeTextBox.Text.Length, 0);
                }
            }
            else
            {
                UIHelper.SendInput(ZipCodeTextBox, e.Character);
            }

            ZipCodeTextBox.Focus();
        }

        /// <summary>
        /// Handles the Click event of the DoneButton control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs" /> instance containing the event data.</param>
        protected void DoneButton_Click(object sender, RoutedEventArgs e)
        {
            bool isValidZipcode = BaseController.ValidateZipcode(UserZipCode);

            if (isValidZipcode)
            {
                if (OnDoneButtonClicked != null)
                {
                    OnDoneButtonClicked.Invoke(UserZipCode);
                }
            }
            else
            {
                ShowMessage(Constants.Messages.Zipcode);
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
        /// Loads this instance.
        /// </summary>
        /// <param name="zipCode">The zip code.</param>
        public void Load(string zipCode)
        {
            ZipCodeTextBox.Text = zipCode;
            ClearMessage();
            ZipCodeTextBox.DelayedFocus();
        }
    }
}
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Bettery.Kiosk;
using BKiosk.HelperClasses;
using BKiosk.UserControls;

namespace BKiosk
{
    /// <summary>
    /// Interaction logic for ZipCode.xaml
    /// </summary>
    public partial class ZipCode : Page
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ZipCode" /> class.
        /// </summary>
        public ZipCode()
        {
            InitializeComponent();

            ZipcodeNumericKeyboard.OnTextButtonClicked += ZipcodeNumericKeyboard_OnTextButtonClicked;
            BaseController.CurrentPage = this;
        }

        /// <summary>
        /// Handles the Loaded event of the Page control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs" /> instance containing the event data.</param>
        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            ZipCodeTextBox.Focus();
        }

        /// <summary>
        /// Handles the TextChanged event of the ZipCodeTextBox control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="TextChangedEventArgs" /> instance containing the event data.</param>
        private void ZipCodeTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            bool zipcodeValid = BaseController.ValidateZipcode(ZipCodeTextBox.Text);
            if (zipcodeValid)
            {
                Next.IsEnabled = true;
            }
            else
            {
                Next.IsEnabled = false;
            }
        }

        /// <summary>
        /// Handles the OnTextButtonClicked event of the ZipcodeNumericKeyboard control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="TextButtonClickedEventArgs" /> instance containing the event data.</param>
        protected void ZipcodeNumericKeyboard_OnTextButtonClicked(object sender, TextButtonClickedEventArgs e)
        {
            if (e.Character == "Del")
            {
                ZipCodeTextBox.Text = string.Empty;
            }
            else
            {
                SendInput(ZipCodeTextBox, e.Character);
            }

            ZipCodeTextBox.Focus();
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
            uint zipcode;
            uint.TryParse(ZipCodeTextBox.Text, out zipcode);
            user.Zipcode = zipcode;

            // TODO: navigate to page
        }

        /// <summary>
        /// Sends the input.
        /// </summary>
        /// <param name="element">The element.</param>
        /// <param name="text">The text.</param>
        public static void SendInput(UIElement element, string text)
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
    }
}
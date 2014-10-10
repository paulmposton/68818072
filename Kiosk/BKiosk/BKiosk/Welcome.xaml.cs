using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Bettery.Kiosk
{
    /// <summary>
    /// Interaction logic for Page2.xaml
    /// </summary>
    public partial class Welcome : Page
    {
        private int _aaExchange = 0;
        private int _aaaExchange = 0;

        public Welcome()
        {
            InitializeComponent();
            BaseController.CurrentPage = this;
        }

        public Welcome(int aaExchange, int aaaExchange)
        {
            _aaExchange = aaExchange;
            _aaaExchange = aaaExchange;
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
            if (BaseController.IsLoggedOnUser)
            {
                Login.Content = "Logout";
            }
        }

        private void YesButton_Click(object sender, RoutedEventArgs e)
        {
            BaseController.PreviousPage = this;
            Exchange page = new Exchange(_aaExchange, _aaaExchange);
            this.NavigationService.Navigate(page);
        }

        private void NoButton_Click(object sender, RoutedEventArgs e)
        {
            BaseController.PreviousPage = this;
            GetBatteries page = new GetBatteries();
            this.NavigationService.Navigate(page);
        }

        private void FAQButton_Click(object sender, RoutedEventArgs e)
        {
            BaseController.PreviousPage = this;
            FAQ page = new FAQ();
            this.NavigationService.Navigate(page);
        }

        /// <summary>
        /// Handles the Click event of the LogInButton control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs" /> instance containing the event data.</param>
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
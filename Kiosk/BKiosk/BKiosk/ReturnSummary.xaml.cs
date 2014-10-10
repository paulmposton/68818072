using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using BKiosk;

namespace Bettery.Kiosk
{
    /// <summary>
    /// Interaction logic for ReturnSummary.xaml
    /// </summary>
    ///

    public partial class ReturnSummary : Page
    {
        private int _aaCartridge;

        public ReturnSummary()
        {
            InitializeComponent();
            BaseController.CurrentPage = this;
        }

        public ReturnSummary(int aaCartridge)
        {
            InitializeComponent();
            BaseController.CurrentPage = this;
            _aaCartridge = aaCartridge;
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            Message.Text = "You have returned " + _aaCartridge + " AA Batteries and 0 AAA Batteries";
            //aaSummary.Text = aaCartridge.ToString();

            if (BaseController.IsLoggedOnUser)
            {
                Login.Content = "Logout";
            }
        }

        private void YesButton_Click(object sender, RoutedEventArgs e)
        {
            BaseController.PreviousPage = this;
            GetBatteries page = new GetBatteries(_aaCartridge);
            this.NavigationService.Navigate(page);
        }

        private void NoButton_Click(object sender, RoutedEventArgs e)
        {
            BaseController.PreviousPage = this;
            Checkout page = new Checkout(_aaCartridge, 0, 0);
            this.NavigationService.Navigate(page);
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
            Welcome page = new Welcome();
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
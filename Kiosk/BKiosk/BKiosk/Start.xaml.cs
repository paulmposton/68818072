using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;

namespace Bettery.Kiosk
{
    /// <summary>
    /// Interaction logic for Page1.xaml
    /// </summary>
    public partial class Start : Page
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Start"/> class.
        /// </summary>
        public Start()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Handles the Click event of the GetStartedButton control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.RoutedEventArgs"/> instance containing the event data.</param>
        private void GetStartedButton_Click(object sender, RoutedEventArgs e)
        {
            BaseController.PreviousPage = this;
            Welcome page = new Welcome();
            this.NavigationService.Navigate(page);
        }

        /// <summary>
        /// Handles the MediaEnded event of the IntroduceMedia control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.RoutedEventArgs"/> instance containing the event data.</param>
        private void IntroduceMedia_MediaEnded(object sender, RoutedEventArgs e)
        {
            IntroduceMedia.Position = TimeSpan.Zero;
            IntroduceMedia.LoadedBehavior = MediaState.Play;
        }
    }
}
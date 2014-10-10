using System;
using System.Windows.Controls;

namespace Bettery.Kiosk.UserControls
{
    /// <summary>
    /// Interaction logic for LearnMorePerformance.xaml
    /// </summary>
    public partial class LearnMorePerformance : UserControl
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LearnMore" /> class.
        /// </summary>
        public LearnMorePerformance()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Occurs when [on start button clicked].
        /// </summary>
        public event EventHandler OnStartButtonClicked;

        /// <summary>
        /// Occurs when [on back button clicked].
        /// </summary>
        public event EventHandler OnBackButtonClicked;

        /// <summary>
        /// Handles the Click event of the start control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.RoutedEventArgs" /> instance containing the event data.</param>
        private void StartButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            if (OnStartButtonClicked != null)
            {
                OnStartButtonClicked.Invoke(this, e);
            }
        }

        /// <summary>
        /// Handles the Click event of the back control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.RoutedEventArgs" /> instance containing the event data.</param>
        private void BackButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            if (OnBackButtonClicked != null)
            {
                OnBackButtonClicked.Invoke(this, e);
            }
        }

        /// <summary>
        /// Loads this instance.
        /// </summary>
        public void Load()
        {
        }

    }
}
using System;
using System.Windows.Controls;

namespace Bettery.Kiosk.UserControls
{
    /// <summary>
    /// Interaction logic for LearnMore.xaml
    /// </summary>
    public partial class LearnMore : UserControl
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LearnMore" /> class.
        /// </summary>
        public LearnMore()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Occurs when [on How button clicked].
        /// </summary>
        public event EventHandler OnHowButtonClicked;
        /// <summary>
        /// Occurs when [on Savings button clicked].
        /// </summary>
        public event EventHandler OnSavingsButtonClicked;
        /// <summary>
        /// Occurs when [on Performance button clicked].
        /// </summary>
        public event EventHandler OnPerformanceButtonClicked;
        /// <summary>
        /// Occurs when [on Environment button clicked].
        /// </summary>
        public event EventHandler OnEnvironmentButtonClicked;
        /// <summary>
        /// Occurs when [on Recycle button clicked].
        /// </summary>
        public event EventHandler OnRecycleButtonClicked;
        /// <summary>
        /// Occurs when [on Back button clicked].
        /// </summary>
        public event EventHandler OnBackButtonClicked;

        /// <summary>
        /// Handles the Click event of the How control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.RoutedEventArgs" /> instance containing the event data.</param>
        private void HowButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            if (OnHowButtonClicked != null)
            {
                OnHowButtonClicked.Invoke(this, e);
            }
        }
        /// <summary>
        /// Handles the Click event of the Savings control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.RoutedEventArgs" /> instance containing the event data.</param>
        private void SavingsButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            if (OnSavingsButtonClicked != null)
            {
                OnSavingsButtonClicked.Invoke(this, e);
            }
        }

        /// <summary>
        /// Handles the Click event of the Performance control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.RoutedEventArgs" /> instance containing the event data.</param>
        private void PerformanceButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            if (OnPerformanceButtonClicked != null)
            {
                OnPerformanceButtonClicked.Invoke(this, e);
            }
        }
        /// <summary>
        /// Handles the Click event of the Environment control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.RoutedEventArgs" /> instance containing the event data.</param>
        private void EnvironmentButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            if (OnEnvironmentButtonClicked != null)
            {
                OnEnvironmentButtonClicked.Invoke(this, e);
            }
        }
        /// <summary>
        /// Handles the Click event of the Recycle control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.RoutedEventArgs" /> instance containing the event data.</param>
        private void RecycleButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            if (OnRecycleButtonClicked != null)
            {
                OnRecycleButtonClicked.Invoke(this, e);
            }
        }
        /// <summary>
        /// Handles the Click event of the Back control.
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
using System;
using System.Windows;
using System.Windows.Controls;

namespace Bettery.Kiosk.UserControls
{
    /// <summary>
    /// Interaction logic for Welcome.xaml
    /// </summary>
    public partial class Welcome : UserControl
    {
        public Welcome()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Occurs when [on get bettery batteries clicked].
        /// </summary>
        public event EventHandler OnGetBetteryBatteriesClicked;

        /// <summary>
        /// Occurs when [on learn more clicked].
        /// </summary>
        public event EventHandler OnLearnMoreClicked;

        /// <summary>
        /// Handles the Click event of the GetBetteryBatteries control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs" /> instance containing the event data.</param>
        private void GetBetteryBatteries_Click(object sender, RoutedEventArgs e)
        {
            if (OnGetBetteryBatteriesClicked != null)
            {
                OnGetBetteryBatteriesClicked.Invoke(sender, e);
            }
        }

        /// <summary>
        /// Handles the Click event of the LearnMore control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs" /> instance containing the event data.</param>
        private void LearnMore_Click(object sender, RoutedEventArgs e)
        {
            if (OnLearnMoreClicked != null)
            {
                OnLearnMoreClicked.Invoke(sender, e);
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
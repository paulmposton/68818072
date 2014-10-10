using System;
using System.Windows;
using System.Windows.Controls;

namespace Bettery.Kiosk.UserControls
{
    /// <summary>
    /// Interaction logic for Privacy.xaml
    /// </summary>
    public partial class Privacy : UserControl
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Privacy" /> class.
        /// </summary>
        public Privacy()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Occurs when [on ok click].
        /// </summary>
        public event EventHandler<RoutedEventArgs> OnOkButtonClicked;

        /// <summary>
        /// Handles the Click event of the Ok control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs" /> instance containing the event data.</param>
        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            if (OnOkButtonClicked != null)
            {
                OnOkButtonClicked.Invoke(sender, e);
            }
        }

        /// <summary>
        /// Loads this instance.
        /// </summary>
        public void Load()
        {
            PrivContent.FirstPage();
        }
    }
}
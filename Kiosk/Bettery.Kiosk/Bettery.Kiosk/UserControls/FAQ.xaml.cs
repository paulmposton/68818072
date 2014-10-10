using System;
using System.Windows.Controls;

namespace Bettery.Kiosk.UserControls
{
    /// <summary>
    /// Interaction logic for FAQ.xaml
    /// </summary>
    public partial class FAQ : UserControl
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FAQ" /> class.
        /// </summary>
        public FAQ()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Occurs when [on ok button clicked].
        /// </summary>
        public event EventHandler OnOkButtonClicked;

        /// <summary>
        /// Handles the Click event of the Ok control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.RoutedEventArgs" /> instance containing the event data.</param>
        private void OkButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            if (OnOkButtonClicked != null)
            {
                OnOkButtonClicked.Invoke(this, e);
            }
        }

        /// <summary>
        /// Loads this instance.
        /// </summary>
        public void Load()
        {
            FaqContent.FirstPage();
        }
    }
}
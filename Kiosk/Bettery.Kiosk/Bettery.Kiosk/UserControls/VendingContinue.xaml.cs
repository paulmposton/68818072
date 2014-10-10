using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Bettery.Kiosk.UserControls
{
    /// <summary>
    /// Interaction logic for VendingContinue.xaml
    /// </summary>
    public partial class VendingContinue : UserControl
    {
        public VendingContinue()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Occurs when [on Done button clicked].
        /// </summary>
        public event EventHandler OnDoneButtonClicked;


        /// <summary>
        /// Handles the Click event of the DoneButton control.  (Note, no more control, this is invoked in the override handler of the main window - CK)
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs" /> instance containing the event data.</param>
        private void DoneButton_Click(object sender, RoutedEventArgs e)
        {
            if (OnDoneButtonClicked != null)
            {
                OnDoneButtonClicked.Invoke(sender, e);
            }
        }
        public void Load()
        {
            ResetMedia();
        }

        /// <summary>
        /// Handles the MediaEnded event of the VendingMedia control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.RoutedEventArgs"/> instance containing the event data.</param>
        private void VendPackMedia_MediaEnded(object sender, RoutedEventArgs e)
        {
            ResetMedia();
        }

        /// <summary>
        /// Resets the vending media.
        /// </summary>
        public void ResetMedia()
        {
            VendPackMedia.Position = TimeSpan.Zero;
            VendPackMedia.LoadedBehavior = MediaState.Play;
        }

    }
}

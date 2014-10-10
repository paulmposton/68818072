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
    /// Interaction logic for CardChoice.xaml
    /// </summary>
    public partial class CardChoice : UserControl
    {
        public CardChoice()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Occurs when [on New Card clicked].
        /// </summary>
        public event EventHandler OnNewCard_Clicked;

        /// <summary>
        /// Occurs when [on Existing Card clicked].
        /// </summary>
        public event EventHandler OnExistingCard_Clicked;

        /// <summary>
        /// Handles the Click event of the NewCard control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs" /> instance containing the event data.</param>
        private void NewCard_Click(object sender, RoutedEventArgs e)
        {
            if (OnNewCard_Clicked != null)
            {
                
                OnNewCard_Clicked.Invoke(sender, e);
            }
        }

        /// <summary>
        /// Handles the Click event of the Existing Card control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs" /> instance containing the event data.</param>
        private void ExistingCard_Click(object sender, RoutedEventArgs e)
        {
            if (OnExistingCard_Clicked != null)
            {
                OnExistingCard_Clicked.Invoke(sender, e);
            }
        }
    }
}

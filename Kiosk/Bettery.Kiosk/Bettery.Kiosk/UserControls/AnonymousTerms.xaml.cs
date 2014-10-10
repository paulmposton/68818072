using System;
using System.Windows;
using System.Windows.Controls;

namespace Bettery.Kiosk.UserControls
{
    /// <summary>
    /// Interaction logic for Terms.xaml
    /// </summary>
    public partial class AnonymousTerms : UserControl
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AnonymousTerms" /> class.
        /// </summary>
        public AnonymousTerms()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Occurs when [on iaccept button cliked].
        /// </summary>
        public event EventHandler<RoutedEventArgs> OnIAcceptButtonCliked;

        /// <summary>
        /// Occurs when [on cancel button cliked].
        /// </summary>
        public event EventHandler<RoutedEventArgs> OnCancelButtonCliked;

        /// <summary>
        /// Handles the Click event of the IAcceptButton control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs" /> instance containing the event data.</param>
        private void IAcceptButton_Click(object sender, RoutedEventArgs e)
        {
            if (OnIAcceptButtonCliked != null)
            {
                OnIAcceptButtonCliked.Invoke(sender, e);
            }
        }

        /// <summary>
        /// Handles the Click event of the CancelButton control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs" /> instance containing the event data.</param>
        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            if (OnCancelButtonCliked != null)
            {
                OnCancelButtonCliked.Invoke(sender, e);
            }
        }

        /// <summary>
        /// Loads this instance.
        /// </summary>
        public void Load()
        {
            TermsContent.FirstPage();
        }
    }
}
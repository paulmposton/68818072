using System;
using System.Windows;
using System.Windows.Controls;
using BKiosk.HelperClasses;

namespace BKiosk.UserControls
{
    /// <summary>
    /// Interaction logic for NumericKeyBoard.xaml
    /// </summary>
    public partial class NumericKeyBoard : UserControl
    {
        public NumericKeyBoard()
        {
            InitializeComponent();
        }

        public EventHandler<TextButtonClickedEventArgs> OnTextButtonClicked;

        /// <summary>
        /// Handles the Click event of the Keyboard control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.RoutedEventArgs"/> instance containing the event data.</param>
        private void Keyboard_Click(object sender, RoutedEventArgs e)
        {
            string character = Utils.ToString(((Button)sender).Content);

            RaiseOnTextButtonClicked(character);
        }

        /// <summary>
        /// Raises the on text button clicked.
        /// </summary>
        /// <param name="key">The key.</param>
        private void RaiseOnTextButtonClicked(string character)
        {
            EventHandler<TextButtonClickedEventArgs> handler = OnTextButtonClicked;
            if (handler != null)
            {
                App.Current.Dispatcher.BeginInvoke(new Action(() => handler(this, new TextButtonClickedEventArgs(character))));
            }
        }
    }
}
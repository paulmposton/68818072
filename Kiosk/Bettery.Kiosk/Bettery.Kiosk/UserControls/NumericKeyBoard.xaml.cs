using System;
using System.Windows;
using System.Windows.Controls;
using Bettery.Kiosk.Common;

namespace Bettery.Kiosk.UserControls
{
    /// <summary>
    /// Interaction logic for NumericKeyBoard.xaml
    /// </summary>
    public partial class NumericKeyBoard : UserControl
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="NumericKeyBoard" /> class.
        /// </summary>
        public NumericKeyBoard()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Occurs when [on text button clicked].
        /// </summary>
        public event EventHandler<KeyBoard.TextButtonClickedEventArgs> OnTextButtonClicked;

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
        /// <param name="character">The character.</param>
        private void RaiseOnTextButtonClicked(string character)
        {
            if (OnTextButtonClicked != null)
            {
                OnTextButtonClicked.Invoke(this, new KeyBoard.TextButtonClickedEventArgs(character));
            }
        }
    }
}
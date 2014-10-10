using System.Windows;
using System.Windows.Controls;
using Bettery.Kiosk.Common;
using System;
using Bettery.Kiosk.Controllers;

namespace Bettery.Kiosk.UserControls
{
    /// <summary>
    /// Interaction logic for PromoCodes.xaml
    /// </summary>
    public partial class PromoCodes : UserControl
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PromoCodes" /> class.
        /// </summary>
        public PromoCodes()
        {
            InitializeComponent();

            PromoKeyboard.OnTextButtonClicked += PromoKeyboard_OnTextButtonClicked;
        }

        public delegate void PromoCodeChanged(string code);

        /// <summary>
        /// Occurs when [on done button clicked].
        /// </summary>
        public event PromoCodeChanged OnDoneButtonClicked;

        /// <summary>
        /// Occurs when [on cancel button clicked].
        /// </summary>
        public event EventHandler OnCancelButtonClicked;

        /// <summary>
        /// Occurs when [on promo code changed].
        /// </summary>
        public event PromoCodeChanged OnPromoCodeChanged;

        /// <summary>
        /// Gets or sets the promo codes.
        /// </summary>
        /// <value>
        /// The promo codes.
        /// </value>
        public string PromoCodeValue { get; set; }

        /// <summary>
        /// Handles the OnTextButtonClicked event of the PromoKeyboard control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="KeyBoard.TextButtonClickedEventArgs" /> instance containing the event data.</param>
        protected void PromoKeyboard_OnTextButtonClicked(object sender, KeyBoard.TextButtonClickedEventArgs e)
        {
            if (e.Character == "BACKSPACE")
            {
                if (PromoCode.Text.Length > 0)
                {
                    PromoCode.Text = PromoCode.Text.Substring(0, PromoCode.Text.Length - 1);
                    PromoCode.Select(PromoCode.Text.Length, 0);

                }

            }
            else
            {
                UIHelper.SendInput(PromoCode, e.Character);
            }

            PromoCode.Focus();
        }

        /// <summary>
        /// Handles the TextChanged event of the PromoCode control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="TextChangedEventArgs" /> instance containing the event data.</param>
        private void PromoCode_TextChanged(object sender, TextChangedEventArgs e)
        {
            PromoCodeValue = PromoCode.Text;

            if (OnPromoCodeChanged != null)
            {
                OnPromoCodeChanged.Invoke(PromoCodeValue);
            }
        }

        /// <summary>
        /// Handles the Click event of the DoneButton control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs" /> instance containing the event data.</param>
        private void DoneButton_Click(object sender, RoutedEventArgs e)
        {
            if (OnDoneButtonClicked != null)
            {
                OnDoneButtonClicked.Invoke(PromoCodeValue);
            }
        }

        /// <summary>
        /// Handles the Click event of the CancelButton control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.RoutedEventArgs"/> instance containing the event data.</param>
        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            if (OnCancelButtonClicked != null)
            {
                OnCancelButtonClicked.Invoke(null, null);
            }
        }

        /// <summary>
        /// Loads this instance.
        /// </summary>
        public void Load()
        {
            Message.Text = Constants.Messages.InvalidPromotionCode;
            PromoCodeValue = string.Empty;
            ProcessInvalidAttempts();

            PromoCode.Clear();
            PromoCode.DelayedFocus();
        }

        /// <summary>
        /// Shows the error message.
        /// </summary>
        public void ShowErrorMessage(string invalidReasonCode)
        {

            ProcessInvalidAttempts(invalidReasonCode);
        }

                /// <summary>
        /// Processes the invalid attempts.
        /// </summary>
        private void ProcessInvalidAttempts()
        {
            if (BaseController.InvalidPromotionCodeAttempts >= BaseController.MaxInvalidPromotionCodeAttempts)
            {
                DoneButton.IsEnabled = false;
                Message.Text = Constants.Messages.TooManyInvalidPromotionCodeTries;
                
                Message.Visibility = Visibility.Visible;
            }
            else
            {
                Message.Text = Constants.Messages.InvalidPromotionCode;

                Message.Visibility = Visibility.Collapsed;

                DoneButton.IsEnabled = true;
            }
        }


        /// <summary>
        /// Processes the invalid attempts.
        /// </summary>
        private void ProcessInvalidAttempts(string invalidReasonCode)
        {
            if (BaseController.InvalidPromotionCodeAttempts >= BaseController.MaxInvalidPromotionCodeAttempts)
            {
                DoneButton.IsEnabled = false;

                Message.Text = Constants.Messages.InvalidPromotionCode + " " + Constants.Messages.TooManyInvalidPromotionCodeTries;

                Message.Visibility = Visibility.Visible;
            }
            else
            {
                if (invalidReasonCode.Length > 0)
                    Message.Text = invalidReasonCode;
                else
                    Message.Text = Constants.Messages.InvalidPromotionCode;

                Message.Visibility = Visibility.Visible;

                DoneButton.IsEnabled = true;
            }
        }
    }
}
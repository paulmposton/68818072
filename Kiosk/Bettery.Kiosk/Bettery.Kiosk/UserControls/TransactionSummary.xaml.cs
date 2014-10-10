using System;
using System.Windows;
using System.Windows.Controls;
using Bettery.Kiosk.Common;
using Bettery.Kiosk.Controllers;
using Bettery.Kiosk.Entities;

namespace Bettery.Kiosk.UserControls
{
    /// <summary>
    /// Interaction logic for TransactionSummary.xaml
    /// </summary>
    public partial class TransactionSummary : UserControl
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TransactionSummary" /> class.
        /// </summary>
        public TransactionSummary()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Occurs when [on discount code clicked].
        /// </summary>
        public event EventHandler<RoutedEventArgs> OnDiscountCodeClicked;

        /// <summary>
        /// Occurs when [on done clicked].
        /// </summary>
        public event EventHandler<RoutedEventArgs> OnDoneClicked;

        /// <summary>
        /// Occurs when [on back clicked].
        /// </summary>
        public event EventHandler<RoutedEventArgs> OnBackClicked;

        /// <summary>
        /// Handles the Click event of the DiscountCode control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs" /> instance containing the event data.</param>
        private void DiscountCode_Click(object sender, RoutedEventArgs e)
        {
            if (OnDiscountCodeClicked != null)
            {
                OnDiscountCodeClicked.Invoke(sender, e);
            }
        }

        /// <summary>
        /// Handles the Click event of the DoneButton control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs" /> instance containing the event data.</param>
        private void DoneButton_Click(object sender, RoutedEventArgs e)
        {
            if (OnDoneClicked != null)
            {
                OnDoneClicked.Invoke(sender, e);
            }
        }

        /// <summary>
        /// Handles the Click event of the BackButton control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs" /> instance containing the event data.</param>
        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            if (OnBackClicked != null)
            {
                OnBackClicked.Invoke(sender, e);
            }
        }

        /// <summary>
        /// Sets the promotional amount.
        /// </summary>
        public void PopulateCreditAndTotal()
        {
            decimal accountCredit = 0M;
            decimal totalAmount = 0M;
            decimal totalTax = 0M;
            decimal promotionAmount = 0M;

            this.TotalTax.Visibility = Visibility.Collapsed;
            this.TotalTaxTitle.Visibility = Visibility.Collapsed;

            this.DiscountCodeCredit.Visibility = Visibility.Visible;
            this.PromoButton.Visibility = Visibility.Visible;
            this.DiscountCodeCreditTitle.Visibility = Visibility.Visible;

            this.DepositAmount.Visibility = Visibility.Collapsed;
            this.DepositAmountTitle.Visibility = Visibility.Collapsed;
            this.DepositUnits.Visibility = Visibility.Collapsed;

            if (BaseController.SelectedBettery.DepositAmount > 0)
            {
                this.DepositUnits.Text = BaseController.SelectedBettery.NewCartridges.ToString();
                this.DepositAmount.Text = string.Format(Constants.Messages.PriceMessage, BaseController.SelectedBettery.DepositAmount);
                this.DepositAmount.Visibility = Visibility.Visible;
                this.DepositAmountTitle.Visibility = Visibility.Visible;
                this.DepositUnits.Visibility = Visibility.Visible; 
            }

            if (BaseController.SelectedBettery != null)
            {
                promotionAmount = BaseController.SelectedBettery.PromotionalAmount;
                accountCredit += BaseController.SelectedBettery.CalculatedReturnedAmount;

                totalAmount = BaseController.SelectedBettery.TotalAmount;
                
                if (BaseController.SelectedBettery.TotalTaxAmount > 0)
                {
                    totalTax = BaseController.SelectedBettery.TotalTaxAmount;
                    this.TotalTax.Visibility = Visibility.Visible;
                    this.TotalTaxTitle.Visibility = Visibility.Visible;
                }
                
            }
            if (promotionAmount <= 0)
                DiscountCodeCredit.Text = string.Format(Constants.Messages.PriceMessage, Math.Abs(promotionAmount));
            else
                DiscountCodeCredit.Text = "-" + string.Format(Constants.Messages.PriceMessage, Math.Abs(promotionAmount));

            TotalTax.Text = string.Format(Constants.Messages.TaxMessage, totalTax);
            if (totalAmount > 0)
                TotalPrice.Text = string.Format(Constants.Messages.PriceMessage, totalAmount);
            else
                TotalPrice.Text = string.Format(Constants.Messages.PriceMessage, 0M);

        }

        /// <summary>
        /// Loads this instance.
        /// </summary>
        public void Load()
        {
            BaseController.CalcCharges();

            BetteryVend betteryVend = BaseController.SelectedBettery;

            //if (BaseController.GetBatteriesMode == GetBatteriesModes.BuyNew)
            //{
                AATextbox.Text = betteryVend.AaVend.ToString();
                AAATextbox.Text = betteryVend.AaaVend.ToString();

                AAPrice.Text = string.Format(Constants.Messages.PriceMessage, betteryVend.CalculatedAaAmount);
                AAAPrice.Text = string.Format(Constants.Messages.PriceMessage, betteryVend.CalculatedAaaAmount);

                TotalCartridge.Text = betteryVend.TotalVendCartridges.ToString();
                PurchaseSubtotalAmount.Text = string.Format(Constants.Messages.PriceMessage, betteryVend.SubSubTotalAmount);

            //}
            //else
            //{
            //    AATextbox.Text = betteryVend.AaForgotDrainedVend.ToString();
            //    AAATextbox.Text = betteryVend.AaaForgotDrainedVend.ToString();

            //    AAPrice.Text = string.Format(Constants.Messages.PriceMessage, betteryVend.AaNewForgotDrainedAmount);
            //    AAAPrice.Text = string.Format(Constants.Messages.PriceMessage, betteryVend.AaaNewForgotDrainedAmount);

            //    TotalCartridge.Text = betteryVend.TotalForgotDrainedVendCartridges.ToString();
            //}

            PopulateCreditAndTotal();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {

        }
    }
}
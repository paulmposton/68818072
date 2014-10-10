using System.Windows;
using System.Windows.Controls;
using Bettery.Kiosk.Common;
using Bettery.Kiosk.Controllers;
using Bettery.Kiosk.Entities;

namespace Bettery.Kiosk.UserControls
{
    /// <summary>
    /// Interaction logic for GetBatteries.xaml
    /// </summary>
    public partial class GetBatteries : UserControl
    {
        private int _aa;
        private int _aaa;

        private int _maxAaProduct;
        private int _maxAaaProduct;

        /// <summary>
        /// Gets a value indicating whether this instance is reached limit plan.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance is reached limit plan; otherwise, <c>false</c>.
        /// </value>
        bool isReachedLimitPlan
        {
            get
            {
                bool isReachedLimitPlan = false;
                if (BaseController.LoggedOnUser != null)
                {
                    int maxPlan = 0;
                    if (BaseController.GetBatteriesMode == GetBatteriesModes.BuyNew)
                    {
                        if (BaseController.LoggedOnUser.BatteriesInPlan == 0)
                        {
                            maxPlan = Constants.BetteryProduct.AaMax + Constants.BetteryProduct.AaaMax;
                        }
                        else
                        {
                            maxPlan = BaseController.LoggedOnUser.BatteriesInPlan - BaseController.LoggedOnUser.BatteriesCheckedOut + BaseController.SelectedBettery.ReturnedCartridges;
                        }
                    }
                    else
                    {
                        maxPlan = BaseController.LoggedOnUser.BatteriesCheckedOut;
                    }

                    isReachedLimitPlan = (_aa + _aaa) >= maxPlan;
                }

                return isReachedLimitPlan;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GetBatteries"/> class.
        /// </summary>
        public GetBatteries()
        {
            InitializeComponent();
        }

        public delegate void DoneButtonEventHandler(BetteryVend betteryVend);

        /// <summary>
        /// Gets or sets the on done button clicked.
        /// </summary>
        /// <value>
        /// The on done button clicked.
        /// </value>
        public event DoneButtonEventHandler OnDoneButtonClicked;

        /// <summary>
        /// Handles the TextChanged event of the aaTextbox control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.Controls.TextChangedEventArgs"/> instance containing the event data.</param>
        private void AATextbox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (IsLoaded)
            {
                OnBatteriesAmountChanged();
            }
        }

        /// <summary>
        /// Handles the TextChanged event of the aaaTextbox control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.Controls.TextChangedEventArgs"/> instance containing the event data.</param>
        private void AAATextbox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (IsLoaded)
            {
                OnBatteriesAmountChanged();
            }
        }

        /// <summary>
        /// Handles the Click event of the AaPlus control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.RoutedEventArgs"/> instance containing the event data.</param>
        private void AAPlus_Click(object sender, RoutedEventArgs e)
        {
            if (_aa < _maxAaProduct && !isReachedLimitPlan)
            {
                _aa++;

                AATextbox.Text = _aa.ToString();
            }
        }

        /// <summary>
        /// Handles the Click event of the AaMinus control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.RoutedEventArgs"/> instance containing the event data.</param>
        private void AAMinus_Click(object sender, RoutedEventArgs e)
        {
            if (_aa > 0)
            {
                _aa--;
                AATextbox.Text = _aa.ToString();
            }
        }

        /// <summary>
        /// Handles the Click event of the AaaPlus control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.RoutedEventArgs"/> instance containing the event data.</param>
        private void AAAPlus_Click(object sender, RoutedEventArgs e)
        {
            if (_aaa < _maxAaaProduct && !isReachedLimitPlan)
            {
                _aaa++;

                AAATextbox.Text = _aaa.ToString();
            }
        }

        /// <summary>
        /// Handles the Click event of the AaaMinus control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.RoutedEventArgs"/> instance containing the event data.</param>
        private void AAAMinus_Click(object sender, RoutedEventArgs e)
        {
            if (_aaa > 0)
            {
                _aaa--;
                AAATextbox.Text = _aaa.ToString();
            }
        }

        /// <summary>
        /// Handles the Click event of the DoneButton control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.RoutedEventArgs"/> instance containing the event data.</param>
        private void DoneButton_Click(object sender, RoutedEventArgs e)
        {
            if (OnDoneButtonClicked != null)
            {
                BetteryVend betteryVend = BaseController.SelectedBettery;
                OnDoneButtonClicked.Invoke(betteryVend);
            }
        }

        /// <summary>
        /// Called when [batteries amount changed].
        /// </summary>
        private void OnBatteriesAmountChanged()
        {
            BaseController.CalcCharges(_aa, _aaa, BaseController.GetBatteriesMode);

            bool isReachedAALimit = _aa >= _maxAaProduct;
            bool isReachedAAALimit = _aaa >= _maxAaaProduct;


            AAPlus.IsEnabled = (!isReachedAALimit && !isReachedLimitPlan);
            AAAPlus.IsEnabled = (!isReachedAAALimit && !isReachedLimitPlan);

            // User has requested the max batteries in inventory?
            if (isReachedAALimit)
            {
                LimitAaMessageTextBlock.Visibility = Visibility.Visible;
                LimitAaMessageTextBlock.Text = Constants.Messages.LimitedBatteries;
            }
            // Subscription plan limit, un-used code branch?
            else if (isReachedLimitPlan)
            {
                LimitAaMessageTextBlock.Visibility = Visibility.Visible;
                if (BaseController.GetBatteriesMode == GetBatteriesModes.BuyNew)
                {
                    LimitAaMessageTextBlock.Text = Constants.Messages.LimitedBatteriesInPlan;
                }
                else
                {
                    LimitAaMessageTextBlock.Text = Constants.Messages.LimitedBatteriesCheckedOut;
                }
            }
            else
            {
                LimitAaMessageTextBlock.Visibility = Visibility.Hidden;
            }

            // User has requested the max batteries in inventory?
            if (isReachedAAALimit)
            {
                LimitAaaMessageTextBlock.Visibility = Visibility.Visible;
                LimitAaaMessageTextBlock.Text = Constants.Messages.LimitedBatteries;
            }
            // Subscription plan limit, un-used code branch?
            else if (isReachedLimitPlan)
            {
                LimitAaaMessageTextBlock.Visibility = Visibility.Visible;
                if (BaseController.GetBatteriesMode == GetBatteriesModes.BuyNew)
                {
                    LimitAaaMessageTextBlock.Text = Constants.Messages.LimitedBatteriesInPlan;
                }
                else
                {
                    LimitAaaMessageTextBlock.Text = Constants.Messages.LimitedBatteriesCheckedOut;
                }
            }
            else
            {
                LimitAaaMessageTextBlock.Visibility = Visibility.Hidden;
            }

            AAMinus.IsEnabled = _aa > 0;
            AAAMinus.IsEnabled = _aaa > 0;

            // Update view
            if (BaseController.GetBatteriesMode == GetBatteriesModes.BuyNew)
            {
                ReturnMessage.Text = Constants.Messages.YouHaveReturnMessage;  //Unused ??  This is blank in the constants file
                ValueReturnMessage.Text = string.Format(Constants.Messages.ValueReturnMessage, BaseController.SelectedBettery.ReturnedCartridges);  //ValueReturnMessage = "You have returned {0} pack(s) of BETTERY batteries.";
                NewSelectMessage.Text = string.Format(Constants.Messages.NewSelectMessage, BaseController.SelectedBettery.ReturnedCartridges, BaseController.SwapPrice);  //NewSelectMessage = "SWAP up to {0} 4-pack(s) of freshly charged batteries at the SWAP price of ${1:N2} per pack.";         

                CommitMessage.Visibility = Visibility.Collapsed;  // This means this branch is run when you have not forgotten batteries.  (which is no longer a function we support)
                NewMessage.Visibility = Visibility.Visible; // Blank
                ReturnMessage.Visibility = Visibility.Visible;  //Blank
                //NewSelectMessage.Visibility = Visibility.Visible;  //CK, not sure what this was, it would always be visible, even though the branches above turn it on/off

                //
                // CK 1/26/13 - New code branch, hiding too much text if no swaps are available, and simplifying text
                //                
                if (BaseController.SelectedBettery.ReturnedCartridges > 0)
                {
                    Message.Text = string.Format(Constants.Messages.BuyNewAdditionalMessage, BaseController.SwapPrice); //BuyNewAdditionalMessage = "Get additional 4-packs for ${1:N2} per pack, with a one-time $5 deposit per pack.";
                    //Message.Text = Constants.Messages.BuyNewAdditionalMessage;
                    ValueReturnMessage.Visibility = Visibility.Visible;
                    NewSelectMessage.Visibility = Visibility.Visible;
                    Message.Visibility = Visibility.Visible;
                }
                else
                {
                    Message.Text = string.Format(Constants.Messages.BuyNewMessage, BaseController.SwapPrice); //BuyNewMessage = "Get 4-packs for ${1:N2} per pack, with a one-time $5 deposit per pack.";
                    //Message.Text = Constants.Messages.BuyNewMessage;
                    ValueReturnMessage.Visibility = Visibility.Collapsed;
                    NewSelectMessage.Visibility = Visibility.Collapsed;
                    Message.Visibility = Visibility.Visible;
                }
            }
            else
            {
                 //
                //  CK 1/6/13 - this whole branch should never run, since we have disabled the forgot batteries button.
                //
                Message.Visibility = Visibility.Collapsed;
                CommitMessage.Text = string.Format(Constants.Messages.SwapForgotBatteryNotice, _aa, _aaa);
                CommitMessage.Visibility = Visibility.Visible;
                NewMessage.Visibility = Visibility.Collapsed;
                NewSelectMessage.Visibility = Visibility.Collapsed;
                ReturnMessage.Visibility = Visibility.Collapsed;
                ValueReturnMessage.Visibility = Visibility.Collapsed;
            }

            if (BaseController.SelectedBettery != null && BaseController.SelectedBettery.TotalVendCartridges > 0 )
            {
                DoneButton.IsEnabled = true;
            }
            else
            {
                DoneButton.IsEnabled = false;
            }
        }

        /// <summary>
        /// Loads this instance.
        /// </summary>
        public void Load(int maxAaProduct, int maxAaaProduct)
        {
            _maxAaProduct = maxAaProduct;
            _maxAaaProduct = maxAaaProduct;

            if (BaseController.SelectedBettery == null)
            {
                BaseController.SelectedBettery = new BetteryVend();
            }

            if (BaseController.GetBatteriesMode == GetBatteriesModes.BuyNew)
            {
                _aa = BaseController.SelectedBettery.AaVend;
                _aaa = BaseController.SelectedBettery.AaaVend;
            }
                //
                // CK 1/6/13 - this branch should never run because we have disabled "forgot batteries"
                //
            else
            {
                _aa = BaseController.SelectedBettery.AaForgotDrainedVend;
                _aaa = BaseController.SelectedBettery.AaaForgotDrainedVend;
            }

            AATextbox.Text = _aa.ToString();
            AAATextbox.Text = _aaa.ToString();
            OnBatteriesAmountChanged();
        }
    }
}
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using BKiosk;
using BKiosk.HelperClasses;

namespace Bettery.Kiosk
{
    /// <summary>
    /// Interaction logic for GetBatteries.xaml
    /// </summary>
    public partial class GetBatteries : Page
    {
        private int _aa = 0;
        private int _aaa = 0;
        private int _aaCartridge = 0;
        private BackgroundWorker _worker = new BackgroundWorker();

        public GetBatteries()
        {
            InitializeComponent();
            BaseController.CurrentPage = this;
            //StartTimer();

            _worker.WorkerSupportsCancellation = true;
            _worker.RunWorkerCompleted += CalcCharges_RunWorkerCompleted;
            _worker.DoWork += CalcCharges_DoWork;
        }

        //private void StartTimer()
        //{
        //    DispatcherTimer timer = new DispatcherTimer();
        //    //  TODO: get timeout from config file
        //    const int Timeout = 15000;
        //    timer.Interval = TimeSpan.FromMilliseconds(Timeout);
        //    timer.Tick += new EventHandler(TimeoutHandler);
        //    timer.Start();
        //}
        //private void TimeoutHandler(Object sender, EventArgs args)
        //{
        //    DispatcherTimer thisTimer = (DispatcherTimer)sender;
        //    thisTimer.Stop();
        //    Start page = new Start();
        //    this.NavigationService.Navigate(page);
        //}
        public GetBatteries(int aaCartridge)
        {
            InitializeComponent();
            BaseController.CurrentPage = this;
            _aaCartridge = aaCartridge;

            _worker.WorkerSupportsCancellation = true;
            _worker.RunWorkerCompleted += CalcCharges_RunWorkerCompleted;
            _worker.DoWork += CalcCharges_DoWork;
        }

        /// <summary>
        /// Handles the Loaded event of the Page control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.RoutedEventArgs"/> instance containing the event data.</param>
        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            if (BaseController.IsLoggedOnUser)
            {
                Login.Content = "Logout";
            }

            OnBatteriesAmountChanged();
        }

        /// <summary>
        /// Handles the DoWork event of the CalcCharges control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="DoWorkEventArgs" /> instance containing the event data.</param>
        private void CalcCharges_DoWork(object sender, DoWorkEventArgs e)
        {
            // Calculate charges
            BetteryVend betteryVend = Calculations.CalcCharges(_aaCartridge, 0, _aa, _aaa);
            e.Result = betteryVend;
        }

        /// <summary>
        /// Handles the RunWorkerCompleted event of the CalcCharges control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RunWorkerCompletedEventArgs" /> instance containing the event data.</param>
        private void CalcCharges_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            BetteryVend betteryVend = e.Result as BetteryVend;

            if (betteryVend != null)
            {
                // Update view
                AAPrice.Text = string.Format("{0}$", betteryVend.AaNewAmount);
                AAAPrice.Text = string.Format("{0}$", betteryVend.AaaNewAmount);
                ReturnsPrice.Text = string.Format("-{0}$", betteryVend.ReturnedAmount);
                TotalPrice.Text = string.Format("{0}$", betteryVend.TotalAmount);
            }

            BetteryBusyIndicator.IsBusy = false;
        }

        private void DoneButton_Click(object sender, RoutedEventArgs e)
        {
            Checkout page = new Checkout(_aaCartridge, _aa, _aaa);
            this.NavigationService.Navigate(page);
        }

        private void aaPlus_Click(object sender, RoutedEventArgs e)
        {
            _aa++;
            aaTextbox.Text = _aa.ToString();
        }

        private void aaMinus_Click(object sender, RoutedEventArgs e)
        {
            if (_aa > 0)
            {
                _aa--;
                aaTextbox.Text = _aa.ToString();
            }
        }

        private void aaaPlus_Click(object sender, RoutedEventArgs e)
        {
            _aaa++;
            aaaTextbox.Text = _aaa.ToString();
        }

        private void aaaMinus_Click(object sender, RoutedEventArgs e)
        {
            if (_aaa > 0)
            {
                _aaa--;
                aaaTextbox.Text = _aaa.ToString();
            }
        }

        private void FAQButton_Click(object sender, RoutedEventArgs e)
        {
        }

        /// <summary>
        /// Handles the Click event of the LogInButton control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs" /> instance containing the event data.</param>
        private void LogInButton_Click(object sender, RoutedEventArgs e)
        {
            if (BaseController.IsLoggedOnUser)
            {
                BaseController.Logout();
            }
            else
            {
                BaseController.PreviousPage = this;
                Login page = new Login();
                this.NavigationService.Navigate(page);
            }
        }

        private void StartOverButton_Click(object sender, RoutedEventArgs e)
        {
            BaseController.PreviousPage = this;
            Start page = new Start();
            this.NavigationService.Navigate(page);
        }

        private void Terms_PrivacyButton_Click(object sender, RoutedEventArgs e)
        {
            BaseController.PreviousPage = this;
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            BaseController.PreviousPage = this;
            Welcome page = new Welcome();
            this.NavigationService.Navigate(page);
        }

        /// <summary>
        /// Handles the TextChanged event of the aaTextbox control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="TextChangedEventArgs" /> instance containing the event data.</param>
        private void aaTextbox_TextChanged(object sender, TextChangedEventArgs e)
        {
            OnBatteriesAmountChanged();
        }

        /// <summary>
        /// Handles the TextChanged event of the aaaTextbox control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="TextChangedEventArgs" /> instance containing the event data.</param>
        private void aaaTextbox_TextChanged(object sender, TextChangedEventArgs e)
        {
            OnBatteriesAmountChanged();
        }

        #region Override

        /// <summary>
        /// Invoked when an unhandled <see cref="E:System.Windows.Input.Mouse.PreviewMouseDown"/> attached routed event reaches an element in its route that is derived from this class. Implement this method to add class handling for this event.
        /// </summary>
        /// <param name="e">The <see cref="T:System.Windows.Input.MouseButtonEventArgs"/> that contains the event data. The event data reports that one or more mouse buttons were pressed.</param>
        protected override void OnPreviewMouseDown(MouseButtonEventArgs e)
        {
            base.OnPreviewMouseDown(e);
            BaseController.ResetLastActiveTime();
        }

        /// <summary>
        /// Provides class handling for the <see cref="E:System.Windows.UIElement.PreviewTouchDown"/> routed event that occurs when a touch presses this element.
        /// </summary>
        /// <param name="e">A <see cref="T:System.Windows.Input.TouchEventArgs"/> that contains the event data.</param>
        protected override void OnPreviewTouchDown(TouchEventArgs e)
        {
            base.OnPreviewTouchDown(e);
            BaseController.ResetLastActiveTime();
        }

        /// <summary>
        /// Invoked when an unhandled <see cref="E:System.Windows.Input.Mouse.PreviewMouseMove"/> attached event reaches an element in its route that is derived from this class. Implement this method to add class handling for this event.
        /// </summary>
        /// <param name="e">The <see cref="T:System.Windows.Input.MouseEventArgs"/> that contains the event data.</param>
        protected override void OnPreviewMouseMove(MouseEventArgs e)
        {
            base.OnPreviewMouseMove(e);
            BaseController.ResetLastActiveTime();
        }

        #endregion Override

        /// <summary>
        /// Called when [batteries amount changed].
        /// </summary>
        private void OnBatteriesAmountChanged()
        {
            if (IsLoaded)
            {
                BetteryBusyIndicator.IsBusy = true;

                // Handle buttons
                // Assumpt that we have maximum 9 batteries

                if (_aa == 9)
                {
                    AaPlus.IsEnabled = false;
                }
                else
                {
                    AaPlus.IsEnabled = true;
                }

                if (_aaa == 9)
                {
                    AaaPlus.IsEnabled = false;
                }
                else
                {
                    AaaPlus.IsEnabled = true;
                }

                if (_aa > 0)
                {
                    AaMinus.IsEnabled = true;
                }
                else
                {
                    AaMinus.IsEnabled = false;
                }
                if (_aaa > 0)
                {
                    AaaMinus.IsEnabled = true;
                }
                else
                {
                    AaaMinus.IsEnabled = false;
                }

                _worker.RunWorkerAsync();
            }
        }
    }
}
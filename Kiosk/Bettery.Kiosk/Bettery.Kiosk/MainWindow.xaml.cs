using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Diagnostics;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Bettery.Kiosk.Common;
using Bettery.Kiosk.Controllers;
using Bettery.Kiosk.Entities;
using Bettery.Kiosk.UserControls;
using System.Text;
using Bettery.Kiosk.DataAccess;

namespace Bettery.Kiosk
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly Dictionary<string, UserControl> _controls = new Dictionary<string, UserControl>();
        private readonly Timer _alertTimer = new Timer();

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="MainWindow"/> class.
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();
            InitControls();
        }

        #endregion Constructor

        #region Form Events

        /// <summary>
        /// Handles the Loaded event of the Window control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.RoutedEventArgs"/> instance containing the event data.</param>
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            SetBusyIndicatorMessage();
            NavigateToView(Constants.ViewName.Start, null);

            // Init timer for timeout processing
            int maxIdleTime = BaseController.MaxIdleTime;
            int showCountDown = BaseController.ShowCountDown;

            BaseController.OnThrowException += BaseController_OnThrowException;
            BaseController.OnInterfaceKitInputChanged += BaseController_OnInterfaceKitInputChanged;

            //
            //  If global test mode is on, turn on indicator
            //
            if (ConfigurationManager.AppSettings["TestTransaction"] == "TRUE")
                UIHelper.DispatchThread(this, main => main.FreeVendModeButton.Visibility = Visibility.Visible);

            BaseController.OnLogout += (mainWindow, args) =>
            {
                UIHelper.DispatchThread(this, main => main.InventoryButton.Visibility = Visibility.Collapsed);
                if (ConfigurationManager.AppSettings["TestTransaction"] != "TRUE")
                    UIHelper.DispatchThread(this, main => main.FreeVendModeButton.Visibility = Visibility.Collapsed);
                UIHelper.DispatchThread(this, main => main.HelloUser.Visibility = Visibility.Hidden);

                UIHelper.DispatchThread(this, main => main.Logout.Visibility = Visibility.Collapsed);
                UIHelper.DispatchThread(this, main => main.Login.Visibility = Visibility.Visible);
            };

            LoginController.OnLogin += (mainWindow, args) =>
            {
                if (BaseController.LoggedOnUser.GroupID == Constants.Group.SuperUser || BaseController.LoggedOnUser.GroupID == Constants.Group.SwapStationAdmin)
                {
                    UIHelper.DispatchThread(this, main => main.InventoryButton.Visibility = Visibility.Visible);
                    UIHelper.DispatchThread(this, main => main.FreeVendModeButton.Visibility = Visibility.Visible);
                    Logger.Log(EventLogEntryType.Information, string.Format("User: SwapStation Admin or SuperUser Logged In: {0}", BaseController.LoggedOnUser.MemberFirstName), BaseController.StationId);
                }
                else
                    UIHelper.DispatchThread(this, main => main.InventoryButton.Visibility = Visibility.Collapsed);

                UIHelper.DispatchThread(this, main => main.HelloUser.Text = string.Format("Hello, {0}", BaseController.LoggedOnUser.MemberFirstName));
                UIHelper.DispatchThread(this, main => main.HelloUser.Visibility = Visibility.Visible);

                BaseController.RegistrationUser = null;
                UIHelper.DispatchThread(this, main => main.Logout.Visibility = Visibility.Visible);
                UIHelper.DispatchThread(this, main => main.Login.Visibility = Visibility.Collapsed);
            };

            VendingController.OnVending += OnVending;
            VendingController.OnVendingCase += VendingController_OnVendingCase;

            PopupCountDown.Load(BetteryBusyIndicator, maxIdleTime, showCountDown);
            PopupCountDown.OnCountDownEnd += OnCountDownEnd;
            PopupCountDown.OnYesClicked += PopupCountDown_OnYesClicked;
            PopupCountDown.OnNoClicked += PopupCountDown_OnNoClicked;

            InitAlertTimer();
            //
            //  Log a report on inventory here, on startup
            //
            Logger.Log(EventLogEntryType.Information, GetInventoryReport(), BaseController.StationId);
        }

        /// <summary>
        /// Handles the GotFocus event of the Window control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs" /> instance containing the event data.</param>
        private void Window_GotFocus(object sender, RoutedEventArgs e)
        {
            Control control = e.OriginalSource as Control;
            CountDown countDown = e.Source as CountDown;

            if (control != null && countDown == null)
            {
                BaseController.CurrentControlFocus = control;
            }
        }

        #endregion Form Events

        #region Exception handling

        /// <summary>
        /// Handles the OnThrowException event of the BaseController control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="eventArgs">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void BaseController_OnThrowException(object sender, EventArgs eventArgs)
        {
            //Todo: Process current transaction information (Tobe defined)

            //Logout
            BaseController.Logout();

            //Navigate to error page
            UIHelper.DispatchThread(this, main => main.NavigateToView(Constants.ViewName.ErrorMessage, null));
        }

        #endregion Exception handling

        #region Alert

        /// <summary>
        /// Inits the alert timer.
        /// </summary>
        private void InitAlertTimer()
        {
            _alertTimer.Interval = BaseController.AlertInterval;
            _alertTimer.Start();
            _alertTimer.Elapsed += new ElapsedEventHandler(AlertTimer_Elapsed);
        }

        /// <summary>
        /// Handles the Elapsed event of the AlertTimer control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Timers.ElapsedEventArgs"/> instance containing the event data.</param>
        private void AlertTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            using (BackgroundWorker worker = new BackgroundWorker())
            {
                worker.DoWork += delegate
                                     {
                                         //
                                         //  Log a report on inventory here, on startup
                                         //
                                         Logger.Log(EventLogEntryType.Information, GetInventoryReport(), BaseController.StationId);

                                         //AlertController.Alert();
                                     };

                worker.RunWorkerAsync();
            }
        }

        #endregion Alert

        #region InterfaceKit

        /// <summary>
        /// Handles the OnInterfaceKitInputChanged event of the BaseController control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void BaseController_OnInterfaceKitInputChanged(object sender, EventArgs e)
        {
            if (BaseController.CurrentView == Constants.ViewName.Exchange)
            {
                using (BackgroundWorker worker = new BackgroundWorker())
                {
                    worker.DoWork += (o, args) =>
                                         {
                                             SwapController.ReturnCartridge();
                                         };

                    worker.RunWorkerCompleted += (o, args) =>
                    {
                        UserControl control = _controls[Constants.ViewName.Exchange];
                        if (control != null)
                        {
                            // Resets the control with the number of packs Exchnaged
                            ((Exchange)control).Load();
                        }

                        PopupCountDown.ResetPopup();
                    };

                    worker.RunWorkerAsync();
                }
            }
        }

        #endregion InterfaceKit

        #region Main Navigation Buttons
        /// <summary>
        /// Handles the Click event of the TermsPrivacy control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.RoutedEventArgs"/> instance containing the event data.</param>
        private void InventoryLogin_Click(object sender, RoutedEventArgs e)
        {
            //
            // Go ahead and do the report here, in addition to on startup.
            //
            Logger.Log(EventLogEntryType.Information, GetInventoryReport(), BaseController.StationId);
            if (BaseController.LoggedOnUser != null)
            {
                if (BaseController.LoggedOnUser.GroupID == Constants.Group.SuperUser || BaseController.LoggedOnUser.GroupID == Constants.Group.SwapStationAdmin)
                    NavigateToView(Constants.ViewName.InventoryAdmin, null);                
            }
        }

        /// <summary>
        /// Handles the Click event of the StartOver control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.RoutedEventArgs"/> instance containing the event data.</param>
        private void StartOver_Click(object sender, RoutedEventArgs e)
        {
            BetteryVend selectedBettery = BaseController.SelectedBettery;
            if (selectedBettery != null && (selectedBettery.ReturnedCartridges > 0 || selectedBettery.TotalVendCartridges > 0 || selectedBettery.TotalForgotDrainedVendCartridges > 0))
            {
                if (BaseController.LoggedOnUser != null && selectedBettery.ReturnedCartridges > 0)
                {
                    Logger.Log(EventLogEntryType.Information, string.Format("User: Member {0} returned {1} cases and signed off", BaseController.LoggedOnUser.UserName, selectedBettery.ReturnedCartridges), BaseController.StationId);
                }
                NavigateToView(Constants.ViewName.Confirmation, null);
            }
            else
            {
                BaseController.Logout();
                NavigateToView(Constants.ViewName.Start, null);
            }
        }

        /// <summary>
        /// Handles the Click event of the Login control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.RoutedEventArgs"/> instance containing the event data.</param>
        private void Login_Click(object sender, RoutedEventArgs e)
        {
            NavigateToView(Constants.ViewName.Login, null);
        }

        /// <summary>
        /// Handles the Click event of the SignUp control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.RoutedEventArgs"/> instance containing the event data.</param>
        private void SignUp_Click(object sender, RoutedEventArgs e)
        {
            NavigateToView(Constants.ViewName.MembershipRegistration, null);
        }

        /// <summary>
        /// Handles the Click event of the Logout control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.RoutedEventArgs"/> instance containing the event data.</param>
        private void Logout_Click(object sender, RoutedEventArgs e)
        {
            BetteryVend selectedBettery = BaseController.SelectedBettery;
            if (selectedBettery != null)
            {
                if (BaseController.LoggedOnUser != null && selectedBettery.ReturnedCartridges > 0)
                {
                    Logger.Log(EventLogEntryType.Information, string.Format("User: Member {0} returned {1} cases and signed off", BaseController.LoggedOnUser.UserName, selectedBettery.ReturnedCartridges), BaseController.StationId);
                }
            }
   
            BaseController.Logout();

            // Navigate to start page
            UIHelper.DispatchThread(this, main => main.NavigateToView(Constants.ViewName.Start, null));
        }

        /// <summary>
        /// Handles the Click event of the FAQ control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.RoutedEventArgs"/> instance containing the event data.</param>
        private void FAQ_Click(object sender, RoutedEventArgs e)
        {
            NavigateToView(Constants.ViewName.Faq, null);
        }

        /// <summary>
        /// Handles the Click event of the TermsPrivacy control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.RoutedEventArgs"/> instance containing the event data.</param>
        private void TermsPrivacy_Click(object sender, RoutedEventArgs e)
        {
            NavigateToView(Constants.ViewName.Privacy, null);
        }

        /// <summary>
        /// Handles the Click event of the ViewProfile control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.RoutedEventArgs"/> instance containing the event data.</param>
        private void ViewProfile_Click(object sender, RoutedEventArgs e)
        {
            ///NavigateToView(Constants.ViewName.UserProfile, null);
        }

        /// <summary>
        /// Shows the header and footer.
        /// </summary>
        private void ShowHeaderAndFooter()
        {
            HeaderGrid.Visibility = Visibility.Visible;
            FooterGrid.Visibility = Visibility.Visible;
        }

        /// <summary>
        /// Hides the header and footer.
        /// </summary>
        private void HideHeaderAndFooter()
        {
            HeaderGrid.Visibility = Visibility.Collapsed;
            FooterGrid.Visibility = Visibility.Collapsed;
        }

        /// <summary>
        /// Shows the navigation panel.
        /// Also includes the signup button, which hangs out below that
        /// </summary>
        private void ShowNavigationPanel()
        {
            MainNavigationStackPanel.Visibility = Visibility.Visible;
            SignUp.Visibility = Visibility.Visible;
        }

        /// <summary>
        /// Hides the navigation panel.
        /// Also includes the signup button, which hangs out below that
        /// </summary>
        private void HideNavigationPanel()
        {
            MainNavigationStackPanel.Visibility = Visibility.Collapsed;
            SignUp.Visibility = Visibility.Collapsed;
        }

        #endregion Main Navigation Buttons

        #region Override

        /// <summary>
        /// Invoked when an unhandled <see cref="E:System.Windows.Input.Mouse.PreviewMouseDown"/> attached routed event reaches an element in its route that is derived from this class. Implement this method to add class handling for this event.
        /// </summary>
        /// <param name="e">The <see cref="T:System.Windows.Input.MouseButtonEventArgs"/> that contains the event data. The event data reports that one or more mouse buttons were pressed.</param>
        protected override void OnPreviewMouseDown(MouseButtonEventArgs e)
        {
            // Don't have to worry about countdown popup, because we will be sure to move off thankyou before that shows up.
            if (BaseController.CurrentView == Constants.ViewName.ThankYou)
            {
                ThankYou_OnDoneButtonClicked(this, e);
            }
            else if (BaseController.CurrentView == Constants.ViewName.Start && !(e.Source is CountDown))
            {
                if (e.Source is Start && ((Start)e.Source).IsMessageBoxVisible)
                {
                    e.Handled = false;
                }
                else
                {
                    Start_OnGetStartedButtonClicked(this, e);
                    e.Handled = true;
                }
            }
            else if (!(e.Source is Start) && !(e.Source is CountDown) && PopupCountDown.Visibility != Visibility.Visible)
            {
                PopupCountDown.ResetPopup();
            }

            base.OnPreviewMouseDown(e);
        }

        /// <summary>
        /// Provides class handling for the <see cref="E:System.Windows.UIElement.PreviewTouchDown"/> routed event that occurs when a touch presses this element.
        /// </summary>
        /// <param name="e">A <see cref="T:System.Windows.Input.TouchEventArgs"/> that contains the event data.</param>
        protected override void OnPreviewTouchDown(TouchEventArgs e)
        {
            // Don't have to worry about countdown popup, because we will be sure to move off thankyou before that shows up.
            if (BaseController.CurrentView == Constants.ViewName.ThankYou)
            {
                ThankYou_OnDoneButtonClicked(this, e);
            }
            else if (BaseController.CurrentView == Constants.ViewName.Start && !(e.Source is CountDown))
            {
                // Touch anywhwere on the start screen
                if (e.Source is Start && ((Start)e.Source).IsMessageBoxVisible)
                {
                    e.Handled = false;
                }
                else
                {
                    Start_OnGetStartedButtonClicked(this, e);
                    e.Handled = true;
                }
            }
            else if (!(e.Source is Start) && !(e.Source is CountDown) && PopupCountDown.Visibility != Visibility.Visible)
            {
                // Reset the timeout
                PopupCountDown.ResetPopup();
            }

            base.OnPreviewTouchDown(e);
        }

        /// <summary>
        /// Invoked when an unhandled <see cref="E:System.Windows.Input.Mouse.PreviewMouseMove" /> attached event reaches an element in its route that is derived from this class. Implement this method to add class handling for this event.
        /// </summary>
        /// <param name="e">The <see cref="T:System.Windows.Input.MouseEventArgs" /> that contains the event data.</param>
        protected override void OnPreviewMouseMove(MouseEventArgs e)
        {
            if (BaseController.CurrentView != Constants.ViewName.Start && !(e.Source is Start) && !(e.Source is CountDown) && PopupCountDown.Visibility != Visibility.Visible)
            {
                PopupCountDown.ResetPopup();
            }

            base.OnPreviewMouseMove(e);
        }

        #endregion Override

        #region Init Controls

        /// <summary>
        /// Inits the controls.
        /// </summary>
        private void InitControls()
        {
            Welcome welcome = new Welcome();
            welcome.OnGetBetteryBatteriesClicked += WelcomeOnGetBetteryBatteriesClicked;
            welcome.OnLearnMoreClicked += WelcomeOnLearnMoreClicked;
            _controls.Add(Constants.ViewName.Welcome, welcome);


            Selection selection = new Selection { Name = Constants.ViewName.Selection };
            selection.OnYesButtonClicked += Selection_OnYesButtonClicked;
            selection.OnNoButtonClicked += Selection_OnNoButtonClicked;
            selection.OnForgotDrainedBatterieButtonClicked += Selection_OnForgotDrainedBatterieButtonClicked;
            selection.OnGetCaseButtonButtonClicked += Selection_OnGetCaseButtonButtonClicked;
            selection.OnLearnMoreButtonButtonClicked += Selection_OnLearnMoreButtonButtonClicked;
            _controls.Add(Constants.ViewName.Selection, selection);

            Confirmation confirmation = new Confirmation();
            confirmation.OnOkClicked += ConfirmationOnOkClicked;
            confirmation.OnCancelClicked += ConfirmationOnCancelClicked;
            _controls.Add(Constants.ViewName.Confirmation, confirmation);

            Vending vending = new Vending { Name = Constants.ViewName.Vending };
            _controls.Add(Constants.ViewName.Vending, vending);

            VendingContinue vendingContinue = new VendingContinue { Name = Constants.ViewName.VendingContinue };
            _controls.Add(Constants.ViewName.VendingContinue, vendingContinue);
            vendingContinue.OnDoneButtonClicked += VendingContinue_OnDoneButtonClicked;

            Start start = new Start { Name = Constants.ViewName.Start };
            start.OnGetStartedButtonClicked += Start_OnGetStartedButtonClicked;
            start.OnCustomMessageBoxYesButtonClicked += Start_OnCustomMessageBoxYesButtonClicked;
            _controls.Add(Constants.ViewName.Start, start);

            FAQ faq = new FAQ { Name = Constants.ViewName.Faq };
            faq.OnOkButtonClicked += Faq_OnOkButtonClicked;
            _controls.Add(Constants.ViewName.Faq, faq);

            LearnMore learnmore = new LearnMore { Name = Constants.ViewName.LearnMore };
            learnmore.OnHowButtonClicked += LearnMore_OnHowButtonClicked;
            learnmore.OnSavingsButtonClicked += LearnMore_OnSavingsButtonClicked;
            learnmore.OnPerformanceButtonClicked += LearnMore_OnPerformanceButtonClicked;
            learnmore.OnEnvironmentButtonClicked += LearnMore_OnEnvironmentButtonClicked;
            learnmore.OnRecycleButtonClicked += LearnMore_OnRecycleButtonClicked;
            learnmore.OnBackButtonClicked += LearnMore_OnBackButtonClicked;
            _controls.Add(Constants.ViewName.LearnMore, learnmore);

            LearnMoreHow learnmorehow = new LearnMoreHow { Name = Constants.ViewName.LearnMoreHow };
            learnmorehow.OnStartButtonClicked += LearnMoreHow_OnStartButtonClicked;
            learnmorehow.OnBackButtonClicked += LearnMoreHow_OnBackButtonClicked;
            _controls.Add(Constants.ViewName.LearnMoreHow, learnmorehow);

            LearnMoreSavings learnmoresavings = new LearnMoreSavings { Name = Constants.ViewName.LearnMoreSavings };
            learnmoresavings.OnStartButtonClicked += LearnMoreSavings_OnStartButtonClicked;
            learnmoresavings.OnBackButtonClicked += LearnMoreSavings_OnBackButtonClicked;
            _controls.Add(Constants.ViewName.LearnMoreSavings, learnmoresavings);

            LearnMorePerformance learnmoreperformance = new LearnMorePerformance { Name = Constants.ViewName.LearnMorePerformance };
            learnmoreperformance.OnStartButtonClicked += LearnMorePerformance_OnStartButtonClicked;
            learnmoreperformance.OnBackButtonClicked += LearnMorePerformance_OnBackButtonClicked;
            _controls.Add(Constants.ViewName.LearnMorePerformance, learnmoreperformance);

            LearnMoreEnvironment learnmoreenvironment = new LearnMoreEnvironment { Name = Constants.ViewName.LearnMoreEnvironment };
            learnmoreenvironment.OnStartButtonClicked += LearnMoreEnvironment_OnStartButtonClicked;
            learnmoreenvironment.OnBackButtonClicked += LearnMoreEnvironment_OnBackButtonClicked;
            _controls.Add(Constants.ViewName.LearnMoreEnvironment, learnmoreenvironment);

            Terms termCondition = new Terms { Name = Constants.ViewName.Terms };
            termCondition.OnIAcceptButtonCliked += Terms_OnIAcceptButtonCliked;
            termCondition.OnCancelButtonCliked += Terms_OnCancelButtonCliked;
            _controls.Add(Constants.ViewName.Terms, termCondition);

            AnonymousTerms anonymousTermCondition = new AnonymousTerms { Name = Constants.ViewName.AnonymousTerms };
            anonymousTermCondition.OnIAcceptButtonCliked += AnonymousTerms_OnIAcceptButtonCliked;
            anonymousTermCondition.OnCancelButtonCliked += AnonymousTerms_OnCancelButtonCliked;
            _controls.Add(Constants.ViewName.AnonymousTerms, anonymousTermCondition);

            Privacy privacy = new Privacy { Name = Constants.ViewName.Privacy };
            privacy.OnOkButtonClicked += Privacy_OnOkButtonClicked;
            _controls.Add(Constants.ViewName.Privacy, privacy);

            MembershipSubscription membershipSubcription = new MembershipSubscription { Name = Constants.ViewName.MembershipSubscription };
            membershipSubcription.OnDoneButtonClicked += MembershipSubscription_OnDoneButtonClicked;
            membershipSubcription.OnCancelClicked += MembershipSubcriptionOnCancelClicked;
            _controls.Add(Constants.ViewName.MembershipSubscription, membershipSubcription);

            MembershipRegistration membershipRegistration = new MembershipRegistration { Name = Constants.ViewName.MembershipRegistration };
            membershipRegistration.OnDoneButtonClicked += MembershipRegistration_OnDoneButtonClicked;
            membershipRegistration.OnSubcriptionPlanClicked += MembershipRegistrationOnSubcriptionPlanClicked;
            membershipRegistration.OnCancelClicked += MembershipRegistrationOnCancelClicked;
            membershipRegistration.OnInputFieldChanged += MembershipRegistration_OnInputFieldChanged;
            _controls.Add(Constants.ViewName.MembershipRegistration, membershipRegistration);

            MembershipVerification membershipVerification = new MembershipVerification { Name = Constants.ViewName.MembershipVerification };
            membershipVerification.OnSwipeCard += MembershipVerification_OnSwipeCard;
            _controls.Add(Constants.ViewName.MembershipVerification, membershipVerification);

            ErrorMessage errorMessage = new ErrorMessage { Name = Constants.ViewName.ErrorMessage };
            errorMessage.OnDoneButtonClicked += new EventHandler(ErrorMessage_OnDoneButtonClicked);
            _controls.Add(Constants.ViewName.ErrorMessage, errorMessage);

            Login login = new Login { Name = Constants.ViewName.Login };
            login.OnSigninButtonClicked += Login_OnSigninButtonClicked;
            login.OnSignupButtonClicked += Login_OnSignupButtonClicked;
            login.OnCancelClicked += Login_OnCancelClicked;
            _controls.Add(Constants.ViewName.Login, login);

            GetBatteries getBetteries = new GetBatteries { Name = Constants.ViewName.GetBatteries };
            getBetteries.OnDoneButtonClicked += GetBatteries_OnDoneButtonClicked;
            _controls.Add(Constants.ViewName.GetBatteries, getBetteries);

            Exchange exchange = new Exchange { Name = Constants.ViewName.Exchange };
            exchange.OnDoneButtonClicked += Exchange_OnDoneButtonClicked;
            _controls.Add(Constants.ViewName.Exchange, exchange);

            Recycle recycle = new Recycle { Name = Constants.ViewName.Recycle };
            recycle.OnDoneButtonClicked += Recycle_OnDoneButtonClicked;
            _controls.Add(Constants.ViewName.Recycle, recycle);

            EmailReceipt emailReceipt = new EmailReceipt { Name = Constants.ViewName.EmailReceipt };
            emailReceipt.OnDoneButtonClicked += EmailReceipt_OnDoneButtonClicked;
            emailReceipt.OnSkipButtonClicked += EmailReceipt_OnSkipButtonClicked;
            _controls.Add(Constants.ViewName.EmailReceipt, emailReceipt);

            Checkout checkout = new Checkout { Name = Constants.ViewName.Checkout };
            checkout.OnSwipeCard += Checkout_OnSwipeCard;
            _controls.Add(Constants.ViewName.Checkout, checkout);

            ZipCode zipCode = new ZipCode { Name = Constants.ViewName.ZipCode };
            zipCode.OnDoneButtonClicked += ZipCode_OnDoneButtonClicked;
            _controls.Add(Constants.ViewName.ZipCode, zipCode);

            ThankYou thankYou = new ThankYou { Name = Constants.ViewName.ThankYou };
            thankYou.OnTimeout += ThankYou_OnTimeout;
            thankYou.OnDoneButtonClicked += ThankYou_OnDoneButtonClicked;
            _controls.Add(Constants.ViewName.ThankYou, thankYou);

            TransactionSummary transactionSummary = new TransactionSummary();
            transactionSummary.OnDiscountCodeClicked += TransactionSummary_OnDiscountCodeClicked;
            transactionSummary.OnDoneClicked += TransactionSummaryOnDoneClicked;
            transactionSummary.OnBackClicked += TransactionSummaryOnBackClicked;
            _controls.Add(Constants.ViewName.TransactionSummary, transactionSummary);

            CreditSwap creditSwap = new CreditSwap { Name = Constants.ViewName.CreditSwap };
            creditSwap.OnAdditionalBatteriesButtonClicked += CreditSwap_OnAdditionalBatteriesButtonClicked;
            creditSwap.OnKeepDepositButtonClicked += CreditSwap_OnKeepDepositButtonClicked;
            creditSwap.OnCreateCustomerAccountButtonClicked += CreditSwap_OnCreateCustomerAccountButtonClicked;
            creditSwap.OnSkipButtonClicked += CreditSwap_OnSkipButtonClicked;
            _controls.Add(Constants.ViewName.CreditSwap, creditSwap);

            GetCase getCase = new GetCase();
            getCase.OnDoneButtonClicked += GetCase_OnDoneButtonClicked;
            _controls.Add(Constants.ViewName.GetCase, getCase);

            PromoCodes promoCodes = new PromoCodes();
            promoCodes.OnPromoCodeChanged += PromoCodes_OnPromoCodeChanged;
            promoCodes.OnDoneButtonClicked += PromoCodes_OnDoneButtonClicked;
            promoCodes.OnCancelButtonClicked += PromoCodes_OnCancelButtonClicked;
            _controls.Add(Constants.ViewName.PromoCodes, promoCodes);

            UserProfile userProfile = new UserProfile();
            userProfile.OnContinueTransactionClicked += UserProfile_OnContinueTransactionClicked;
            userProfile.OnCancelTransactionClicked += UserProfile_OnCancelTransactionClicked;
            userProfile.OnChangeSubscriptionPlanClicked += UserProfile_OnChangeSubscriptionPlanClicked;
            _controls.Add(Constants.ViewName.UserProfile, userProfile);

            InventoryAdmin inventoryAdmin = new InventoryAdmin();
            inventoryAdmin.OnDoneButton_Clicked += InventoryAdmin_OnDoneClicked;
            _controls.Add(Constants.ViewName.InventoryAdmin, inventoryAdmin);

            CardChoice cardChoice = new CardChoice();
            cardChoice.OnNewCard_Clicked += CardChoice_OnNewCardClicked;
            cardChoice.OnExistingCard_Clicked += CardChoice_OnExistingCardClicked;
            _controls.Add(Constants.ViewName.CardChoice, cardChoice);

            AddCardToAccount addCardToAccount = new AddCardToAccount();
            addCardToAccount.OnSaveCard_Clicked += AddCardToAccount_OnSaveCardClicked;
            addCardToAccount.OnDontSaveCard_Clicked += AddCardToAccount_OnDontSaveCardClicked;
            _controls.Add(Constants.ViewName.AddCardToAccount, addCardToAccount);

        }

        /// <summary>
        /// Navigates to user control.
        /// </summary>
        /// <param name="controlName">Name of the control.</param>
        private void NavigateToView(string controlName, object data)
        {
            PopupCountDown.ResetPopup();

            BaseController.PreviousView = BaseController.CurrentView;
            BaseController.CurrentView = controlName;
            BaseController.UpdateRecentViewOfCurrentFlow(controlName);

            MainContentBorder.Child = null;
            UserControl control = _controls[controlName];
            MainContentBorder.Child = control;
            NavigationButtonPopulate();

            switch (controlName)
            {
                case Constants.ViewName.Welcome:
                    {
                        Welcome welcome = (Welcome)control;
                        welcome.Load();

                        ShowHeaderAndFooter();
                        ShowNavigationPanel();

                        break;
                    }
                case Constants.ViewName.Selection:
                    {
                        Selection selection = (Selection)control;
                        selection.Load();

                        ShowHeaderAndFooter();
                        ShowNavigationPanel();

                        break;
                    }
                case Constants.ViewName.LearnMore:
                    {
                        LearnMore learnmore = (LearnMore)control;
                        learnmore.Load();

                        ShowHeaderAndFooter();
                        ShowNavigationPanel();

                        break;
                    }
                case Constants.ViewName.LearnMoreHow:
                    {
                        LearnMoreHow learnmorehow = (LearnMoreHow)control;
                        learnmorehow.Load();

                        ShowHeaderAndFooter();
                        ShowNavigationPanel();

                        break;
                    }
                case Constants.ViewName.LearnMoreSavings:
                    {
                        LearnMoreSavings learnmoresavings = (LearnMoreSavings)control;
                        learnmoresavings.Load();

                        ShowHeaderAndFooter();
                        ShowNavigationPanel();

                        break;
                    }
                case Constants.ViewName.LearnMorePerformance:
                    {
                        LearnMorePerformance learnmoreperformance = (LearnMorePerformance)control;
                        learnmoreperformance.Load();

                        ShowHeaderAndFooter();
                        ShowNavigationPanel();

                        break;
                    }
                case Constants.ViewName.LearnMoreEnvironment:
                    {
                        LearnMoreEnvironment learnmoreenvironment = (LearnMoreEnvironment)control;
                        learnmoreenvironment.Load();

                        ShowHeaderAndFooter();
                        ShowNavigationPanel();

                        break;
                    }  
                case Constants.ViewName.Confirmation:
                    {
                        Confirmation confirmation = (Confirmation)control;
                        confirmation.Load();

                        ShowHeaderAndFooter();
                        HideNavigationPanel();

                        break;
                    }
                case Constants.ViewName.Vending:
                    {
                        Vending vending = (Vending)control;
                        vending.Load();

                        Vending();

                        ShowHeaderAndFooter();
                        HideNavigationPanel();

                        break;
                    }
                case Constants.ViewName.VendingContinue:
                    {
                        VendingContinue vendingContinue = (VendingContinue)control;
                        vendingContinue.Load();

                        ShowHeaderAndFooter();
                        HideNavigationPanel();

                        break;
                    }

                case Constants.ViewName.CreditSwap:
                    {
                        CreditSwap creditSwap = (CreditSwap)control;
                        int batteriesCartridges;
                        bool hasCredit = SwapController.HasCredit(out batteriesCartridges);

                        if (hasCredit)
                        {
                            creditSwap.Load(batteriesCartridges);
                        }

                        ShowHeaderAndFooter();
                        ShowNavigationPanel();

                        break;
                    }
                case Constants.ViewName.ErrorMessage:
                    {
                        PopupCountDown.StopPopup();
                        HideHeaderAndFooter();

                        ErrorMessage errorMessage = (ErrorMessage)control;
                        errorMessage.Load();

                        break;
                    }
                case Constants.ViewName.Start:
                    {
                        PopupCountDown.StopPopup();
                        HideHeaderAndFooter();

                        Start start = (Start)control;
                        start.Load();

                        break;
                    }
                case Constants.ViewName.Privacy:
                    {
                        Privacy privacy = (Privacy)control;
                        privacy.Load();

                        ShowHeaderAndFooter();
                        ShowNavigationPanel();

                        break;
                    }
                case Constants.ViewName.Terms:
                    {
                        Terms terms = (Terms)control;
                        terms.Load();

                        ShowHeaderAndFooter();
                        ShowNavigationPanel();

                        break;
                    }
                case Constants.ViewName.AnonymousTerms:
                    {
                        AnonymousTerms terms = (AnonymousTerms)control;
                        terms.Load();

                        ShowHeaderAndFooter();
                        ShowNavigationPanel();

                        break;
                    }
                case Constants.ViewName.PromoCodes:
                    {
                        PromoCodes promoCodes = (PromoCodes)control;
                        promoCodes.Load();

                        ShowHeaderAndFooter();
                        ShowNavigationPanel();

                        break;
                    }
                case Constants.ViewName.MembershipSubscription:
                    {
                        MembershipSubscription membershipSubcription = (MembershipSubscription)control;
                        if (BaseController.LoggedOnUser != null)
                        {
                            int currentSubscriptionPlanId = BaseController.GetSubscriptionPlanId(BaseController.LoggedOnUser.BatteriesInPlan);
                            membershipSubcription.Load(currentSubscriptionPlanId);
                        }
                        else
                        {
                            membershipSubcription.Load(0);
                        }

                        ShowHeaderAndFooter();
                        ShowNavigationPanel();

                        break;
                    }
                case Constants.ViewName.MembershipRegistration:
                    {
                        MembershipRegistration membershipRegistration = (MembershipRegistration)control;

                        BetteryUser user = BaseController.RegistrationUser;
                        if (user == null)
                        {
                            membershipRegistration.Load();
                        }
                        else
                        {
                            membershipRegistration.Load(user.MemberFirstName, user.MemberLastName, user.Email, user.Password, user.Password, user.ZipCode, user.GetEmails);
                        }

                        ShowHeaderAndFooter();
                        ShowNavigationPanel();

                        break;
                    }
                case Constants.ViewName.Login:
                    {
                        Login login = (Login)control;
                        login.Load();

                        ShowHeaderAndFooter();
                        ShowNavigationPanel();

                        break;
                    }
                case Constants.ViewName.GetBatteries:
                    {
                        SetBusyIndicatorMessage();
                        BetteryBusyIndicator.IsBusy = true;
                        using (BackgroundWorker worker = new BackgroundWorker())
                        {
                            int? maxAaProduct = 0;
                            int? maxAaaProduct = 0;

                            worker.DoWork += (sender, args) =>
                                                 {
                                                     maxAaProduct = GetBatteriesController.GetMaxAaProduct();
                                                     maxAaaProduct = GetBatteriesController.GetMaxAaaProduct();
                                                 };

                            worker.RunWorkerCompleted += (sender, args) =>
                                                             {
                                                                 BetteryBusyIndicator.IsBusy = false;

                                                                 if (maxAaProduct.HasValue && maxAaaProduct.HasValue)
                                                                 {
                                                                     GetBatteries getBatteries = (GetBatteries)control;

                                                                     getBatteries.Load(maxAaProduct.Value, maxAaaProduct.Value);
                                                                 }
                                                             };

                            worker.RunWorkerAsync();
                        }

                        ShowHeaderAndFooter();
                        ShowNavigationPanel();

                        break;
                    }
                case Constants.ViewName.GetCase:
                    {
                        SetBusyIndicatorMessage();
                        BetteryBusyIndicator.IsBusy = true;
                        using (BackgroundWorker worker = new BackgroundWorker())
                        {
                            int maxEmptyCases = 0;
                            worker.DoWork += (sender, args) =>
                                                 {
                                                     maxEmptyCases = GetCaseController.GetMaxEmptyCases();
                                                 };

                            worker.RunWorkerCompleted += (sender, args) =>
                                                             {
                                                                 GetCase getCase = (GetCase)control;
                                                                 getCase.Load(maxEmptyCases);
                                                                 BetteryBusyIndicator.IsBusy = false;
                                                             };

                            worker.RunWorkerAsync();
                        }

                        ShowHeaderAndFooter();
                        ShowNavigationPanel();

                        break;
                    }
                case Constants.ViewName.Faq:
                    {
                        FAQ faq = (FAQ)control;
                        faq.Load();

                        ShowHeaderAndFooter();
                        ShowNavigationPanel();

                        break;
                    }
                case Constants.ViewName.Recycle:
                    {
                        Recycle recycle = (Recycle)control;
                        recycle.Load();

                        ShowHeaderAndFooter();
                        ShowNavigationPanel();

                        break;
                    }
                case Constants.ViewName.Exchange:
                    {
                        Exchange exchange = (Exchange)control;
                        SetBusyIndicatorMessage();
                        BetteryBusyIndicator.IsBusy = true;

                        using (BackgroundWorker worker = new BackgroundWorker())
                        {
                            worker.DoWork += (sender, args) =>
                                                 {
                                                     BaseController.OpenPhidget(3000);
                                                 };
                            worker.RunWorkerCompleted += (sender, args) =>
                            {
                                exchange.Load();

                                BetteryBusyIndicator.IsBusy = false;
                            };

                            worker.RunWorkerAsync();
                        }

                        ShowHeaderAndFooter();
                        ShowNavigationPanel();

                        break;
                    }
                case Constants.ViewName.EmailReceipt:
                    {
                        EmailReceipt emailReceipt = (EmailReceipt)control;
                        emailReceipt.Load(string.Empty);

                        ShowHeaderAndFooter();
                        HideNavigationPanel();

                        break;
                    }
                case Constants.ViewName.UserProfile:
                    {
                        UserProfile userProfile = (UserProfile)control;
                        userProfile.Load();

                        ShowHeaderAndFooter();
                        ShowNavigationPanel();

                        break;
                    }
                case Constants.ViewName.Checkout:
                    {
                        BaseController.CalcCharges();

                        long customerProfileId = 0, customerPaymentProfileId = 0;
                        bool result = false;

                        if (BaseController.LoggedOnUser != null)
                        {
                            result = long.TryParse(BaseController.LoggedOnUser.CustomerProfileId, out customerProfileId);
                            if (result)
                            {
                                result = long.TryParse(BaseController.LoggedOnUser.PaymentProfileId, out customerPaymentProfileId);
                            }
                        }

                        if (result && customerProfileId > 0 && customerPaymentProfileId > 0)
                        {
                            SetBusyIndicatorMessage();
                            string message = string.Empty;
                            using (BackgroundWorker worker = new BackgroundWorker())
                            {
                                worker.DoWork += (sender, args) =>
                                {
                                    message = CheckOutController.MembershipCheckout();
                                };

                                worker.RunWorkerCompleted += (sender, args) =>
                                {
                                    BetteryBusyIndicator.IsBusy = false;
                                    if (string.IsNullOrEmpty(message))
                                    {
                                        NavigateToView(Constants.ViewName.Vending, null);
                                    }
                                };

                                BetteryBusyIndicator.IsBusy = true;
                                worker.RunWorkerAsync();
                            }
                        }
                        else
                        {
                            if (BaseController.SelectedBettery.TotalAmount > 0)
                            {
                                Checkout checkout = (Checkout)control;
                                checkout.Load();

                                if (data != null && data is string && !string.IsNullOrEmpty(data.ToString()))
                                {
                                    checkout.ShowMessage(data.ToString());
                                }

                                ShowHeaderAndFooter();
                                ShowNavigationPanel();
                            }
                            else
                            {
                                CheckOutController.CheckOut(string.Empty, string.Empty);
                                if (BaseController.LoggedOnUser != null)
                                {
                                    NavigateToView(Constants.ViewName.Vending, null);
                                }
                                else
                                {
                                    NavigateToView(Constants.ViewName.EmailReceipt, null);
                                }
                            }
                        }


                        break;
                    }
                case Constants.ViewName.MembershipVerification:
                    {
                        MembershipVerification membershipVerification = (MembershipVerification)control;
                        membershipVerification.Load();

                        ShowHeaderAndFooter();
                        ShowNavigationPanel();

                        break;
                    }
                case Constants.ViewName.ZipCode:
                    {
                        ZipCode zipCode = (ZipCode)control;
                        zipCode.Load(string.Empty);

                        ShowHeaderAndFooter();
                        HideNavigationPanel();

                        break;
                    }
                case Constants.ViewName.ThankYou:
                    {
                        BaseController.CalcCharges();

                        ThankYou thankYou = (ThankYou)control;
                        thankYou.Load();

                        //Clear transaction data
                        BaseController.SelectedBettery = null;
                        BaseController.CartridgeInserted = false;
                        BaseController.PreviousView = string.Empty;
                        BaseController.CurrentTransaction = null;
                        BaseController.RegistrationUser = null;
                        BaseController.CardInfo = string.Empty;
                        BaseController.GetBatteriesMode = GetBatteriesModes.BuyNew;
                        BaseController.RecentViewOfCurrentFlow = string.Empty;

                        HideHeaderAndFooter();
                        HideNavigationPanel();

                        break;
                    }
                case Constants.ViewName.TransactionSummary:
                    {
                        TransactionSummary transactionSummary = (TransactionSummary)control;
                        transactionSummary.Load();

                        ShowHeaderAndFooter();
                        ShowNavigationPanel();

                        break;
                    }
                case Constants.ViewName.InventoryAdmin:
                    {
                        InventoryAdmin inventoryAdmin = (InventoryAdmin)control;
                        inventoryAdmin.Load();

                        ShowHeaderAndFooter();
                        ShowNavigationPanel();

                        break;

                    }
            }
        }

        /// <summary>
        /// Navigations the button populate.
        /// </summary>
        private void NavigationButtonPopulate()
        {
            if (BaseController.CurrentView == Constants.ViewName.Faq)
            {
                FAQ.Visibility = Visibility.Collapsed;
            }
            else
            {
                FAQ.Visibility = Visibility.Visible;
            }

            if (BaseController.CurrentView == Constants.ViewName.Privacy || BaseController.CurrentView == Constants.ViewName.Terms)
            {
                TermsPrivacy.Visibility = Visibility.Collapsed;
            }
            else
            {
                TermsPrivacy.Visibility = Visibility.Visible;
            }

            if (BaseController.CurrentView != Constants.ViewName.UserProfile && BaseController.LoggedOnUser != null)
            {
                ///ViewProfile.Visibility = Visibility.Visible;
                SignUp.Visibility = Visibility.Collapsed;
            }
            else
            {
                ///ViewProfile.Visibility = Visibility.Collapsed;
                SignUp.Visibility = Visibility.Visible;
            }
        }

        /// <summary>
        /// Sets the busy indicator message.
        /// </summary>
        /// <param name="message">The message.</param>
        private void SetBusyIndicatorMessage(string message)
        {
            UIHelper.DispatchThread(this, main => main.BusyIndicatorMessage.Text = message);
        }

        /// <summary>
        /// Sets the busy indicator message.
        /// </summary>
        private void SetBusyIndicatorMessage()
        {
            UIHelper.DispatchThread(this, main => main.BusyIndicatorMessage.Text = Constants.Messages.DefaultWaitPopup);
        }

        private string GetProductType(int ProductType)
        {
            switch (ProductType)
            {
                case 1:
                    return Constants.BetteryProduct.AaaDesc;
                case 2:
                    return Constants.BetteryProduct.AaDesc;
                case 3:
                    return Constants.BetteryProduct.CaseDesc;
            }
            return string.Empty;
        }

        /// <summary>
        /// Gets the inventory report as a string
        /// </summary>
        private string GetInventoryReport()
        {
            BinProduct binProduct = new BinProduct();
            StringBuilder strReport  = new StringBuilder();
                
            strReport.AppendFormat ("INVENTORY REPORT\n=================\nPRODUCT AVAILABLE:  Empty Cases = {0}, AA 4-Packs = {1}, AAA 4-Packs = {2}\n\nACTUAL INVENTORY BY BIN\n=======================\n", BaseDAL.GetTotalQuantitybyProduct(ProductTypes.Cartridge), BaseDAL.GetTotalQuantitybyProduct(ProductTypes.AA), BaseDAL.GetTotalQuantitybyProduct(ProductTypes.AAA));
            
            binProduct = BaseDAL.GetInventorybyBin(16);
            strReport.AppendFormat("BIN {0}, PRODUCT TYPE = {1}, PRODUCT COUNT = {2}, ENABLED = {3}, RESTOCK AMOUNT = {4}\n", 16, GetProductType(binProduct.ProductID), binProduct.Quantity.ToString(), binProduct.Enabled, 12-binProduct.Quantity);

            binProduct = BaseDAL.GetInventorybyBin(17);
            strReport.AppendFormat("BIN {0}, PRODUCT TYPE = {1}, PRODUCT COUNT = {2}, ENABLED = {3}, RESTOCK AMOUNT = {4}\n", 17, GetProductType(binProduct.ProductID), binProduct.Quantity.ToString(), binProduct.Enabled, 12 - binProduct.Quantity);

            binProduct = BaseDAL.GetInventorybyBin(18);
            strReport.AppendFormat("BIN {0}, PRODUCT TYPE = {1}, PRODUCT COUNT = {2}, ENABLED = {3}, RESTOCK AMOUNT = {4}\n", 18, GetProductType(binProduct.ProductID), binProduct.Quantity.ToString(), binProduct.Enabled, 12 - binProduct.Quantity);

            binProduct = BaseDAL.GetInventorybyBin(20);
            strReport.AppendFormat("BIN {0}, PRODUCT TYPE = {1}, PRODUCT COUNT = {2}, ENABLED = {3}, RESTOCK AMOUNT = {4}\n", 20, GetProductType(binProduct.ProductID), binProduct.Quantity.ToString(), binProduct.Enabled, 12 - binProduct.Quantity);

            binProduct = BaseDAL.GetInventorybyBin(21);
            strReport.AppendFormat("BIN {0}, PRODUCT TYPE = {1}, PRODUCT COUNT = {2}, ENABLED = {3}, RESTOCK AMOUNT = {4}\n", 21, GetProductType(binProduct.ProductID), binProduct.Quantity.ToString(), binProduct.Enabled, 12 - binProduct.Quantity);

            binProduct = BaseDAL.GetInventorybyBin(22);
            strReport.AppendFormat("BIN {0}, PRODUCT TYPE = {1}, PRODUCT COUNT = {2}, ENABLED = {3}, RESTOCK AMOUNT = {4}\n", 22, GetProductType(binProduct.ProductID), binProduct.Quantity.ToString(), binProduct.Enabled, 12 - binProduct.Quantity);

            binProduct = BaseDAL.GetInventorybyBin(23);
            strReport.AppendFormat("BIN {0}, PRODUCT TYPE = {1}, PRODUCT COUNT = {2}, ENABLED = {3}, RESTOCK AMOUNT = {4}\n", 23, GetProductType(binProduct.ProductID), binProduct.Quantity.ToString(), binProduct.Enabled, 12 - binProduct.Quantity);

            binProduct = BaseDAL.GetInventorybyBin(24);
            strReport.AppendFormat("BIN {0}, PRODUCT TYPE = {1}, PRODUCT COUNT = {2}, ENABLED = {3}, RESTOCK AMOUNT = {4}\n", 24, GetProductType(binProduct.ProductID), binProduct.Quantity.ToString(), binProduct.Enabled, 12 - binProduct.Quantity);

            binProduct = BaseDAL.GetInventorybyBin(25);
            strReport.AppendFormat("BIN {0}, PRODUCT TYPE = {1}, PRODUCT COUNT = {2}, ENABLED = {3}, RESTOCK AMOUNT = {4}\n", 25, GetProductType(binProduct.ProductID), binProduct.Quantity.ToString(), binProduct.Enabled, 12 - binProduct.Quantity);

            binProduct = BaseDAL.GetInventorybyBin(26);
            strReport.AppendFormat("BIN {0}, PRODUCT TYPE = {1}, PRODUCT COUNT = {2}, ENABLED = {3}, RESTOCK AMOUNT = {4}\n", 26, GetProductType(binProduct.ProductID), binProduct.Quantity.ToString(), binProduct.Enabled, 12 - binProduct.Quantity);

            binProduct = BaseDAL.GetInventorybyBin(27);
            strReport.AppendFormat("BIN {0}, PRODUCT TYPE = {1}, PRODUCT COUNT = {2}, ENABLED = {3}, RESTOCK AMOUNT = {4}\n", 27, GetProductType(binProduct.ProductID), binProduct.Quantity.ToString(), binProduct.Enabled, 12 - binProduct.Quantity);

            binProduct = BaseDAL.GetInventorybyBin(28);
            strReport.AppendFormat("BIN {0}, PRODUCT TYPE = {1}, PRODUCT COUNT = {2}, ENABLED = {3}, RESTOCK AMOUNT = {4}\n", 28, GetProductType(binProduct.ProductID), binProduct.Quantity.ToString(), binProduct.Enabled, 12 - binProduct.Quantity);

            return (strReport.ToString());

        }

        #endregion Init Controls

        #region Login

        /// <summary>
        /// Handles the OnSigninButtonClicked event of the Login control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected void Login_OnSigninButtonClicked(object sender, EventArgs e)
        {
            Login login = (Login)sender;
            SetBusyIndicatorMessage();
            BetteryBusyIndicator.IsBusy = true;

            using (BackgroundWorker worker = new BackgroundWorker())
            {
                bool? isSuccess = false;
                worker.DoWork += (s, args) =>
                                     {
                                         //AuthenticateUser
                                         isSuccess = LoginController.AuthenticateUser(login.BetteryUserName, login.BetteryPassword);
                                     };

                worker.RunWorkerCompleted += (s, args) =>
                                                  {
                                                      BetteryBusyIndicator.IsBusy = false;

                                                      // If success, navigate to next view
                                                      if (isSuccess == true)
                                                      {
                                                          if (!string.IsNullOrEmpty(BaseController.RecentViewOfCurrentFlow))
                                                          {
                                                              NavigateToView(BaseController.RecentViewOfCurrentFlow, null);
                                                          }
                                                          else
                                                          {
                                                              NavigateToView(Constants.ViewName.Selection, null);
                                                          }
                                                      }
                                                      else
                                                      {
                                                          login.SetAuthenticationFaith(Constants.Messages.LoginFaith);
                                                      }
                                                  };

                worker.RunWorkerAsync();
            }
        }

        #endregion Login

        #region Privacy

        /// <summary>
        /// Handles the OnOkClicked event of the Privacy control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        private void Privacy_OnOkButtonClicked(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(BaseController.RecentViewOfCurrentFlow))
            {
                NavigateToView(BaseController.RecentViewOfCurrentFlow, null);
            }
            else
            {
                NavigateToView(Constants.ViewName.Selection, null);
            }
        }

        #endregion Privacy

        #region FAQ

        /// <summary>
        /// Handles the OnOkClicked event of the Faq control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        private void Faq_OnOkButtonClicked(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(BaseController.RecentViewOfCurrentFlow))
            {
                NavigateToView(BaseController.RecentViewOfCurrentFlow, null);
            }
            else
            {
                NavigateToView(Constants.ViewName.Selection, null);
            }
        }

        #endregion FAQ

        #region LearnMore

        /// <summary>
        /// Handles the OnHowClicked event of the LearnMore control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        private void LearnMore_OnHowButtonClicked(object sender, EventArgs e)
        {
            if (ConfigurationManager.AppSettings["TestTransaction"] != "TRUE")
            {
                Logger.Log(EventLogEntryType.Information, "User Touched On LearnMoreHow button - in LIVE mode", BaseController.StationId);
            }
            NavigateToView(Constants.ViewName.LearnMoreHow, null);
        }

        /// <summary>
        /// Handles the OnSavingsClicked event of the LearnMore control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        private void LearnMore_OnSavingsButtonClicked(object sender, EventArgs e)
        {
            if (ConfigurationManager.AppSettings["TestTransaction"] != "TRUE")
            {
                Logger.Log(EventLogEntryType.Information, "User Touched On LearnMoreSavings button - in LIVE mode", BaseController.StationId);
            }
            NavigateToView(Constants.ViewName.LearnMoreSavings, null);
        }

        /// <summary>
        /// Handles the OnPerformanceClicked event of the LearnMore control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        private void LearnMore_OnPerformanceButtonClicked(object sender, EventArgs e)
        {
            if (ConfigurationManager.AppSettings["TestTransaction"] != "TRUE")
            {
                Logger.Log(EventLogEntryType.Information, "User Touched On LearnMorePerformance button - in LIVE mode", BaseController.StationId);
            }
            NavigateToView(Constants.ViewName.LearnMorePerformance, null);
        }
        
        /// <summary>
        /// Handles the OnEnvironmentClicked event of the LearnMore control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        private void LearnMore_OnEnvironmentButtonClicked(object sender, EventArgs e)
        {
            if (ConfigurationManager.AppSettings["TestTransaction"] != "TRUE")
            {
                Logger.Log(EventLogEntryType.Information, "User Touched On LearnMoreEnvironment button - in LIVE mode", BaseController.StationId);
            }
            NavigateToView(Constants.ViewName.LearnMoreEnvironment, null);
        }
        
        /// <summary>
        /// Handles the OnRecyclelicked event of the LearnMore control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        private void LearnMore_OnRecycleButtonClicked(object sender, EventArgs e)
        {
            if (ConfigurationManager.AppSettings["TestTransaction"] != "TRUE")
            {
                Logger.Log(EventLogEntryType.Information, "User Touched On LearnMoreRecycle button - in LIVE mode", BaseController.StationId);
            }
            NavigateToView(Constants.ViewName.Recycle, null);
        }

        /// <summary>
        /// Handles the OnBackClicked event of the LearnMore control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        private void LearnMore_OnBackButtonClicked(object sender, EventArgs e)
        {
            NavigateToView(Constants.ViewName.Selection, null);
        }


        #endregion LearnMore

        #region LearnMoreHow

        /// <summary>
        /// Handles the OnStartClicked event of the LearnMoreHow control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        private void LearnMoreHow_OnStartButtonClicked(object sender, EventArgs e)
        {
            NavigateToView(Constants.ViewName.Selection, null);
        }

        /// <summary>
        /// Handles the OnBackClicked event of the LearnMoreHow control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        private void LearnMoreHow_OnBackButtonClicked(object sender, EventArgs e)
        {
            NavigateToView(Constants.ViewName.LearnMore, null);
        }

        #endregion LearnMoreHow

        #region LearnMoreSavings

        /// <summary>
        /// Handles the OnStartClicked event of the LearnMoreSavings control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        private void LearnMoreSavings_OnStartButtonClicked(object sender, EventArgs e)
        {
            NavigateToView(Constants.ViewName.Selection, null);
        }

        /// <summary>
        /// Handles the OnBackClicked event of the LearnMoreSavings control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        private void LearnMoreSavings_OnBackButtonClicked(object sender, EventArgs e)
        {
            NavigateToView(Constants.ViewName.LearnMore, null);
        }

        #endregion LearnMoreSavings

        #region LearnMorePerformance

        /// <summary>
        /// Handles the OnStartClicked event of the LearnMorePerformance control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        private void LearnMorePerformance_OnStartButtonClicked(object sender, EventArgs e)
        {
            NavigateToView(Constants.ViewName.Selection, null);
        }

        /// <summary>
        /// Handles the OnBackClicked event of the LearnMorePerformance control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        private void LearnMorePerformance_OnBackButtonClicked(object sender, EventArgs e)
        {
            NavigateToView(Constants.ViewName.LearnMore, null);
        }

        #endregion LearnMorePerformance

        #region LearnMoreEnvironment

        /// <summary>
        /// Handles the OnStartClicked event of the LearnMoreEnvironment control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        private void LearnMoreEnvironment_OnStartButtonClicked(object sender, EventArgs e)
        {
            NavigateToView(Constants.ViewName.Selection, null);
        }

        /// <summary>
        /// Handles the OnBackClicked event of the LearnMoreEnvironment control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        private void LearnMoreEnvironment_OnBackButtonClicked(object sender, EventArgs e)
        {
            NavigateToView(Constants.ViewName.LearnMore, null);
        }

        #endregion LearnMoreEnvironment

        #region Welcome

        /// <summary>
        /// Handles the OnLearnMoreClicked event of the Welcome control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        private void WelcomeOnLearnMoreClicked(object sender, EventArgs e)
        {
            NavigateToView(Constants.ViewName.Faq, null);
        }

        /// <summary>
        /// Handles the OnGetBetteryBatteriesClicked event of the Welcome control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        private void WelcomeOnGetBetteryBatteriesClicked(object sender, EventArgs e)
        {
            NavigateToView(Constants.ViewName.Selection, null);
        }

        #endregion Welcome

        #region Selection

        /// <summary>
        /// Handles the OnYesButtonClicked event of the Selection control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        protected void Selection_OnYesButtonClicked(object sender, EventArgs e)
        {
            if (ConfigurationManager.AppSettings["TestTransaction"] != "TRUE")
            {
                Logger.Log(EventLogEntryType.Information, "User Touched On SWAP Button - in LIVE mode", BaseController.StationId);
            }
            NavigateToView(Constants.ViewName.Exchange, null);
        }

        /// <summary>
        /// Handles the OnNoButtonClicked event of the Selection control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        protected void Selection_OnNoButtonClicked(object sender, EventArgs e)
        {
            if (ConfigurationManager.AppSettings["TestTransaction"] != "TRUE")
            {
                Logger.Log(EventLogEntryType.Information, "User Touched On BUY Button - in LIVE mode", BaseController.StationId);
            }
            BaseController.GetBatteriesMode = GetBatteriesModes.BuyNew;
            NavigateToView(Constants.ViewName.GetBatteries, null);
        }

        /// <summary>
        /// Handles the OnGetCaseButtonButtonClicked event of the Selection control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        private void Selection_OnGetCaseButtonButtonClicked(object sender, EventArgs e)
        {
            NavigateToView(Constants.ViewName.GetCase, null);
        }

        /// <summary>
        /// Handles the OnLearMoreButtonButtonClicked event of the Selection control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        private void Selection_OnLearnMoreButtonButtonClicked(object sender, EventArgs e)
        {
            if (ConfigurationManager.AppSettings["TestTransaction"] != "TRUE")
            {
                Logger.Log(EventLogEntryType.Information, "User Touched On LearnMoreHome Button - in LIVE mode", BaseController.StationId);
            }
            NavigateToView(Constants.ViewName.LearnMore, null);
        }

        /// <summary>
        /// Handles the OnRecycleButtonButtonClicked event of the Selection control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        private void Selection_OnRecycleButtonButtonClicked(object sender, EventArgs e)
        {
            NavigateToView(Constants.ViewName.Recycle, null);
        }

        /// <summary>
        /// Handles the OnForgotDrainedBatterieButtonClicked event of the Selection control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        private void Selection_OnForgotDrainedBatterieButtonClicked(object sender, EventArgs e)
        {
            //BaseController.GetBatteriesMode = GetBatteriesModes.ForgotDrained;
            //NavigateToView(Constants.ViewName.GetBatteries, null);
        }

        #endregion Selection

        #region Confirmation

        /// <summary>
        /// Handles the OnOkClicked event of the Confirmation control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs" /> instance containing the event data.</param>
        private void ConfirmationOnOkClicked(object sender, RoutedEventArgs e)
        {
            BaseController.Logout();
            NavigateToView(Constants.ViewName.Start, null);
        }

        /// <summary>
        /// Handles the OnCancelClicked event of the Confirmation control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs" /> instance containing the event data.</param>
        private void ConfirmationOnCancelClicked(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(BaseController.RecentViewOfCurrentFlow))
            {
                NavigateToView(BaseController.RecentViewOfCurrentFlow, null);
            }
            else
            {
                NavigateToView(Constants.ViewName.Selection, null);
            }
        }

        #endregion Confirmation

        #region Start

        /// <summary>
        /// Handles the OnGetStartedButtonClicked event of the Start control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected void Start_OnGetStartedButtonClicked(object sender, EventArgs e)
        {
            bool canConnect = true;
            string errorMessage = string.Empty;

            if (ConfigurationManager.AppSettings["TestTransaction"] != "TRUE")
            {
                Logger.Log(EventLogEntryType.Information, "User Touched On Attract Screen - in LIVE mode", BaseController.StationId);
            }

            using (BackgroundWorker worker = new BackgroundWorker())
            {
                worker.DoWork += (s, args) =>
                                     {
                                         //
                                         // Try the cell modem 3 times before failing
                                         //
                                         for (int i = 0; i < 3; i++)
                                         {
                                             canConnect = AlertController.PingService();
                                             if (canConnect) break;
                                             Logger.Log(EventLogEntryType.Warning, "Cell Modem Not Ready on Start, Retrying", BaseController.StationId);
                                             //
                                             // Sleep for 5 seconds to give the modem a chance to come up.   This will only happen if the first ping fails (very rare)
                                             //
                                             System.Threading.Thread.Sleep(5000);
                                         }
                                         if (canConnect)
                                         {
                                             canConnect = StartController.GetOutOfBatteryMessage(out errorMessage);
                                         }
                                     };

                worker.RunWorkerCompleted += (o, args) =>
                {
                    BetteryBusyIndicator.IsBusy = false;

                    if (canConnect)
                    {
                        if (string.IsNullOrEmpty(errorMessage))
                        {
                            ///Check to see of the serial port and GVC is ready.
                            if (!BaseController.SerialPortReady())
                            {
                                Logger.Log(EventLogEntryType.Error, "Serial Port Not Ready on User Start - Error", BaseController.StationId);
                                AlertController.TransactionFailureAlert("Serial Port Not Ready on User Start - Error");
                                NavigateToView(Constants.ViewName.ErrorMessage, null);
                            }
                            //
                            // otherwise, proceed with transaction
                            //
                            else
                            {
                                NavigateToView(Constants.ViewName.Selection, null);
                            }
                        }
                        else
                        {
                            ((Start)_controls[Constants.ViewName.Start]).ShowMessage(errorMessage);
                        }
                    }
                    else
                    {
                        // Log the failure and see if LogMeIn will pick it up after the cell modem is available
                        Logger.Log(EventLogEntryType.Error, "Cell Modem Error Detected on User Start - Error", BaseController.StationId);
                        //
                        // Will the alert controller fail if no cell modem available?
                        //
                        //AlertController.TransactionFailureAlert("Cell Modem Error Detected on User Start - Error");
                        NavigateToView(Constants.ViewName.ErrorMessage, null);
                    }
                };

                BetteryBusyIndicator.IsBusy = true;
                worker.RunWorkerAsync();
            }
        }

        /// <summary>
        /// Handles the OnCustomMessageBoxYesButtonClicked event of the Start control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected void Start_OnCustomMessageBoxYesButtonClicked(object sender, EventArgs e)
        {
            NavigateToView(Constants.ViewName.Selection, null);
        }

        #endregion Start

        #region Exchange

        /// <summary>
        /// Handles the OnDoneButtonClicked event of the Exchange control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected void Exchange_OnDoneButtonClicked(object sender, EventArgs e)
        {
            BaseController.ClosePhidget();

            BaseController.GetBatteriesMode = GetBatteriesModes.BuyNew;
            NavigateToView(Constants.ViewName.GetBatteries, null);
        }

        #endregion Exchange

        #region Recycle

        /// <summary>
        /// Handles the OnDoneButtonClicked event of the Exchange control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected void Recycle_OnDoneButtonClicked(object sender, EventArgs e)
        {
            NavigateToView(Constants.ViewName.Selection, null);
        }

        #endregion Recycle


        #region GetBatteries

        /// <summary>
        /// Gets the batteries_ on done button clicked.
        /// </summary>
        /// <param name="e">The e.</param>
        protected void GetBatteries_OnDoneButtonClicked(BetteryVend e)
        {
            bool hasCredit = SwapController.HasCredit();
            if (hasCredit)
            {
                NavigateToView(Constants.ViewName.CreditSwap, null);
            }
            else
            {
                NavigateToView(Constants.ViewName.TransactionSummary, null);
            }
        }

        #endregion GetBatteries

        #region GetCase

        /// <summary>
        /// Gets the case_ on done button clicked.
        /// </summary>
        /// <param name="emptyCases">The empty cases.</param>
        private void GetCase_OnDoneButtonClicked(int emptyCases)
        {
            SetBusyIndicatorMessage();
            BetteryBusyIndicator.IsBusy = true;
            using (BackgroundWorker worker = new BackgroundWorker())
            {
                worker.DoWork += (sender, args) =>
                {
                    VendingController.VendEmptyCase(emptyCases);
                };

                worker.RunWorkerCompleted += (sender, args) =>
                {
                    BetteryBusyIndicator.IsBusy = false;
                    NavigateToView(Constants.ViewName.Exchange, null);
                };

                worker.RunWorkerAsync();
            }
        }

        /// <summary>
        /// Vendings the controller_ on vending case.
        /// </summary>
        /// <param name="e">The <see cref="Bettery.Kiosk.Entities.VendEventArgs"/> instance containing the event data.</param>
        private void VendingController_OnVendingCase(VendEventArgs e)
        {
            if (BaseController.LoggedOnUser != null)
            {
                BaseController.LoggedOnUser.FreeCasesRemaining--;
            }
            string message = string.Format(Constants.Messages.VendingEmptyCasesMessage, e.VendedEmptyCases, e.TotalVendingEmptyCases);
            SetBusyIndicatorMessage(message);
        }

        #endregion GetCase

        #region Vending Page

        /// <summary>
        /// Vendings this instance.
        /// </summary>
        protected void Vending()
        {            
            bool isSuccess = true;
            
            using (BackgroundWorker worker = new BackgroundWorker())
            {
                worker.DoWork += (s, args) =>
                    {
                        //
                        // CK need to figure out a better way to handle a failure here...the value is overused below.
                        //
                        isSuccess = VendingController.Vending();

                        //  Reset popup on every command sent, as this is a proxy for a user action
                        //  Helps ensure we don't time out on multiple vends.
                        PopupCountDown.ResetPopup();

                        //
                        // On the last vend of a series of vends, log the results and send to the back end
                        // To improve the reliability of this, we should have logged and set the transaction to the queue BEFORE starting vending
                        // that would have recorded the transaction, even if the vending failed.  We should improve that.
                        //
                        if (isSuccess && BaseController.SelectedBettery.AaVendRemaining == 0 && BaseController.SelectedBettery.AaaVendRemaining == 0)
                        {
                        
                            if (BaseController.LoggedOnUser != null && BaseController.CurrentTransaction != null)
                            {

                                //
                                //  Log the transaction to the EventViewer and LogMeIn
                                //
                                StringBuilder sb = new StringBuilder();
                                sb.Append("Posting Record:\n");
                                sb.Append("MemberID= " + (BaseController.LoggedOnUser.MemberId == null ? string.Empty : BaseController.LoggedOnUser.MemberId.ToString()) + "\n");
                                sb.Append("SubTotalAmount= " + (BaseController.CurrentTransaction.SubTotalAmount == null ? string.Empty : BaseController.CurrentTransaction.SubTotalAmount.ToString()) + "\n");
                                sb.Append("ChargeAmount= " + (BaseController.CurrentTransaction.ChargeAmount == null ? string.Empty : BaseController.CurrentTransaction.ChargeAmount.ToString()) + "\n");
                                sb.Append("TaxAmount= " + (BaseController.CurrentTransaction.TaxAmount == null ? string.Empty : BaseController.CurrentTransaction.TaxAmount.ToString()) + "\n");
                                sb.Append("NameOnCard= " + (BaseController.CurrentTransaction.NameOnCard == null ? string.Empty : BaseController.CurrentTransaction.NameOnCard.ToString()) + "\n");
                                sb.Append("CardInfo= " + (BaseController.CurrentTransaction.CardInfo == null ? string.Empty : BaseController.CurrentTransaction.CardInfo.ToString()) + "\n");
                                sb.Append("email= " + (BaseController.LoggedOnUser.UserName == null ? string.Empty : BaseController.LoggedOnUser.UserName) + "\n");
                                sb.Append("CustomerProfileId= " + (BaseController.CurrentTransaction.CustomerProfileId == null ? string.Empty : BaseController.CurrentTransaction.CustomerProfileId.ToString()) + "\n");
                                sb.Append("AAVend= " + (BaseController.CurrentTransaction.AaVend == null ? string.Empty : BaseController.CurrentTransaction.AaVend.ToString()) + "\n");
                                sb.Append("AAAVend= " + (BaseController.CurrentTransaction.AaaVend == null ? string.Empty : BaseController.CurrentTransaction.AaaVend.ToString()) + "\n");
                                sb.Append("AaReturn= " + (BaseController.CurrentTransaction.AaReturn == null ? string.Empty : BaseController.CurrentTransaction.AaReturn.ToString()) + "\n");
                                sb.Append("OrderNumber= " + (BaseController.CurrentTransaction.OrderNumber == null ? string.Empty : BaseController.CurrentTransaction.OrderNumber.ToString()) + "\n");
                                sb.Append("Authorization= " + (BaseController.CurrentTransaction.Authorization == null ? string.Empty : BaseController.CurrentTransaction.Authorization.ToString()) + "\n");
                                sb.Append("PromoCode= " + (BaseController.CurrentTransaction.PromoCode == null ? string.Empty : BaseController.CurrentTransaction.PromoCode.ToString()) + "\n");
                                sb.Append("PromoCodeAmount= " + (BaseController.CurrentTransaction.PromoCodeAmount == null ? string.Empty : BaseController.CurrentTransaction.PromoCodeAmount.ToString()) + "\n");

                                Logger.Log(EventLogEntryType.Information, sb.ToString(), BaseController.StationId);

                                //
                                //  Post the transaction to the MSFT Azure back end, for logged on users.    For non logged on users
                                //  this is done in the EmailReceipt_OnDoneButtonClicked Button clicked code.   Really should be in one place...
                                //
  
                                int retries = 3;
                                isSuccess = false;
                                while (!isSuccess)
                                {
                                    isSuccess = BaseController.TransactionQueueSend(BaseController.CurrentTransaction.TransactionID,
                                              BaseController.LoggedOnUser.MemberId,
                                              BaseController.CurrentTransaction.SubTotalAmount,
                                              BaseController.CurrentTransaction.ChargeAmount,
                                              BaseController.CurrentTransaction.TaxAmount,
                                              BaseController.CurrentTransaction.NameOnCard,
                                              BaseController.CurrentTransaction.CardInfo,
                                              BaseController.LoggedOnUser.UserName,
                                              BaseController.CurrentTransaction.CustomerProfileId,
                                              BaseController.CurrentTransaction.PaymentProfileID,
                                              BaseController.CurrentTransaction.CardHash,
                                              BaseController.CurrentTransaction.AaVend,
                                              BaseController.CurrentTransaction.AaaVend,
                                              BaseController.CurrentTransaction.AaReturn,
                                              BaseController.CurrentTransaction.AaaReturn,
                                              BaseController.CurrentTransaction.AaForgotVend,
                                              BaseController.CurrentTransaction.AaaForgotVend,
                                              BaseController.CurrentTransaction.OrderNumber,
                                              BaseController.CurrentTransaction.Authorization,
                                              BaseController.CurrentTransaction.PromoCode,
                                              BaseController.CurrentTransaction.PromoCodeAmount,
                                              BaseController.CurrentTransaction.BatteryPacksCheckedOut);

                                    if (isSuccess)
                                        break; // success!
                                    else
                                    {
                                        if (--retries == 0) break;
                                        Logger.Log(EventLogEntryType.Warning, "Failed to post transaction to BETTERY report server, retrying", BaseController.StationId);
                                        System.Threading.Thread.Sleep(5000);
                                    }
                                }
                                if (!isSuccess)
                                {
                                    Logger.Log(EventLogEntryType.Error, "Complete fail to post transaction to BETTERY report server", BaseController.StationId);
                                    //
                                    // Go ahead and vend anyway, we've logged the transaction in the event log
                                    //
                                    //BaseController.RaiseOnThrowExceptionEvent();
                                }

                                BaseController.CurrentTransaction = null;
                            }
                        }
                        
                    };

                worker.RunWorkerCompleted += (s, args) =>
                {
                    //if (isSuccess)
                    //{
                        if (BaseController.SelectedBettery.AaVendRemaining > 0 || BaseController.SelectedBettery.AaaVendRemaining > 0)
                        {
                            NavigateToView(Constants.ViewName.VendingContinue, null);
                            BetteryBusyIndicator.IsBusy = false;
                        }

                        else
                        {
                            NavigateToView(Constants.ViewName.ThankYou, null);
                            BetteryBusyIndicator.IsBusy = false;
                        }
                    //}
                    //
                    // What are we supposed to do if we don't have success?  Commented out that condition.
                    // If we had a failure, and have more batteries to vend, we should try to vend.
                    // If we had a failure, but don't have more batteries to vend, we should move to the thank you screen.   Above.
                    //
                };
                //
                //  Run the workers now.   First the DoWork runs, then the completed runs after that.
                //
                worker.RunWorkerAsync();
            }
        }

        /// <summary>
        /// Raises the <see cref="E:Vending" /> event.   In this case, we update the on screen message with the number of packs vended
        /// </summary>
        /// <param name="vendEventArgs">The <see cref="VendEventArgs" /> instance containing the event data.</param>
        private void OnVending(VendEventArgs vendEventArgs)
        {
            string message = string.Format(Constants.Messages.VendingMessage, vendEventArgs.CurrentProductAA + vendEventArgs.CurrentProductAAA + 1, vendEventArgs.TotalProductAA + vendEventArgs.TotalProductAAA);
            Vending vending = (Vending)_controls[Constants.ViewName.Vending];
            UIHelper.DispatchThread(vending, control => control.ShowMessage(message));
         }

        #endregion Vending Page
        
        #region Vending Continue Page

        /// <summary>
        /// Handles the OnDoneButtonClicked event of the ThankYou control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        private void VendingContinue_OnDoneButtonClicked(object sender, EventArgs e)
        {
            //
            //  If we have vended more batteries than the vending limit, and the user touches vend more, then we re-start the vending process for added packs
            //
            NavigateToView(Constants.ViewName.Vending, null);
        }

        #endregion

        #region Membership registration

        /// <summary>
        /// Handles the OnSignupButtonClicked event of the Login control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        public void Login_OnSignupButtonClicked(object sender, EventArgs e)
        {
            NavigateToView(Constants.ViewName.MembershipRegistration, null);
        }


        /// <summary>
        /// Handles the OnSignupButtonClicked event of the Login control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        public void Login_OnCancelClicked(object sender, EventArgs e)
        {
            NavigateToView(BaseController.RecentViewOfCurrentFlow, null);
        }

        
        /// <summary>
        /// Handles the OnDoneButtonClicked event of the MembershipRegistration control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="MembershipRegistration.OnSignUpEventArgs" /> instance containing the event data.</param>
        public void MembershipRegistration_OnDoneButtonClicked(object sender, MembershipRegistration.OnSignUpEventArgs e)
        {
            BaseController.RegistrationUser = new BetteryUser
            {
                MemberFirstName = e.FirstName,
                MemberLastName = e.LastName,
                Email = e.Email,
                ZipCode = e.ZipCode,
                Password = e.Password,
                GetEmails = e.GetSubscriptionPlan
            };

            SetBusyIndicatorMessage();
            BetteryBusyIndicator.IsBusy = true;
            bool isValidEmail = false;

            using (BackgroundWorker worker = new BackgroundWorker())
            {
                worker.DoWork += (s, args) =>
                {
                    isValidEmail = MembershipRegistrationController.CheckEmail(e.Email);
                };

                worker.RunWorkerCompleted += (o, args) =>
                {
                    BetteryBusyIndicator.IsBusy = false;

                    if (!isValidEmail)
                    {
                        MembershipRegistration membershipRegistration = (MembershipRegistration)_controls[BaseController.CurrentView];
                        membershipRegistration.ShowMessage(Constants.Messages.RegisteredEmailError, MembershipRegistration.ControlNames.Email);
                    }
                    else
                    {
                        NavigateToView(Constants.ViewName.Terms, null);
                    }
                };

                worker.RunWorkerAsync();
            }
        }

        /// <summary>
        /// Handles the OnSubcriptionPlanClicked event of the MembershipRegistration control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="MembershipRegistration.OnSignUpEventArgs" /> instance containing the event data.</param>
        private void MembershipRegistrationOnSubcriptionPlanClicked(object sender, MembershipRegistration.OnSignUpEventArgs e)
        {
            BaseController.RegistrationUser = new BetteryUser
            {
                MemberFirstName = e.FirstName,
                MemberLastName = e.LastName,
                Email = e.Email,
                ZipCode = e.ZipCode,
                Password = e.Password,
                GetEmails = e.GetSubscriptionPlan
            };

            SetBusyIndicatorMessage();
            BetteryBusyIndicator.IsBusy = true;
            bool isValidEmail = false;

            using (BackgroundWorker worker = new BackgroundWorker())
            {
                worker.DoWork += (s, args) =>
                                     {
                                         isValidEmail = MembershipRegistrationController.CheckEmail(e.Email);
                                     };

                worker.RunWorkerCompleted += (o, args) =>
                                                 {
                                                     BetteryBusyIndicator.IsBusy = false;

                                                     if (!isValidEmail)
                                                     {
                                                         MembershipRegistration membershipRegistration = (MembershipRegistration)_controls[BaseController.CurrentView];
                                                         membershipRegistration.ShowMessage(Constants.Messages.RegisteredEmailError, MembershipRegistration.ControlNames.Email);
                                                     }
                                                     else
                                                     {
                                                         NavigateToView(Constants.ViewName.MembershipSubscription, null);
                                                     }
                                                 };

                worker.RunWorkerAsync();
            }
        }

        /// <summary>
        /// Handles the OnCancelClicked event of the MembershipRegistration control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        /// <exception cref="System.NotImplementedException"></exception>
        private void MembershipRegistrationOnCancelClicked(object sender, EventArgs e)
        {
            BaseController.RegistrationUser = null;
            if (!string.IsNullOrEmpty(BaseController.RecentViewOfCurrentFlow))
            {
                NavigateToView(BaseController.RecentViewOfCurrentFlow, null);
            }
            else
            {
                NavigateToView(Constants.ViewName.Selection, null);
            }
        }

        /// <summary>
        /// Handles the OnInputFieldChanged event of the MembershipRegistration control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="MembershipRegistration.OnInputFieldChangedEventArgs"/> instance containing the event data.</param>
        public void MembershipRegistration_OnInputFieldChanged(object sender, MembershipRegistration.OnInputFieldChangedEventArgs e)
        {
        }

        /// <summary>
        /// Handles the OnVerifyButtonClicked event of the Login control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        public void Login_OnSubscriptionButtonClicked(object sender, EventArgs e)
        {
            NavigateToView(Constants.ViewName.MembershipSubscription, null);
        }

        #endregion Membership registration

        #region Membership verification

        /// <summary>
        /// Handles the OnSwipeCard event of the MembershipVerification control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="Bettery.Kiosk.UserControls.MembershipVerification.OnSwipeCardEventArgs"/> instance containing the event data.</param>
        public void MembershipVerification_OnSwipeCard(object sender, MembershipVerification.OnSwipeCardEventArgs e)
        {
            PopupCountDown.ResetPopup();
            BaseController.CardInfo = e.CardInfo;
            MembershipVerification membershipVerification = (MembershipVerification)_controls[Constants.ViewName.MembershipVerification];

            if (BaseController.RegistrationUser != null)
            {
                string errorMessage = string.Empty;
                bool? isSuccess = null;
                SetBusyIndicatorMessage();
                BetteryBusyIndicator.IsBusy = true;
                using (BackgroundWorker worker = new BackgroundWorker())
                {
                    worker.DoWork += (checOutSender, args) =>
                    {
                        isSuccess = MembershipRegistrationController.SaveRegistrationUser();

                        if (isSuccess == true)
                        {
                            errorMessage = CheckOutController.VerifyMembershipCredit(BaseController.CardInfo);

                            if (string.IsNullOrEmpty(errorMessage))
                            {
                                int newSubscriptionPlan = BaseController.RegistrationUser.BatteriesInPlan;
                                isSuccess = LoginController.AuthenticateUser(BaseController.RegistrationUser.Email, BaseController.RegistrationUser.Password);
                                
                                if (isSuccess == false)
                                {
                                    errorMessage = Constants.Messages.UserIsNotLoggedIn;
                                }
                                else
                                {
                                    MembershipRegistrationController.UpdateMembershipProfile(BaseController.LoggedOnUser.MemberId, BaseController.CurrentTransaction.CustomerProfileId, BaseController.CurrentTransaction.PaymentProfileID, newSubscriptionPlan, 0, false, false);
                                    BaseController.LoggedOnUser.CustomerProfileId = BaseController.CurrentTransaction.CustomerProfileId;
                                    BaseController.LoggedOnUser.PaymentProfileId = BaseController.CurrentTransaction.PaymentProfileID;
                                    BaseController.LoggedOnUser.BatteriesInPlan = newSubscriptionPlan;
                                }
                            }
                        }
                        else if (isSuccess == false)
                        {
                            errorMessage = Constants.Messages.UserIsNotRegistered;
                        }
                    };

                    worker.RunWorkerCompleted += (checOutSender, args) =>
                    {
                        BetteryBusyIndicator.IsBusy = false;
                        if (string.IsNullOrEmpty(errorMessage))
                        {
                            BaseController.RegistrationUser = null;
                            membershipVerification.ClearMessage();
                            NavigateToView(BaseController.RecentViewOfCurrentFlow, null);
                        }
                        else
                        {
                            membershipVerification.ShowMessage(errorMessage);
                            membershipVerification.Clear();
                        }
                    };

                    worker.RunWorkerAsync();
                }
            }
            else if (BaseController.LoggedOnUser != null)
            {
                SetBusyIndicatorMessage();
                string message = string.Empty;
                using (BackgroundWorker worker = new BackgroundWorker())
                {
                    worker.DoWork += delegate
                    {
                        message = CheckOutController.CheckOutForSubscriptionPlan(BaseController.CardInfo);
                       
                        LoginController.AuthenticateUser(BaseController.LoggedOnUser.Email, BaseController.LoggedOnUser.Password);
                    };

                    worker.RunWorkerCompleted += delegate
                    {
                        BetteryBusyIndicator.IsBusy = false;
                        if (string.IsNullOrEmpty(message))
                        {
                            //Todo: Pass a success message
                            NavigateToView(Constants.ViewName.UserProfile, null);
                        }
                    };

                    BetteryBusyIndicator.IsBusy = true;
                    worker.RunWorkerAsync();
                }
            }
        }

        #endregion Membership verification

        #region Membership Subscription

        /// <summary>
        /// Handles the OnDoneButtonClicked event of the MembershipSubscription control.
        /// </summary>
        /// <param name="batteriesInPlan">The batteries in plan.</param>
        protected void MembershipSubscription_OnDoneButtonClicked(int newBatteriesInPlanId)
        {
            int newBatteriesInPlan = BaseController.GetBetteryPacksInPlan(newBatteriesInPlanId);
            if (BaseController.RegistrationUser != null)
            {
                BaseController.RegistrationUser.BatteriesInPlan = newBatteriesInPlan;

                NavigateToView(Constants.ViewName.Terms, null);
            }
            else if (BaseController.LoggedOnUser != null)
            {
                BaseController.LoggedOnUser.NewBatteriesInPlan = newBatteriesInPlan;
                if (newBatteriesInPlanId > 0 && newBatteriesInPlan != BaseController.LoggedOnUser.BatteriesInPlan)
                {
                    long customerProfileId = 0, customerPaymentProfileId = 0;
                    bool result = false;

                    result = long.TryParse(BaseController.LoggedOnUser.CustomerProfileId, out customerProfileId);
                    if (result)
                    {
                        result = long.TryParse(BaseController.LoggedOnUser.PaymentProfileId, out customerPaymentProfileId);
                    }

                    if (result && customerProfileId > 0 && customerPaymentProfileId > 0)
                    {
                        SetBusyIndicatorMessage();
                        string message = string.Empty;
                        using (BackgroundWorker worker = new BackgroundWorker())
                        {
                            worker.DoWork += (sender, args) =>
                            {
                                message = CheckOutController.CheckOutForSubscriptionPlan(customerProfileId, customerPaymentProfileId);
                               
                                LoginController.AuthenticateUser(BaseController.LoggedOnUser.Email, BaseController.LoggedOnUser.Password);
                            };

                            worker.RunWorkerCompleted += (sender, args) =>
                            {
                                BetteryBusyIndicator.IsBusy = false;
                                if (string.IsNullOrEmpty(message))
                                {
                                    //Todo: Pass a success message
                                    NavigateToView(Constants.ViewName.UserProfile, null);
                                }
                            };

                            BetteryBusyIndicator.IsBusy = true;
                            worker.RunWorkerAsync();
                        }
                    }
                    else
                    {
                        BaseController.LoggedOnUser.NewBatteriesInPlan = newBatteriesInPlan;
                        NavigateToView(Constants.ViewName.MembershipVerification, null);
                    }
                }
                else
                {
                    NavigateToView(Constants.ViewName.UserProfile, null);
                    //Todo: Pass a Nothing Changed message
                }
            }
        }

        /// <summary>
        /// Handles the OnCancelClicked event of the MembershipSubcription control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        /// <exception cref="System.NotImplementedException"></exception>
        private void MembershipSubcriptionOnCancelClicked(object sender, EventArgs e)
        {
            if (BaseController.LoggedOnUser != null)
            {
                NavigateToView(Constants.ViewName.UserProfile, null);
            }
            else
            {
                NavigateToView(Constants.ViewName.MembershipRegistration, null);
                BaseController.RegistrationUser = null;
            }
        }

        #endregion Membership Subscription

        #region Checkout

        /// <summary>
        /// Handles the OnSwipeCard event of the Checkout control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="Checkout.OnSwipeCardEventArgs"/> instance containing the event data.</param>
        public void Checkout_OnSwipeCard(object sender, Checkout.OnSwipeCardEventArgs e)
        {
            PopupCountDown.ResetPopup();
            BaseController.CardInfo = e.CardInfo;
            Checkout checkout = (Checkout)_controls[Constants.ViewName.Checkout];

            if (BaseController.LoggedOnUser != null && BaseController.LoggedOnUser.PaymentProfileId != null)
            {
                string errorMessage = string.Empty;
                SetBusyIndicatorMessage();
                BetteryBusyIndicator.IsBusy = true;
                using (BackgroundWorker worker = new BackgroundWorker())
                {
                    worker.DoWork += (checOutSender, args) =>
                    {
                        errorMessage = CheckOutController.MembershipCheckout(BaseController.CardInfo);
                    };

                    worker.RunWorkerCompleted += (checOutSender, args) =>
                    {
                        BetteryBusyIndicator.IsBusy = false;
                        if (string.IsNullOrEmpty(errorMessage))
                        {
                            string zipcode = string.Empty;
                            if (BaseController.LoggedOnUser != null)
                            {
                                zipcode = BaseController.LoggedOnUser.ZipCode;
                            }

                            if (string.IsNullOrEmpty(zipcode))
                            {
                                NavigateToView(Constants.ViewName.ZipCode, null);
                            }
                            else
                            {
                                checkout.ClearMessage();
                                NavigateToView(Constants.ViewName.Vending, null);
                            }
                        }
                        else
                        {
                            checkout.ShowMessage(errorMessage);
                            checkout.Clear();
                        }
                    };

                    worker.RunWorkerAsync();
                }
            }
            else
            {
                string zipcode = string.Empty;
                if (BaseController.CurrentTransaction != null)
                {
                    zipcode = BaseController.CurrentTransaction.ZipCode;
                }

                if (string.IsNullOrEmpty(zipcode))
                {
                    NavigateToView(Constants.ViewName.ZipCode, null);
                }
                else
                {
                    BetteryBusyIndicator.IsBusy = true;
                    string errorMessage = string.Empty;
                    using (BackgroundWorker worker = new BackgroundWorker())
                    {
                        worker.DoWork += (obj, args) =>
                        {
                            errorMessage = CheckOutController.CheckOut(BaseController.CardInfo, zipcode);
                        };

                        worker.RunWorkerCompleted += (obj, args) =>
                        {
                            BetteryBusyIndicator.IsBusy = false;

                            if (string.IsNullOrEmpty(errorMessage))
                            {
                                checkout.ClearMessage();
                                if (BaseController.LoggedOnUser == null)
                                {
                                    NavigateToView(Constants.ViewName.EmailReceipt, null);
                                }
                                else
                                {
                                    NavigateToView(Constants.ViewName.Vending, null);
                                }
                            }
                            else
                            {
                                checkout.ShowMessage(errorMessage);
                                checkout.Clear();
                            }
                        };

                        worker.RunWorkerAsync();
                    }
                }
            }
        }

        #endregion Checkout

        #region EmailReceipt

        /// <summary>
        /// Emails the receipt_ on done button clicked.   Also called by the SKIP button method, but uses an empty
        /// email string.   This code not called for logged in members.   Also, unclear if it's called
        /// for zero-dollar transactions.   Need to check that.
        /// </summary>
        /// <param name="email">The email.</param>
        protected void EmailReceipt_OnDoneButtonClicked(string email)
        {
            SetBusyIndicatorMessage();
            BetteryBusyIndicator.IsBusy = true;
            bool isSuccess = false;
            using (BackgroundWorker worker = new BackgroundWorker())
            {
                worker.DoWork += delegate
                {
                    if (BaseController.CurrentTransaction != null)
                    {
                        int MemberID = 0;
                        if (email == null)
                            email = String.Empty;
                        if (BaseController.LoggedOnUser != null)
                            MemberID = BaseController.LoggedOnUser.MemberId;

                        //
                        //  This case should log every transaction for users that are not logged in, so long as they are asked for an email reciept...
                        //  Check to see if this code runs, even for $0.00 transactions...
                        //
                        //  Test commenting out this code, and having the logging always done in Vending, regardless of email receipt.  Sometimes this screen gets skipped
                        //  Zero $ transaction for non-logged in user?
                        //
                        StringBuilder sb = new StringBuilder();
                        sb.Append("Posting Record:\n");
                        sb.Append("MemberID= " + (MemberID == null ? string.Empty : MemberID.ToString()) + "\n");
                        sb.Append("SubTotalAmount= " + (BaseController.CurrentTransaction.SubTotalAmount == null ? string.Empty : BaseController.CurrentTransaction.SubTotalAmount.ToString()) + "\n");
                        sb.Append("ChargeAmount= " + (BaseController.CurrentTransaction.ChargeAmount == null ? string.Empty : BaseController.CurrentTransaction.ChargeAmount.ToString()) + "\n");
                        sb.Append("TaxAmount= " + (BaseController.CurrentTransaction.TaxAmount == null ? string.Empty : BaseController.CurrentTransaction.TaxAmount.ToString()) + "\n");
                        sb.Append("NameOnCard= " + (BaseController.CurrentTransaction.NameOnCard == null ? string.Empty : BaseController.CurrentTransaction.NameOnCard.ToString()) + "\n");
                        sb.Append("CardInfo= " + (BaseController.CurrentTransaction.CardInfo == null ? string.Empty : BaseController.CurrentTransaction.CardInfo.ToString()) + "\n");
                        sb.Append("email= " + (email == null ? string.Empty : email) + "\n");
                        sb.Append("CustomerProfileId= " + (BaseController.CurrentTransaction.CustomerProfileId == null ? string.Empty : BaseController.CurrentTransaction.CustomerProfileId.ToString()) + "\n");
                        sb.Append("AAVend= " + (BaseController.CurrentTransaction.AaVend == null ? string.Empty : BaseController.CurrentTransaction.AaVend.ToString()) + "\n");
                        sb.Append("AAAVend= " + (BaseController.CurrentTransaction.AaaVend == null ? string.Empty : BaseController.CurrentTransaction.AaaVend.ToString()) + "\n");
                        sb.Append("AaReturn= " + (BaseController.CurrentTransaction.AaReturn == null ? string.Empty : BaseController.CurrentTransaction.AaReturn.ToString()) + "\n");
                        sb.Append("OrderNumber= " + (BaseController.CurrentTransaction.OrderNumber == null ? string.Empty : BaseController.CurrentTransaction.OrderNumber.ToString()) + "\n");
                        sb.Append("Authorization= " + (BaseController.CurrentTransaction.Authorization == null ? string.Empty : BaseController.CurrentTransaction.Authorization.ToString()) + "\n");
                        sb.Append("PromoCode= " + (BaseController.CurrentTransaction.PromoCode == null ? string.Empty : BaseController.CurrentTransaction.PromoCode.ToString()) + "\n");
                        sb.Append("PromoCodeAmount= " + (BaseController.CurrentTransaction.PromoCodeAmount == null ? string.Empty : BaseController.CurrentTransaction.PromoCodeAmount.ToString()) + "\n");

                        Logger.Log(EventLogEntryType.Information, sb.ToString(), BaseController.StationId);     
 
                        int retries = 3;
                        while (!isSuccess)
                        {
                            isSuccess = BaseController.TransactionQueueSend(BaseController.CurrentTransaction.TransactionID,
                                      MemberID,
                                      BaseController.CurrentTransaction.SubTotalAmount,
                                      BaseController.CurrentTransaction.ChargeAmount,
                                      BaseController.CurrentTransaction.TaxAmount,
                                      BaseController.CurrentTransaction.NameOnCard,
                                      BaseController.CurrentTransaction.CardInfo,
                                      email,
                                      BaseController.CurrentTransaction.CustomerProfileId,
                                      BaseController.CurrentTransaction.PaymentProfileID,
                                      BaseController.CurrentTransaction.CardHash,
                                      BaseController.CurrentTransaction.AaVend,
                                      BaseController.CurrentTransaction.AaaVend,
                                      BaseController.CurrentTransaction.AaReturn,
                                      BaseController.CurrentTransaction.AaaReturn,
                                      BaseController.CurrentTransaction.AaForgotVend,
                                      BaseController.CurrentTransaction.AaaForgotVend,
                                      BaseController.CurrentTransaction.OrderNumber,
                                      BaseController.CurrentTransaction.Authorization,
                                      BaseController.CurrentTransaction.PromoCode,
                                      BaseController.CurrentTransaction.PromoCodeAmount,
                                      BaseController.CurrentTransaction.BatteryPacksCheckedOut);
         
                                if (isSuccess)
                                    break; // success!
                                else
                                {
                                    if (--retries == 0) break;
                                    Logger.Log(EventLogEntryType.Warning, "Failed to post transaction to BETTERY report server, retrying", BaseController.StationId);
                                    System.Threading.Thread.Sleep(5000);
                                }
                        }
                        if (!isSuccess)
                        {
                            Logger.Log(EventLogEntryType.Error, "Complete fail to post transaction to BETTERY report server", BaseController.StationId);
                            //
                            // Go ahead and vend anyway, we've logged the transaction in the event log
                            //
                            //BaseController.RaiseOnThrowExceptionEvent();
                        }

                    }
                };

                worker.RunWorkerCompleted += delegate
                {
                    BetteryBusyIndicator.IsBusy = false;
                    //
                    //  Even if the transaction queue send failed, if we are here, we've charged the customer.  So vend anyway.
                    //
                    //if (isSuccess)
                    //{
                        BaseController.CurrentTransaction = null;
                        NavigateToView(Constants.ViewName.Vending, null);
                    //}
                };

                worker.RunWorkerAsync();
            }
        }

        /// <summary>
        /// Handles the OnSkipButtonClicked event of the EmailReceipt control.    Just goes to the DoneButton anyway, but with an empty string
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs" /> instance containing the event data.</param>
        private void EmailReceipt_OnSkipButtonClicked(object sender, RoutedEventArgs e)
        {
            EmailReceipt_OnDoneButtonClicked(string.Empty);
        }

        #endregion EmailReceipt

        #region ZipCode

        /// <summary>
        /// Handles the OnDoneButtonClicked event of the ZipCode control.
        /// </summary>
        /// <param name="zipcode">The zipcode.</param>
        protected void ZipCode_OnDoneButtonClicked(string zipcode)
        {
            SetBusyIndicatorMessage();
            BetteryBusyIndicator.IsBusy = true;
            string errorMessage = string.Empty;
            using (BackgroundWorker worker = new BackgroundWorker())
            {
                worker.DoWork += (obj, args) =>
                                     {
                                         errorMessage = CheckOutController.CheckOut(BaseController.CardInfo, zipcode);
                                     };

                worker.RunWorkerCompleted += (obj, args) =>
                                                 {
                                                     BetteryBusyIndicator.IsBusy = false;

                                                     if (string.IsNullOrEmpty(errorMessage))
                                                     {
                                                         if (BaseController.LoggedOnUser == null)
                                                         {
                                                             NavigateToView(Constants.ViewName.EmailReceipt, null);
                                                         }
                                                         else
                                                         {
                                                             NavigateToView(Constants.ViewName.Vending, null);
                                                         }
                                                     }
                                                     else
                                                     {
                                                         NavigateToView(Constants.ViewName.Checkout, errorMessage);
                                                     }
                                                 };

                worker.RunWorkerAsync();
            }
        }

        #endregion ZipCode

        #region CreditSwap

        /// <summary>
        /// Handles the OnKeepDepositButtonClicked event of the CreditSwap control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void CreditSwap_OnKeepDepositButtonClicked(object sender, EventArgs e)
        {
            NavigateToView(Constants.ViewName.Checkout, null);
        }

        /// <summary>
        /// Handles the OnAdditionalBatteriesButtonClicked event of the CreditSwap control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void CreditSwap_OnAdditionalBatteriesButtonClicked(object sender, EventArgs e)
        {
            NavigateToView(Constants.ViewName.GetBatteries, null);
        }

        /// <summary>
        /// Handles the OnCreateCustomerAccountButtonClicked event of the CreditSwap control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void CreditSwap_OnCreateCustomerAccountButtonClicked(object sender, EventArgs e)
        {
            NavigateToView(Constants.ViewName.MembershipRegistration, null);
        }

        /// <summary>
        /// Handles the OnSkipButtonClicked event of the creditSwap control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void CreditSwap_OnSkipButtonClicked(object sender, EventArgs e)
        {
            NavigateToView(Constants.ViewName.TransactionSummary, null);
        }

        #endregion CreditSwap

        #region PromoCodes

        /// <summary>
        /// Promoes the codes_ on promo code changed.
        /// </summary>
        /// <param name="value">The value.</param>
        private void PromoCodes_OnPromoCodeChanged(string value)
        {
        }

        /// <summary>
        /// Promoes the codes_ on cancel button clicked.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        private void PromoCodes_OnCancelButtonClicked(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(BaseController.RecentViewOfCurrentFlow))
            {
                NavigateToView(BaseController.RecentViewOfCurrentFlow, null);
            }
            else
            {
                NavigateToView(Constants.ViewName.Selection, null);
            }
        }

        /// <summary>
        /// Promoes the codes_ on done button clicked.
        /// </summary>
        /// <param name="code">The code.</param>
        private void PromoCodes_OnDoneButtonClicked(string code)
        {
            SetBusyIndicatorMessage();
            BetteryBusyIndicator.IsBusy = true;
            bool isSuccess = false;
            bool isValidPromotionCode = false;
            string invalidReasonCode = String.Empty;
            using (BackgroundWorker worker = new BackgroundWorker())
            {
                worker.DoWork += (sender, args) =>
                                     {
                                         
                                         decimal? promotionalCredit = PromoCodesController.GetPromotionalAmount(code, out invalidReasonCode);
                                         if (promotionalCredit.HasValue)
                                         {
                                             isValidPromotionCode = promotionalCredit.Value > 0;

                                             if (isValidPromotionCode)
                                             {
                                                 if (BaseController.SelectedBettery == null)
                                                 {
                                                     BaseController.SelectedBettery = new BetteryVend();
                                                 }

                                                 BaseController.SelectedBettery.PromotionCode = code;
                                                 BaseController.SelectedBettery.PromotionalAmount = promotionalCredit.Value;
                                                 BaseController.CalcCharges();
                                             }

                                             isSuccess = true;
                                         }
                                     };

                worker.RunWorkerCompleted += (sender, args) =>
                                                 {
                                                     BetteryBusyIndicator.IsBusy = false;
                                                     if (isSuccess)
                                                     {
                                                         if (isValidPromotionCode)
                                                         {
                                                             if (
                                                                 !string.IsNullOrEmpty(
                                                                     BaseController.RecentViewOfCurrentFlow))
                                                             {
                                                                 NavigateToView(BaseController.RecentViewOfCurrentFlow, null);
                                                             }
                                                             else
                                                             {
                                                                 NavigateToView(Constants.ViewName.Selection, null);
                                                             }
                                                         }
                                                         else
                                                         {
                                                             BaseController.InvalidPromotionCodeAttempts++;
                                                             ((PromoCodes)_controls[Constants.ViewName.PromoCodes]).ShowErrorMessage(invalidReasonCode);
                                                         }
                                                     }
                                                 };

                worker.RunWorkerAsync();
            }
        }

        #endregion PromoCodes

        #region CountDown

        /// <summary>
        /// Called when [count down end].
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="eventArgs">The <see cref="EventArgs" /> instance containing the event data.</param>
        private void OnCountDownEnd(object sender, EventArgs eventArgs)
        {
            Logout_Click(sender, null);
        }

        /// <summary>
        /// Handles the OnYesClicked event of the PopupCountDown control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="eventArgs">The <see cref="EventArgs" /> instance containing the event data.</param>
        private void PopupCountDown_OnYesClicked(object sender, EventArgs eventArgs)
        {
        }

        /// <summary>
        /// Handles the OnNoClicked event of the PopupCountDown control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="eventArgs">The <see cref="EventArgs" /> instance containing the event data.</param>
        private void PopupCountDown_OnNoClicked(object sender, EventArgs eventArgs)
        {
            BaseController.Logout();

            NavigateToView(Constants.ViewName.Start, null);
        }

        #endregion CountDown

        #region Error message

        /// <summary>
        /// Handles the OnDoneButtonClicked event of the ErrorMessage control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void ErrorMessage_OnDoneButtonClicked(object sender, EventArgs e)
        {
            UIHelper.DispatchThread(this, main => main.NavigateToView(Constants.ViewName.Start, null));
        }

        #endregion Error message

        #region ThankYou

        /// <summary>
        /// Handles the OnTimeout event
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        private void ThankYou_OnTimeout(object sender, EventArgs e)
        {
            if (BaseController.LoggedOnUser != null)
            {
                if (BaseController.LoggedOnUser.GroupID == Constants.Group.SuperUser || BaseController.LoggedOnUser.GroupID == Constants.Group.SwapStationAdmin)
                    NavigateToView(Constants.ViewName.Selection, null);
                else
                {
                    BaseController.Logout();
                    NavigateToView(Constants.ViewName.Start, null);
                }
            }
            else
            {
                BaseController.Logout();
                NavigateToView(Constants.ViewName.Start, null);
            }
        }

        /// <summary>
        /// Handles the OnDoneButtonClicked event of the ThankYou control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        private void ThankYou_OnDoneButtonClicked(object sender, EventArgs e)
        {
            if (BaseController.LoggedOnUser != null)
            {
                if (BaseController.LoggedOnUser.GroupID == Constants.Group.SuperUser || BaseController.LoggedOnUser.GroupID == Constants.Group.SwapStationAdmin)
                    NavigateToView(Constants.ViewName.Selection, null);
                else
                {
                    BaseController.Logout();
                    NavigateToView(Constants.ViewName.Start, null);
                }
            }
            else
            {
                BaseController.Logout();
                NavigateToView(Constants.ViewName.Start, null);
            }
        }

        #endregion ThankYou

        #region TransactionSummary

        /// <summary>
        /// Handles the OnDiscountCodeClicked event of the TransactionSummary control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs" /> instance containing the event data.</param>
        /// <exception cref="System.NotImplementedException"></exception>
        private void TransactionSummary_OnDiscountCodeClicked(object sender, RoutedEventArgs e)
        {
            NavigateToView(Constants.ViewName.PromoCodes, null);
        }

        /// <summary>
        /// Handles the OnBackClicked event of the TransactionSummary control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs" /> instance containing the event data.</param>
        /// <exception cref="System.NotImplementedException"></exception>
        private void TransactionSummaryOnBackClicked(object sender, RoutedEventArgs e)
        {
            NavigateToView(Constants.ViewName.GetBatteries, null);
        }

        /// <summary>
        /// Handles the OnDoneClicked event of the TransactionSummary control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs" /> instance containing the event data.</param>
        /// <exception cref="System.NotImplementedException"></exception>
        private void TransactionSummaryOnDoneClicked(object sender, RoutedEventArgs e)
        {
            if (BaseController.LoggedOnUser == null)
                NavigateToView(Constants.ViewName.AnonymousTerms, null);
            else
            {
                //
                //  Run the code like we are asking for a "new card" each time.
                //
                BaseController.LoggedOnUser.CustomerProfileId = null;
                BaseController.LoggedOnUser.PaymentProfileId = null;
                BaseController.LoggedOnUser.ZipCode = null;
                NavigateToView(Constants.ViewName.Checkout, null);
            }
        }

        #endregion TransactionSummary

        #region Terms and Condition

        /// <summary>
        /// Handles the OnIAcceptButtonCliked event of the Terms control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        public void Terms_OnIAcceptButtonCliked(object sender, EventArgs e)
        {
            try
            {
                bool? isSuccess = MembershipRegistrationController.SaveRegistrationUser();
                if (isSuccess == true)
                {
                    isSuccess = LoginController.AuthenticateUser(BaseController.RegistrationUser.Email, BaseController.RegistrationUser.Password);
                    NavigateToView(BaseController.RecentViewOfCurrentFlow, null);
                }
                else
                    throw new Exception("Registration Failed");
            }
            catch
            {
                throw;
            }
            


        }

        /// <summary>
        /// Handles the OnCancelButtonCliked event of the Terms control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        public void Terms_OnCancelButtonCliked(object sender, EventArgs e)
        {
            if (BaseController.RegistrationUser != null)
            {
                BaseController.RegistrationUser = null;
            }

            NavigateToView(BaseController.RecentViewOfCurrentFlow, null);
        }

        #endregion Terms and Condition

        #region Anonymous Terms and Conditions

        /// <summary>
        /// Handles the OnIAcceptButtonCliked event of the Terms control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        public void AnonymousTerms_OnIAcceptButtonCliked(object sender, EventArgs e)
        {
            try
            {
                NavigateToView(Constants.ViewName.Checkout, null);
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Handles the OnCancelButtonCliked event of the Terms control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        public void AnonymousTerms_OnCancelButtonCliked(object sender, EventArgs e)
        {
            if (BaseController.RegistrationUser != null)
            {
                BaseController.RegistrationUser = null;
            }
            NavigateToView(BaseController.RecentViewOfCurrentFlow, null);
        }

        #endregion

        #region User Profile

        /// <summary>
        /// Handles the OnContinueTransactionClicked event of the UserProfile control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        void UserProfile_OnContinueTransactionClicked(object sender, EventArgs e)
        {
            NavigateToView(BaseController.RecentViewOfCurrentFlow, string.Empty);
        }

        /// <summary>
        /// Handles the OnCancelTransactionClicked event of the UserProfile control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        void UserProfile_OnCancelTransactionClicked(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// Handles the OnChangeSubscriptionPlanClicked event of the UserProfile control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        void UserProfile_OnChangeSubscriptionPlanClicked(object sender, EventArgs e)
        {
            //
            // CK 1/6/13 - Make sure we can never get here - disabled for now
            //
            //NavigateToView(Constants.ViewName.MembershipSubscription, string.Empty);
        }

        #endregion User Profile

        #region Card Choice
        /// <summary>
        /// Handles the Existing Card event of the Card Choice control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        void CardChoice_OnExistingCardClicked(object sender, EventArgs e)
        {
            NavigateToView(Constants.ViewName.Checkout, null);
        }

        /// <summary>
        /// Handles the New Card event of the Card Choice control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        void CardChoice_OnNewCardClicked(object sender, EventArgs e)
        {
            BaseController.LoggedOnUser.CustomerProfileId = null;
            BaseController.LoggedOnUser.PaymentProfileId = null;
            BaseController.LoggedOnUser.ZipCode = null;
            NavigateToView(Constants.ViewName.Checkout, null);
        }

        #endregion  

        #region Add Card To Account

        /// <summary>
        /// Handles the Save Card event of the Add Card To Account control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>

        void AddCardToAccount_OnSaveCardClicked(object sender, EventArgs e)
        {
            CheckOutController.MembershipSaveCardOnFile(BaseController.CardInfo);
            MembershipRegistrationController.UpdateMembershipProfile(BaseController.LoggedOnUser.MemberId, BaseController.CurrentTransaction.CustomerProfileId, BaseController.CurrentTransaction.PaymentProfileID, 0, 0, false, false);
            BaseController.LoggedOnUser.CustomerProfileId = BaseController.CurrentTransaction.CustomerProfileId;
            BaseController.LoggedOnUser.PaymentProfileId = BaseController.CurrentTransaction.PaymentProfileID;
            NavigateToView(Constants.ViewName.Vending, null);
        }

        /// <summary>
        /// Handles the Dont Save Card event of the Add Card To Account control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>

        void AddCardToAccount_OnDontSaveCardClicked(object sender, EventArgs e)
        {
            NavigateToView(Constants.ViewName.Vending, null);
        }

        #endregion

        #region Inventory Admin
        /// <summary>
        /// Handles the Cancel event of the Inventory Admin control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        void InventoryAdmin_OnDoneClicked(object sender, EventArgs e)
        {
            NavigateToView(Constants.ViewName.Selection, null);
        }
        #endregion  


    }
}

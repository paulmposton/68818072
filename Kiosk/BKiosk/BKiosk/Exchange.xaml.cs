using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using Phidgets;
using Phidgets.Events;

namespace Bettery.Kiosk
{
    /// <summary>
    /// Interaction logic for Exchange.xaml
    /// </summary>
    public partial class Exchange : Page
    {
        //Declare an InterfaceKit object
        private static InterfaceKit ifKit;

        private int _aaExchange = 0;
        private int _aaaExchange = 0;
        private bool _cartridgeInserted = true;
        private BackgroundWorker _worker = new BackgroundWorker();

        public Exchange()
        {
            InitializeComponent();
            BaseController.CurrentPage = this;

            _worker.WorkerSupportsCancellation = true;
            _worker.RunWorkerCompleted += PageLoad_RunWorkerCompleted;
            _worker.DoWork += PageLoad_DoWork;
        }

        public Exchange(int aaExchange, int aaaExchange)
        {
            _aaExchange = aaExchange;
            _aaaExchange = aaaExchange;
            InitializeComponent();
            BaseController.CurrentPage = this;

            _worker.WorkerSupportsCancellation = true;
            _worker.RunWorkerCompleted += PageLoad_RunWorkerCompleted;
            _worker.DoWork += PageLoad_DoWork;
        }

        /// <summary>
        /// Handles the Loaded event of the Page control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs" /> instance containing the event data.</param>
        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            BetteryBusyIndicator.IsBusy = true;
            _worker.RunWorkerAsync();
        }

        /// <summary>
        /// Handles the DoWork event of the PageLoad control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="DoWorkEventArgs" /> instance containing the event data.</param>
        private void PageLoad_DoWork(object sender, DoWorkEventArgs e)
        {
            InitPhidget();
        }

        /// <summary>
        /// Handles the RunWorkerCompleted event of the PageLoad control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RunWorkerCompletedEventArgs" /> instance containing the event data.</param>
        private void PageLoad_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (BaseController.IsLoggedOnUser)
            {
                Login.Content = "Logout";
            }

            BetteryBusyIndicator.IsBusy = false;
            UPCCode.Focus();
        }

        private void DoneButton_Click(object sender, RoutedEventArgs e)
        {
            // Remove event handler and close Phidget
            ifKit.InputChange -= new InputChangeEventHandler(ifKit_InputChange);
            ifKit.close();

            // Go to Return Summary Page
            BaseController.PreviousPage = this;
            ReturnSummary page = new ReturnSummary(_aaExchange);
            this.NavigationService.Navigate(page);
        }

        private void ReturnButton_Click(object sender, RoutedEventArgs e)
        {
            if (_cartridgeInserted)
            {
                // increment each battery type indicated by the upc code
                //
                //string upc = UPCCode.Text;
                //if (upc == "0100")
                //    _aaaExchange++;
                //else if (upc == "0101")
                //    _aaExchange++;
                //
                // For now, this accepts input from the scanner and increments aa batteries if the phidget Rollball detected a cartridge was inserted
                _aaExchange++;
                _cartridgeInserted = true;
            }
        }

        private void FAQButton_Click(object sender, RoutedEventArgs e)
        {
            BaseController.PreviousPage = this;
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

        private void InitPhidget()
        {
            try
            {
                //Initialize the InterfaceKit object
                ifKit = new InterfaceKit();

                //Hook the basica event handlers
                ifKit.Error += new ErrorEventHandler(ifKit_Error);

                //Open the object for device connections
                ifKit.open();

                //Wait for an InterfaceKit phidget to be attached
                ifKit.waitForAttachment(3000);

                //Hook the phidget spcific event handlers
                ifKit.InputChange += new InputChangeEventHandler(ifKit_InputChange);
            }
            catch (PhidgetException ex)
            {
                Console.WriteLine(ex.Description);
            }
        }

        private static void ifKit_Error(object sender, ErrorEventArgs e)
        {
            throw new Exception(e.exception.Message);
        }

        //Input Change event handler
        private void ifKit_InputChange(object sender, InputChangeEventArgs e)
        {
            // A cartridge has been inserted
            _cartridgeInserted = true;
        }
    }
}
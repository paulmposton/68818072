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
using BKiosk.HelperClasses;
using System.Data.SqlClient;
using System.IO.Ports;

namespace Bettery.Kiosk
{
    /// <summary>
    /// Interaction logic for Exchange.xaml
    /// </summary>
    public partial class VendingPage : Page
    {
        private BackgroundWorker _worker = new BackgroundWorker();

        public VendingPage()
        {
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
            try
            {
                //Todo: Confirm: Base on the number of betteries, bettery type that user had selected
                //Ex: 3 AA betterys && 5 AAA bettery && 1 cartridge
                //const int aaCount = 3;
                //const int aaaCount = 5;
                //const int cartridgeCount = 1;

                //Product: AA
                int totalBins = GetTotalQuantitybyProduct(ProductTypes.AA);
                if (totalBins > 0)
                {
                    List<BinVend> aaBins = GetBinsbyProduct(ProductTypes.AA);
                    foreach (BinVend bin in aaBins)
                    {
                        if (bin.Quantity > 0)
                        {
                            SendCommandToSerialPort(bin.Quantity);

                            DecrementBinInventory(bin);
                        }
                    }
                }

                //Product: AAA
                totalBins = GetTotalQuantitybyProduct(ProductTypes.AAA);
                if (totalBins > 0)
                {
                    List<BinVend> aaaBins = GetBinsbyProduct(ProductTypes.AAA);
                    foreach (BinVend bin in aaaBins)
                    {
                        if (bin.Quantity > 0)
                        {
                            SendCommandToSerialPort(bin.Quantity);

                            DecrementBinInventory(bin);
                        }
                    }
                }

                //Product: Cartridge
                totalBins = GetTotalQuantitybyProduct(ProductTypes.Cartridge);
                if (totalBins > 0)
                {
                    List<BinVend> cartridgeBins = GetBinsbyProduct(ProductTypes.Cartridge);
                    foreach (BinVend bin in cartridgeBins)
                    {
                        if (bin.Quantity > 0)
                        {
                            SendCommandToSerialPort(bin.Quantity);

                            DecrementBinInventory(bin);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                //Todo: Log message
                throw;
            }
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
        }

        /// <summary>
        /// Handles the Click event of the FAQButton control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.RoutedEventArgs"/> instance containing the event data.</param>
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

        /// <summary>
        /// Handles the Click event of the StartOverButton control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.RoutedEventArgs"/> instance containing the event data.</param>
        private void StartOverButton_Click(object sender, RoutedEventArgs e)
        {
            BaseController.PreviousPage = this;
            Start page = new Start();
            this.NavigationService.Navigate(page);
        }

        /// <summary>
        /// Handles the Click event of the Terms_PrivacyButton control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.RoutedEventArgs"/> instance containing the event data.</param>
        private void Terms_PrivacyButton_Click(object sender, RoutedEventArgs e)
        {
            BaseController.PreviousPage = this;
        }

        /// <summary>
        /// Handles the Click event of the BackButton control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.RoutedEventArgs"/> instance containing the event data.</param>
        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            BaseController.PreviousPage = this;
            Welcome page = new Welcome();
            this.NavigationService.Navigate(page);
        }

        /// <summary>
        /// Handles the MediaEnded event of the IntroduceMedia control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.RoutedEventArgs"/> instance containing the event data.</param>
        private void IntroduceMedia_MediaEnded(object sender, RoutedEventArgs e)
        {
            IntroduceMedia.Position = TimeSpan.Zero;
            IntroduceMedia.LoadedBehavior = MediaState.Play;
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

        #region Methods

        /// <summary>
        /// Gets the binsby product.
        /// </summary>
        /// <param name="productType">Type of the product.</param>
        /// <returns></returns>
        protected List<BinVend> GetBinsbyProduct(ProductTypes productType)
        {
            List<BinVend> binVends = new List<BinVend>();
            SqlParameter[] paramenters = new SqlParameter[1];
            paramenters[0] = new SqlParameter("@Product", productType.ToString());

            SqlDataReader reader = SqlHelper.ExecuteReader(BaseController.ConnectionString, "GetBinsbyProduct", paramenters);

            while (reader.Read())
            {
                BinVend bin = new BinVend();
                bin.BinId = SqlHelper.ToInt32(reader, "BinID");
                bin.Quantity = SqlHelper.ToInt32(reader, "PackageQuantity");

                binVends.Add(bin);
            }

            return binVends;
        }

        /// <summary>
        /// Decrements the bin inventory.
        /// </summary>
        /// <param name="bin">The bin.</param>
        /// <returns></returns>
        protected int DecrementBinInventory(BinVend bin)
        {
            List<BinVend> binVends = new List<BinVend>();
            SqlParameter[] paramenters = new SqlParameter[2];
            paramenters[0] = new SqlParameter("@BindID", bin.BinId);
            paramenters[1] = new SqlParameter("@Quantity", bin.Quantity);

            int result = SqlHelper.ExecuteNonQuery(BaseController.ConnectionString, "DecrementBinInventory", paramenters);

            return result;
        }

        /// <summary>
        /// Gets the total quantityby product.
        /// </summary>
        /// <param name="productType">Type of the product.</param>
        /// <returns></returns>
        protected int GetTotalQuantitybyProduct(ProductTypes productType)
        {
            int totalQuantity = 0;
            SqlParameter[] paramenters = new SqlParameter[1];
            paramenters[0] = new SqlParameter("@ProductDescription", productType.ToString());

            SqlDataReader reader = SqlHelper.ExecuteReader(BaseController.ConnectionString, "GetTotalQuantitybyProduct", paramenters);

            while (reader.Read())
            {
                totalQuantity = SqlHelper.ToInt32(reader);
            }

            return totalQuantity;
        }

        /// <summary>
        /// Sends the command to serial port.
        /// </summary>
        /// <param name="command">The command.</param>
        protected void SendCommandToSerialPort(int command)
        {
            SerialPort port = new SerialPort("COM6", 9600, Parity.None, 8, StopBits.One);
            port.Open();
            port.Write("!" + command);
            port.Close();
        }

        #endregion
    }
}
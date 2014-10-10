# define DEBUG

using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Data.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using Bettery.Kiosk.Common;
using Bettery.Kiosk.Entities;
using Microsoft.ServiceBus;
using Microsoft.ServiceBus.Messaging;
using Phidgets;
using Phidgets.Events;
using System.Threading;
using Bettery.Kiosk.DataAccess;
using System.IO.Ports;

namespace Bettery.Kiosk.Controllers
{
    /// <summary>
    /// Class Base Controller
    /// </summary>
    public static class BaseController
    {
        private static readonly string[] _recentViews = new[]
                            {
                                Constants.ViewName.Selection,
                                Constants.ViewName.Welcome,
                                Constants.ViewName.Start,
                                Constants.ViewName.TransactionSummary,
                                Constants.ViewName.CreditSwap,
                                Constants.ViewName.GetCase,
                                Constants.ViewName.GetBatteries,
                                Constants.ViewName.Exchange,
                                Constants.ViewName.Checkout
                            };

        private static readonly Dictionary<int, decimal> _subscriptionPlans = new Dictionary<int, decimal>();
        private static int[] _subscriptionPlanNumbers = new[] { 0, 8, 16, 24, 32, 48, 60 };

        private static readonly string _connectionString = ConfigurationManager.AppSettings[Constants.SettingKeys.ConnectionString];
        private static readonly string _serviceBusconnectionString = ConfigurationManager.AppSettings[Constants.SettingKeys.ServiceBusconnectionString];
        private static readonly string _portName = ConfigurationManager.AppSettings[Constants.SettingKeys.PortName];
        private static readonly string _apiLogin = ConfigurationManager.AppSettings[Constants.SettingKeys.ApiLogin];
        private static readonly string _transactionKey = ConfigurationManager.AppSettings[Constants.SettingKeys.TransactionKey];
        private static readonly string _stationId = ConfigurationManager.AppSettings[Constants.SettingKeys.StationId];
        private static readonly int _maxInvalidPromotionCodeAttempts = Convert.ToInt32(ConfigurationManager.AppSettings[Constants.SettingKeys.MaxInvalidPromotionCodeAttempts]);

        private static readonly int _serialPortTimeout = Convert.ToInt32(ConfigurationManager.AppSettings[Constants.SettingKeys.SerialPortTimeout]);
        private static readonly int _serialPortRetry = Convert.ToInt32(ConfigurationManager.AppSettings[Constants.SettingKeys.SerialPortRetry]);

        
        //  The following assumes all of the Batteries have the same price (Product ID = 1).  This may change in the future
        public static readonly decimal NewPrice = Convert.ToDecimal(ConfigurationManager.AppSettings["PurchasePrice"]);
        public static readonly decimal ReturnPrice = BaseDAL.GetReturnPricebyProduct(1);
        public static readonly decimal SwapPrice = Convert.ToDecimal(ConfigurationManager.AppSettings["SwapPrice"]);
        public static readonly decimal Tax = Convert.ToDecimal(ConfigurationManager.AppSettings["SalesTax"]);

        public static readonly int VendPauseCount = Convert.ToInt32(ConfigurationManager.AppSettings[Constants.SettingKeys.VendPauseCount]);

        private static readonly int _serviceCallRetries;
        private static readonly int _rollBallSwitchInputIndex;
        private static readonly int _contactSwitchInputIndex;
        private static readonly int _recycleBinCapacity;
        private static readonly int _returnBinCapacity;
        private static readonly int _maxIdleTime;
        private static readonly int _showCountDown;

        private static readonly int _minAaRemainingToAlert;
        private static readonly int _minAaaRemainingToAlert;
        private static readonly int _minCartridgeRemainingToAlert;

        public static readonly double _alertInterval;

        private static bool _rollBallSwitchPressed = false;

        private static string _serialBuffer;
        private static bool _iVendSuccess;
        private static bool _iVendFail;
        private static bool _GVCReady;
        private static SerialPort _serialPort;
        // Fake a single failure, for testing
        // private static bool _FakeFailure;


        //public static int AaQuantity { get; set; }
        //public static int AaaQuantity { get; set; }
        //public static int EmptyPackageQuantity { get; set; }

        /// <summary>
        /// Initializes the <see cref="BaseController" /> class.
        /// </summary>
        static BaseController()
        {
            _maxIdleTime = Convert.ToInt32(ConfigurationManager.AppSettings[Constants.SettingKeys.MaxIdleTime]);
            _showCountDown = Convert.ToInt32(ConfigurationManager.AppSettings[Constants.SettingKeys.ShowCountDown]);
            _recycleBinCapacity = Convert.ToInt32(ConfigurationManager.AppSettings[Constants.SettingKeys.RecycleBinCapacity]);
            _returnBinCapacity = Convert.ToInt32(ConfigurationManager.AppSettings[Constants.SettingKeys.ReturnBinCapacity]);
            _rollBallSwitchInputIndex = Convert.ToInt32(ConfigurationManager.AppSettings[Constants.SettingKeys.RollBallSwitchInputIndex]);
            _contactSwitchInputIndex = Convert.ToInt32(ConfigurationManager.AppSettings[Constants.SettingKeys.ContactSwitchInputIndex]);
            _serviceCallRetries = Convert.ToInt32(ConfigurationManager.AppSettings[Constants.SettingKeys.ContactSwitchInputIndex]);

            _minAaRemainingToAlert = Convert.ToInt32(ConfigurationManager.AppSettings[Constants.SettingKeys.MinAaRemainingAlert]);
            _minAaaRemainingToAlert = Convert.ToInt32(ConfigurationManager.AppSettings[Constants.SettingKeys.MinAaaRemainingAlert]);
            _minCartridgeRemainingToAlert = Convert.ToInt32(ConfigurationManager.AppSettings[Constants.SettingKeys.MinCartridgeRemainingAlert]);

            _alertInterval = Convert.ToDouble(ConfigurationManager.AppSettings[Constants.SettingKeys.AlertInterval]);

            for (int i = 1; i < _subscriptionPlanNumbers.Length; i++)
            {
                decimal price = ReadPriceBySubscriptionPlanId(i);
                _subscriptionPlans[i] = price;
            }
            _serialPort = new SerialPort(_portName);
            _serialPort.DataReceived += new SerialDataReceivedEventHandler(DataReceivedHandler);

            _serialPort.BaudRate = 9600;
            _serialPort.Parity = Parity.None;
            _serialPort.StopBits = StopBits.One;
            _serialPort.DataBits = 8;
            _serialPort.Handshake = Handshake.None;
            _GVCReady = false;
            // Fake a Failure (CK)
            //_FakeFailure = true;

        }
        

        /// <summary>
        /// Occurs when [on logout].
        /// </summary>
        public static event EventHandler OnLogout;

        /// <summary>
        /// Occurs when [on throw exception].
        /// </summary>
        public static event EventHandler OnThrowException;

        /// <summary>
        /// Gets the min aa remaining alert.
        /// </summary>
        public static int MinAaRemainingAlert
        {
            get { return _minAaRemainingToAlert; }
        }

        /// <summary>
        /// Gets the status of the GVC
        /// </summary>
        public static bool GVCReady
        {
            get { return _GVCReady; }
        }

        /// <summary>
        /// Gets the min aaa remaining alert.
        /// </summary>
        public static int MinAaaRemainingAlert
        {
            get { return _minAaaRemainingToAlert; }
        }

        /// <summary>
        /// Gets the min cartridge remaining alert.
        /// </summary>
        public static int MinCartridgeRemainingAlert
        {
            get { return _minCartridgeRemainingToAlert; }
        }

        /// <summary>
        /// Gets the Local Sql connection string.
        /// </summary>
        /// <value>
        /// The Local Sql connection string.
        /// </value>
        public static string ConnectionString
        {
            get { return _connectionString; }
        }

        /// <summary>
        /// Gets the API login.
        /// </summary>
        /// <value>
        /// The API login.
        /// </value>
        public static string ApiLogin
        {
            get { return _apiLogin; }
        }

        /// <summary>
        /// Gets the transaction key.
        /// </summary>
        /// <value>
        /// The transaction key.
        /// </value>
        public static string TransactionKey
        {
            get { return _transactionKey; }
        }

        /// <summary>
        /// Gets the max invalid promotion code attempts.
        /// </summary>
        public static int MaxInvalidPromotionCodeAttempts
        {
            get { return _maxInvalidPromotionCodeAttempts; }
        }

        /// <summary>
        /// Gets the max idle time.
        /// </summary>
        /// <value>
        /// The max idle time.
        /// </value>
        public static int MaxIdleTime
        {
            get { return _maxIdleTime; }
        }

        /// <summary>
        /// Gets the show count down.
        /// </summary>
        /// <value>
        /// The show count down.
        /// </value>
        public static int ShowCountDown
        {
            get { return _showCountDown; }
        }

        /// <summary>
        /// Gets the station id.
        /// </summary>
        /// <value>
        /// The station id.
        /// </value>
        public static string StationId
        {
            get { return _stationId; }
        }

        /// <summary>
        /// Gets the recycle bin capacity.
        /// </summary>
        /// <value>
        /// The recycle bin capacity.
        /// </value>
        public static int RecycleBinCapacity
        {
            get { return _recycleBinCapacity; }
        }

        /// <summary>
        /// Gets the return bin capacity.
        /// </summary>
        /// <value>
        /// The return bin capacity.
        /// </value>
        public static int ReturnBinCapacity
        {
            get { return _returnBinCapacity; }
        }

        /// <summary>
        /// Gets the alert interval.
        /// </summary>
        /// <value>
        /// The alert interval.
        /// </value>
        public static double AlertInterval
        {
            get { return _alertInterval; }
        }

        /// <summary>
        /// Gets a value indicating whether [cartridge inserted].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [cartridge inserted]; otherwise, <c>false</c>.
        /// </value>
        public static bool CartridgeInserted { get; set; }

        /// <summary>
        /// Gets or sets the get batteries mode.
        /// </summary>
        /// <value>
        /// The get batteries mode.
        /// </value>
        public static GetBatteriesModes GetBatteriesMode { get; set; }

        /// <summary>
        /// Gets or sets the previous view.
        /// </summary>
        /// <value>
        /// The previous view.
        /// </value>
        public static string PreviousView { get; set; }

        /// <summary>
        /// Gets or sets the recent view of current flow.
        /// </summary>
        /// <value>
        /// The recent view of current flow.
        /// </value>
        public static string RecentViewOfCurrentFlow { get; set; }

        /// <summary>
        /// Gets or sets the current view.
        /// </summary>
        /// <value>
        /// The current view.
        /// </value>
        public static string CurrentView { get; set; }

        /// <summary>
        /// Gets or sets the current control focus.
        /// </summary>
        /// <value>
        /// The current control focus.
        /// </value>
        public static Control CurrentControlFocus { get; set; }

        /// <summary>
        /// Gets or sets the last active time.
        /// </summary>
        /// <value>
        /// The last active time.
        /// </value>
        public static DateTime LastActiveTime { get; set; }


        /// <summary>
        /// Gets or sets the card info.
        /// </summary>
        /// <value>
        /// The card info.
        /// </value>
        public static string CardInfo { get; set; }

        /// <summary>
        /// Gets or sets the user.
        /// </summary>
        /// <value>
        /// The user.
        /// </value>
        public static BetteryUser LoggedOnUser { get; set; }

        /// <summary>
        /// Gets or sets the registration user.
        /// </summary>
        /// <value>
        /// The registration user.
        /// </value>
        public static BetteryUser RegistrationUser { get; set; }

        /// <summary>
        /// Gets or sets the selected bettery.
        /// </summary>
        /// <value>
        /// The selected bettery.
        /// </value>
        public static BetteryVend SelectedBettery { get; set; }

        /// <summary>
        /// Gets or sets the current transaction.
        /// </summary>
        /// <value>
        /// The current transaction.
        /// </value>
        public static TransactionQueueData CurrentTransaction { get; set; }

        /// <summary>
        /// Gets or sets the invalid promotion code attempts.
        /// </summary>
        /// <value>
        /// The invalid promotion code attempts.
        /// </value>
        public static int InvalidPromotionCodeAttempts { get; set; }

        #region Logout

        /// <summary>
        /// Logouts this instance.   If any arg supplied, will run this method directly, forcing a logout
        /// </summary>
        public static void Logout()
        {
            LoggedOnUser = null;
            SelectedBettery = null;
            CartridgeInserted = false;
            PreviousView = string.Empty;
            CurrentTransaction = null;
            RegistrationUser = null;
            CardInfo = string.Empty;
            GetBatteriesMode = GetBatteriesModes.BuyNew;
            CurrentView = Constants.ViewName.Start;
            RecentViewOfCurrentFlow = string.Empty;
            InvalidPromotionCodeAttempts = 0;

            if (OnLogout != null)
            {
                OnLogout.Invoke(null, EventArgs.Empty);
            }
        }

        #endregion Logout

        #region SerialPort

        /// <summary>
        /// Sends the command to serial port.
        /// </summary>
        /// <param name="command">The command.</param>
        public static bool SendCommandToSerialPort(int command)
        {
            _iVendSuccess = false;
            _iVendFail = false;

            try
            {
                if (ConfigurationManager.AppSettings["EnableSerialVending"] != null)
                    if (!Convert.ToBoolean(ConfigurationManager.AppSettings["EnableSerialVending"])) {
                        Thread.Sleep(2000); 
                        return true;
                    }

                if (!_serialPort.IsOpen)
                    _serialPort.Open();
                
                // Send the command to the serial port
                _serialPort.Write(string.Format(Constants.Messages.SerialPortMessage, command));

                // Wait for a response
                for (int i = 0; i < _serialPortRetry; i++)
                {
                    Thread.Sleep(_serialPortTimeout);
                    if (_iVendSuccess || _iVendFail)
                        break;
                }
                //
                // Sleep for 1/2 second to give the GVC a chance to reset (just in case)
                //
                Thread.Sleep(500);

                //  Nothing happened, retry
                if (!_iVendFail && !_iVendSuccess)
                {
                    Logger.Log(EventLogEntryType.Warning, "Serial Port Communication Error - doing a retry", BaseController.StationId);

                    // Send the command to the serial port
                    _serialPort.Write(string.Format(Constants.Messages.SerialPortMessage, command));

                    // Wait for a response
                    for (int i = 0; i < _serialPortRetry; i++)
                    {
                        Thread.Sleep(_serialPortTimeout);
                        if (_iVendSuccess || _iVendFail)
                            break;
                    }
                    //
                    // Sleep for 1/2 second to give the GVC a chance to reset (just in case)
                    //
                    Thread.Sleep(500);

                    if (!_iVendFail && !_iVendSuccess)
                    {
                        Logger.Log(EventLogEntryType.Error, "Serial Port Communication Error - no response detected from sending serial command", BaseController.StationId);
                        throw new Exception("Serial Port Communication Error - no response detected from sending serial command");
                    }
                }                   
                //
                //  GVC Controller tried to vend 2x times from the spiral, with no success.   Time to mark the spiral out of service.
                //  Need to put in the BIN number here...
                //
                if (_iVendFail)
                {
                    Logger.Log(EventLogEntryType.Error, "iVend Failure Detected In SendCommandToSerial", BaseController.StationId);
                    return false;
                }
                else if (_iVendSuccess)
                    return true;
                else
                    return false;
                    
            }
            catch (Exception ex)
            {
                throw;
            }

        }

        /// <summary>
        /// Checks to see if the Serial port is ready
        /// </summary>
        /// <param name="command">The command.</param>
        public static bool SerialPortReady()
        {
            if (ConfigurationManager.AppSettings["CheckSerialReady"] != null)
                 if (!Convert.ToBoolean(ConfigurationManager.AppSettings["CheckSerialReady"])) return true;
            //
            // Before checking connectivity, make sure the GVC controller is ready
            //
            if (!_serialPort.IsOpen)
                _serialPort.Open();

            // Send a bad vend command to force the GVC to respond
            // Log only in debug mode
            //Logger.Log(EventLogEntryType.Error, "Checking Serial Port in SerialPortReady", BaseController.StationId);

            _GVCReady = false;
            _serialPort.Write("!000_");

            // Wait for a response
            for (int i = 0; i < _serialPortRetry; i++)
            {
                Thread.Sleep(_serialPortTimeout);

                if (_GVCReady)
                    break;
            }
            if (!_GVCReady) return false;
            return true;
        }

        private static void DataReceivedHandler(object sender, SerialDataReceivedEventArgs e)
        {
            SerialPort sp = (SerialPort)sender;
            string indata = sp.ReadExisting();
            _serialBuffer += indata;
            //
            //  Vend successful, look for ...Enjoy message (just look for joy, it's easier to test this way)
            //
            if (_serialBuffer.Contains("joy"))
            {
               
                _iVendSuccess = true;
                _serialBuffer = string.Empty;
                //Logger.Log(EventLogEntryType.Information, "Successful vend detected in DataRecievedHandler", BaseController.StationId);
                return;
            }
            //
            //  iVend failure - attempted to vend twice, no product detected in IR sensor.   
            //  Look for "Make Another Selection" message.   Just look for "nother"
            //
            if (_serialBuffer.Contains("nother"))
            {
                _iVendFail = true;
                _serialBuffer = string.Empty;
                //Logger.Log(EventLogEntryType.Error, "iVend Failure Detected in DataRecievedHandler", BaseController.StationId);
                return;
            }
            //
            //  Vending machine ready for command if "Free on Us <br> Selection: --" messaage is showing
            //
            if (_serialBuffer.Contains("Selection:  --"))
            {
                _GVCReady = true;
                _serialBuffer = string.Empty;
                //
                //  Don't log this except in debug mode
                //
                //Logger.Log(EventLogEntryType.Information, "GVC Ready Detected in DataRecievedHandler", BaseController.StationId);
                return;
            }
            //
            //  Log all other GVC Events (This should be debug mode only)
            //
            // Logger.Log(EventLogEntryType.Information, _serialBuffer, BaseController.StationId);
        }

        #endregion

        #region Phidgets

        private static InterfaceKit _ifKit;

        /// <summary>
        /// Occurs when [on interface kit input changed].
        /// </summary>
        public static event EventHandler OnInterfaceKitInputChanged;

        /// <summary>
        /// Inits the phidget.
        /// </summary>
        private static void InitPhidget()
        {
            try
            {
                if (_ifKit == null)
                {
                    //Initialize the InterfaceKit object
                    _ifKit = new InterfaceKit();

                    //Hook the basica event handlers
                    _ifKit.Error += IFKit_Error;
                    //Hook the phidget spcific event handlers
                    _ifKit.InputChange += IFKit_InputChange;
                }
            }
            catch (PhidgetException ex)
            {
                AlertController.TransactionFailureAlert(ex.Message);
                Logger.Log(EventLogEntryType.Error, ex, StationId);
                RaiseOnThrowExceptionEvent();
            }
            catch (Exception ex)
            {
                AlertController.TransactionFailureAlert(ex.Message);
                Logger.Log(EventLogEntryType.Error, ex, StationId);
                RaiseOnThrowExceptionEvent();
            }
        }

        /// <summary>
        /// Handles the Error event of the ifKit control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="ErrorEventArgs"/> instance containing the event data.</param>
        private static void IFKit_Error(object sender, ErrorEventArgs e)
        {
            AlertController.TransactionFailureAlert(e.exception.Message);
            Logger.Log(EventLogEntryType.Error, e.exception, StationId);
            //throw new Exception(e.exception.Message);
        }

        /// <summary>
        /// Handles the InputChange event of the ifKit control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="InputChangeEventArgs"/> instance containing the event data.</param>
        private static void IFKit_InputChange(object sender, InputChangeEventArgs e)
        {
            // A cartridge has been inserted
            if (e.Index == _rollBallSwitchInputIndex && e.Value)
            {
                //Todo: Bruce need to review this code.
                //If the ball is keeping press until the contact switch is active then it works.
                //But if the ball is pressed and then released before the contact switch be active then it does not work correctly
                //In this case, we may need to check if the value is true
                _rollBallSwitchPressed = e.Value;
            }
            //if (e.Index == _rollBallSwitchInputIndex)
            //{
            //    //Todo: Bruce need to review this code.
            //    //If the ball is keeping press until the contact switch is active then it works.
            //    //But if the ball is pressed and then released before the contact switch be active then it does not work correctly
            //    //In this case, we may need to check if the value is true
            //    _rollBallSwitchPressed = e.Value;
            //}

            #region For testing
            //Todo Bruce need to remove this code. It's just for testing
            //if (_rollBallSwitchPressed)
            //{
            //    if (SelectedBettery == null)
            //    {
            //        SelectedBettery = new BetteryVend();
            //    }

            //    SelectedBettery.AaReturn++;

            //    EventHandler handler = OnInterfaceKitInputChanged;
            //    if (handler != null)
            //    {
            //        Application.Current.Dispatcher.BeginInvoke(new Action(() => { handler(null, EventArgs.Empty); }));
            //    }

            //    _rollBallSwitchPressed = false;
            //}

            #endregion For testing

            //Comment block of code for testing
            //Todo: Bruce need to uncomment this code
            else if (e.Index == _contactSwitchInputIndex && _rollBallSwitchPressed)
            {
                if (e.Value)
                {
                    if (SelectedBettery == null)
                    {
                        SelectedBettery = new BetteryVend();
                    }

                    SelectedBettery.AaReturn++;

                    EventHandler handler = OnInterfaceKitInputChanged;
                    if (handler != null)
                    {
                        Application.Current.Dispatcher.BeginInvoke(new Action(() => { handler(null, EventArgs.Empty); }));
                    }

                    _rollBallSwitchPressed = false;
                }
            }
        }

        /// <summary>
        /// Opens the phidget.
        /// </summary>
        /// <param name="milliseconds">The milliseconds.</param>
        /// <returns></returns>
        public static bool OpenPhidget(int milliseconds)
        {
            try
            {
                if (_ifKit == null)
                {
                    InitPhidget();
                }

                // Open the object for device connections
                _ifKit.open();

                // Wait for an InterfaceKit phidget to be attached
                _ifKit.waitForAttachment(3000);

                return true;
            }
            catch (PhidgetException ex)
            {
                //AlertController.TransactionFailureAlert(ex.Message);
                //Logger.Log(EventLogEntryType.Error, ex, StationId);
                //RaiseOnThrowExceptionEvent();
                //return false;
                return true;
            }
            catch (Exception ex)
            {
                AlertController.TransactionFailureAlert(ex.Message);
                Logger.Log(EventLogEntryType.Error, ex, StationId);
                RaiseOnThrowExceptionEvent();
                return false;
            }
        }

        /// <summary>
        /// Closes the phidget.
        /// </summary>
        /// <returns></returns>
        public static bool ClosePhidget()
        {
            try
            {
                if (_ifKit != null)
                {
                    // Close phidget
                    _ifKit.close();
                }

                return true;
            }
            catch (PhidgetException ex)
            {
                AlertController.TransactionFailureAlert(ex.Message);
                Logger.Log(EventLogEntryType.Error, ex, StationId);
                RaiseOnThrowExceptionEvent();
                return false;
            }
            catch (Exception ex)
            {
                AlertController.TransactionFailureAlert(ex.Message);
                Logger.Log(EventLogEntryType.Error, ex, StationId);
                RaiseOnThrowExceptionEvent();
                return false;
            }
        }


        #endregion Phidgets

        #region TransactionQueue

        /// <summary>
        /// Transactions the queue send.
        /// </summary>
        /// <param name="transactionId">The transaction id.</param>
        /// <param name="totalAmount">The amount.</param>
        /// <param name="emailAddress">The email address.</param>
        /// <param name="customerProfileId">The customer profile id.</param>
        /// <param name="paymentProfileID">The payment profile ID.</param>
        /// <param name="aaVend">The aa vend.</param>
        /// <param name="aaaVend">The aaa vend.</param>
        /// <param name="aaReturn">The aa return.</param>
        /// <param name="aaaReturn">The aaa return.</param>
        /// <param name="aaForgotVend">The aa forgot vend.</param>
        /// <param name="aaaForgotVend">The aaa forgot vend.</param>
        /// <param name="orderNumber">The order number.</param>
        /// <param name="authorization">The authorization.</param>
        /// <param name="promoCode">The promo code.</param>
        /// <param name="promoCodeAmount">The promo code amount.</param>
        /// <returns></returns>
        public static bool TransactionQueueSend(string transactionId,
                                    int MemberID,
                                    decimal subTotalAmount,
                                    decimal totalAmount,
                                    decimal taxAmount,
                                    string NameOnCard,
                                    string cardInfo,
                                    string emailAddress,
                                    string customerProfileId,
                                    string paymentProfileID,
                                    string CardHash,
                                    int aaVend,
                                    int aaaVend,
                                    int aaReturn,
                                    int aaaReturn,
                                    int aaForgotVend,
                                    int aaaForgotVend,
                                    string orderNumber,
                                    string authorization,
                                    string promoCode,
                                    decimal promoCodeAmount,
                                    int BatteryPacksCheckedOut)
        {
            try
            {
                // Sends a Transaction Message to the Service Bus

                // Configure Queue Settings
                QueueDescription qd = new QueueDescription("TransactionQueue");
                qd.MaxSizeInMegabytes = 5120;
                qd.DefaultMessageTimeToLive = new TimeSpan(0, 1, 0);

                // Create a new Queue with custom settings
                var namespaceManager = NamespaceManager.CreateFromConnectionString(_serviceBusconnectionString);
                try
                {
                    if (!namespaceManager.QueueExists("TransactionQueue"))
                    {
                        namespaceManager.CreateQueue("TransactionQueue");
                    }
                }
                catch (Exception ex)
                {
                    //AlertController.TransactionFailureAlert(ex.Message);
                    //Logger.Log(EventLogEntryType.Error, ex, StationId);
                    //RaiseOnThrowExceptionEvent();
                    return false;
                }

                // Send a message
                MessagingFactory factory = MessagingFactory.CreateFromConnectionString(_serviceBusconnectionString);

                MessageSender sender = factory.CreateMessageSender("TransactionQueue");

                // Create message, passing a string message for the body
                BrokeredMessage message = new BrokeredMessage("Kiosk Transaction");

                // Set some additional custom app-specific properties
                message.Properties["OrderNumber"] = orderNumber;
                message.Properties["TransactionID"] = transactionId;
                message.Properties["MemberID"] = MemberID;
                message.Properties["SubTotalAmount"] = subTotalAmount;
                message.Properties["TotalAmount"] = totalAmount;
                message.Properties["TaxAmount"] = taxAmount;
                message.Properties["NameOnCard"] = NameOnCard;
                message.Properties["CardInfo"] = cardInfo;
                message.Properties["EmailAddress"] = emailAddress;
                message.Properties["CustomerProfileID"] = customerProfileId;
                message.Properties["PaymentProfileID"] = paymentProfileID;
                message.Properties["AAVend"] = aaVend;
                message.Properties["AAAVend"] = aaaVend;
                message.Properties["AAForgotVend"] = aaForgotVend;
                message.Properties["AAAForgotVend"] = aaaForgotVend;
                message.Properties["AAReturn"] = aaReturn;
                message.Properties["AAAReturn"] = aaaReturn;
                message.Properties["Authorization"] = authorization;
                message.Properties["PromoCode"] = promoCode;
                message.Properties["PromoCodeAmount"] = promoCodeAmount;
                message.Properties["BatteryPacksCheckedOut"] = BatteryPacksCheckedOut;
                message.Properties["StationID"] = StationId;
                message.Properties["CardHash"] = CardHash;
                message.Properties["SwapPrice"] = BaseController.SwapPrice;
                message.Properties["PurchasePrice"] = BaseController.NewPrice;

                // Send message to the queue
                sender.Send(message);

                return true;
                // TODO: Bruce need to update the service bus to add new 3 parameters "AAForgotVend" & "AAAForgotVend" & "PromoCode"
            }
            catch (Exception ex)
            {
                //AlertController.TransactionFailureAlert(ex.Message);
                //Logger.Log(EventLogEntryType.Error, ex, StationId);
                //RaiseOnThrowExceptionEvent();
                return false;
            }
        }

        #endregion TransactionQueue

        #region Validate Inputs

        /// <summary>
        /// Validates the first name.
        /// </summary>
        /// <param name="firtname">The firtname.</param>
        /// <returns></returns>
        public static bool ValidateFirstName(string firtname)
        {
            if (string.IsNullOrWhiteSpace(firtname))
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Validates the last name.
        /// </summary>
        /// <param name="lastname">The lastname.</param>
        /// <returns></returns>
        public static bool ValidateLastName(string lastname)
        {
            if (string.IsNullOrWhiteSpace(lastname))
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Validates the email address.
        /// </summary>
        /// <param name="emailAddress">The email address.</param>
        /// <returns></returns>
        public static bool ValidateEmailAddress(string emailAddress)
        {
            Regex regex = new Regex(Constants.Messages.MatchEmailPattern);
            bool isValidEmail = regex.IsMatch(emailAddress);

            return isValidEmail;
        }

        /// <summary>
        /// Validates the zipcode.
        /// </summary>
        /// <param name="zipcode">The zipcode.</param>
        /// <returns></returns>
        public static bool ValidateZipcode(string zipcode)
        {
            if (zipcode == null || zipcode.Length != 5)
            {
                return false;
            }

            int result;
            if (int.TryParse(zipcode, out result))
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Validates the password.
        /// </summary>
        /// <param name="password">The password.</param>
        /// <returns></returns>
        public static bool ValidatePassword(string password)
        {
            if (password.Length < 6)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Validates the confirm password.
        /// </summary>
        /// <param name="password">The password.</param>
        /// <param name="confirm">The confirm.</param>
        /// <returns></returns>
        public static bool ValidateConfirmPassword(string password, string confirm)
        {
            if (password == confirm)
            {
                return true;
            }

            return false;
        }

        #endregion Validate Inputs

        #region ChargeHelper

        /// <summary>
        /// Calcs the charges.
        /// </summary>
        /// <param name="aaReturn">The aa return.</param>
        /// <param name="aaaReturn">The aaa return.</param>
        public static void CalcCharges(int aaReturn, int aaaReturn)
        {
            if (SelectedBettery == null)
            {
                SelectedBettery = new BetteryVend();
            }

            SelectedBettery.AaReturn = aaReturn;
            SelectedBettery.AaaReturn = aaaReturn;

            CalcCharges();
        }

        /// <summary>
        /// Calcs the charges.
        /// </summary>
        /// <param name="aaVend">The aa vend.</param>
        /// <param name="aaaVend">The aaa vend.</param>
        /// <param name="getBatteriesMode">The get batteries mode.</param>
        public static void CalcCharges(int aaVend, int aaaVend, GetBatteriesModes getBatteriesMode)
        {
            if (SelectedBettery == null)
            {
                SelectedBettery = new BetteryVend();
            }

            if (getBatteriesMode == GetBatteriesModes.BuyNew)
            {
                SelectedBettery.AaVend = aaVend;
                SelectedBettery.AaaVend = aaaVend;
            }
            else
            {
                //SelectedBettery.AaForgotDrainedVend = aaVend;
                //SelectedBettery.AaaForgotDrainedVend = aaaVend;
            }

            CalcCharges();
        }

        /// <summary>
        /// Calcs the charges.
        /// </summary>
        public static void CalcCharges()
        {
            if (SelectedBettery == null)
            {
                SelectedBettery = new BetteryVend();
            }

            try
            {
                    SelectedBettery.Swapped = Math.Min(SelectedBettery.TotalVendCartridges, SelectedBettery.ReturnedCartridges);
                    SelectedBettery.SwappedAmount = SelectedBettery.Swapped * SwapPrice;

                    SelectedBettery.CalculatedAaAmount = SelectedBettery.AaVend * SwapPrice;
                    SelectedBettery.CalculatedAaaAmount = SelectedBettery.AaaVend * SwapPrice;

                    SelectedBettery.CalculatedNewAmount = 0;
                    SelectedBettery.CalculatedNew = 0;
                    SelectedBettery.CalculatedReturned = 0;
                    SelectedBettery.CalculatedReturnedAmount = 0;
                    SelectedBettery.DepositAmount = 0;

                    if (SelectedBettery.TotalVendCartridges > SelectedBettery.ReturnedCartridges)
                    {
                        SelectedBettery.CalculatedNew = SelectedBettery.NewCartridges;
                        SelectedBettery.CalculatedNewAmount = SelectedBettery.CalculatedNew * NewPrice;
                        SelectedBettery.SubSubTotalAmount = SelectedBettery.TotalVendCartridges * SwapPrice;
                        SelectedBettery.TotalAmount = SelectedBettery.CalculatedNewAmount + SelectedBettery.SwappedAmount;
                        SelectedBettery.DepositAmount = SelectedBettery.NewCartridges * (NewPrice - SwapPrice);
                    }
                    //else if (SelectedBettery.ReturnedCartridges >= SelectedBettery.TotalVendCartridges)
                    //{
                        //SelectedBettery.CalculatedReturned = Math.Abs(SelectedBettery.NewCartridges);
                        //SelectedBettery.CalculatedReturnedAmount = SelectedBettery.CalculatedReturned * ReturnPrice;
                        //SelectedBettery.TotalAmount = SelectedBettery.SwappedAmount; 
                    //}
                    else
                    {
                        SelectedBettery.TotalAmount = SelectedBettery.Swapped * SwapPrice;
                        SelectedBettery.SubSubTotalAmount = SelectedBettery.TotalAmount;
                    }
                    SelectedBettery.TotalAmount = SelectedBettery.TotalAmount - SelectedBettery.PromotionalAmount;
                    //
                    //  Protect against promo code larger than purchase amount
                    //
                    if (SelectedBettery.TotalAmount < 0) SelectedBettery.TotalAmount = 0;

                    SelectedBettery.SubTotalAmount = SelectedBettery.TotalAmount;
                    SelectedBettery.TotalTaxAmount = SelectedBettery.TotalAmount * Tax;
                    SelectedBettery.TotalTaxAmount = (decimal)Common.Utils.RoundUp((double)SelectedBettery.TotalTaxAmount, 2);
                    SelectedBettery.TotalAmount += SelectedBettery.TotalTaxAmount;
            }
            catch (Exception ex)
            {
                AlertController.TransactionFailureAlert(ex.Message);
                Logger.Log(EventLogEntryType.Error, ex, StationId);
                RaiseOnThrowExceptionEvent();
            }
        }

        /// <summary>
        /// Gets the charges summary.
        /// </summary>
        /// <returns>The Charge Summary</returns>
        public static string GetChargesSummary()
        {
            //
            //  1/6/13 - CK.   Disabling all details except the total, to simplify the display.
            //     Removed # of exchanged, # of New, Promo and Sales Tax.   All calcs seem to be working, but seem redundant
            //     since they show up on the summary screen already.   This function just adds UI noise, it seems
            //
            //
            string paymentSummary = string.Empty;
            if (SelectedBettery != null)
            {
                // Update view
                // Calc Swapped Charges
                //if (SelectedBettery.TotalVendCartridges > 0 && SelectedBettery.ReturnedCartridges > 0)
                //{
                  // Display Exchanged
                //    paymentSummary += string.Format(Constants.Messages.SwappedPaymentSummary, SelectedBettery.Swapped, SwapPrice, SelectedBettery.SwappedAmount);
                //}

                //if (SelectedBettery.AaNewForgotDrainedAmount > 0)  //CK:  Commented this out, we are disabling "Forgot Batteries" for now.
                //{
                    //paymentSummary += string.Format(Constants.Messages.ForgotPaymentSummary, SelectedBettery.TotalForgotDrainedVendCartridges, SwapPrice, SelectedBettery.TotalForgotDrainedAmount);
                //}

                // Calc additional new Cartridges charges and Refund for returned cartridges if any
                //if (SelectedBettery.TotalVendCartridges > SelectedBettery.ReturnedCartridges)
                //{
                    // Display Additional
                //    paymentSummary += string.Format(Constants.Messages.AddNewPaymentSummary, SelectedBettery.CalculatedNew, NewPrice, SelectedBettery.CalculatedNewAmount);
                //}
                //
                //  This feature was disabled a long timne ago.   Not supporting online returns at this time
                //
                //else if (SelectedBettery.ReturnedCartridges > SelectedBettery.TotalVendCartridges)
                //{
                //    // Display Returned
                //    paymentSummary += string.Format(Constants.Messages.ReturnedPaymentSummary, SelectedBettery.CalculatedReturned, ReturnPrice, Math.Abs(SelectedBettery.CalculatedReturnedAmount));
                //}

                //
                //   Not intending to support user credits at this time
                //
                //if (LoggedOnUser != null)
                //{
                //    paymentSummary += string.Format(Constants.Messages.YourAccountCredit, LoggedOnUser.OutstandingCredit);
                //}

                //if (SelectedBettery.PromotionalAmount > 0)
                //{
                //    paymentSummary += string.Format(Constants.Messages.YourPromotionCredit, SelectedBettery.PromotionalAmount);
                //}

                // Display Total
                if (SelectedBettery.TotalAmount >= 0)
                {
                    //if (SelectedBettery.TotalTaxAmount > 0)
                    //    paymentSummary += string.Format(Constants.Messages.TaxCheckout, SelectedBettery.TotalTaxAmount);

                    paymentSummary += string.Format(Constants.Messages.TotalCheckout, SelectedBettery.TotalAmount);
                }
                //else
                //{
                //    paymentSummary += string.Format(Constants.Messages.CreditCheckout, Math.Abs(SelectedBettery.TotalAmount));
                //}
            }

            return paymentSummary;
        }

        #endregion ChargeHelper

        #region Workflow

        /// <summary>
        /// Updates the recent view of current flow.
        /// </summary>
        /// <param name="navigateTo">The navigate to.</param>
        public static void UpdateRecentViewOfCurrentFlow(string navigateTo)
        {
            if (_recentViews.Contains(navigateTo))
            {
                RecentViewOfCurrentFlow = navigateTo;
            }
        }

        #endregion Workflow

        #region Exception handle

        /// <summary>
        /// Raises the on throw exception event.
        /// </summary>
        public static void RaiseOnThrowExceptionEvent()
        {
            if (OnThrowException != null)
            {
                OnThrowException.Invoke(null, null);
            }
        }

        #endregion Exception handle

        #region SubscriptionPlan

        /// <summary>
        /// Gets the price by subscription plan id.
        /// </summary>
        /// <param name="subcriptionPlanId">The subcription plan id.</param>
        /// <returns></returns>
        public static decimal GetPriceBySubscriptionPlanId(int subcriptionPlanId)
        {
            decimal price = _subscriptionPlans[subcriptionPlanId];

            return price;
        }

        /// <summary>
        /// Gets the subscription plan message.
        /// </summary>
        /// <param name="subcriptionPlanId">The subcription plan id.</param>
        /// <returns></returns>
        public static string GetSubscriptionPlanMessage(int subcriptionPlanId)
        {
            int subcriptionPlanNumber = _subscriptionPlanNumbers[subcriptionPlanId];
            decimal price = _subscriptionPlans[subcriptionPlanId];
            string message = string.Format(Constants.Messages.SubscriptionPlanMessage, subcriptionPlanNumber, price);

            return message;
        }

        /// <summary>
        /// Gets the current subscription plan message.
        /// </summary>
        /// <returns></returns>
        public static string GetCurrentSubscriptionPlanMessage()
        {
            string message = string.Empty;
            if (BaseController.LoggedOnUser != null)
            {
                int index = Array.IndexOf(_subscriptionPlanNumbers, BaseController.LoggedOnUser.BatteriesInPlan * 4);
                decimal price = _subscriptionPlans[index];
                message = string.Format(Constants.Messages.CurrentSubscriptionPlan, BaseController.LoggedOnUser.BatteriesInPlan * 4, price);
            }

            return message;
        }

        /// <summary>
        /// Gets the selected subscription plan message.
        /// </summary>
        /// <returns></returns>
        public static string GetSelectedSubscriptionPlanMessage()
        {
            string message = string.Empty;
            if (BaseController.RegistrationUser != null)
            {
                int index = Array.IndexOf(_subscriptionPlanNumbers, BaseController.RegistrationUser.BatteriesInPlan * 4);
                decimal price = _subscriptionPlans[index];
                message = string.Format(Constants.Messages.CurrentSubscriptionPlan, BaseController.RegistrationUser.BatteriesInPlan * 4, price);
            }

            return message;
        }

        /// <summary>
        /// Gets the subscription plan id.
        /// </summary>
        /// <param name="subscriptionPlan">The subscription plan.</param>
        /// <returns></returns>
        public static int GetSubscriptionPlanId(int subscriptionPlan)
        {
            return Array.IndexOf(_subscriptionPlanNumbers, subscriptionPlan * 4);
        }

        /// <summary>
        /// Gets the bettery packs in plan.
        /// </summary>
        /// <param name="subcriptionPlanId">The subcription plan id.</param>
        /// <returns></returns>
        public static int GetBetteryPacksInPlan(int subcriptionPlanId)
        {
            return _subscriptionPlanNumbers[subcriptionPlanId] / 4;
        }

        /// <summary>
        /// Reads the price by subscription plan id.
        /// </summary>
        /// <param name="subcriptionPlanId">The subcription plan id.</param>
        /// <returns></returns>
        private static decimal ReadPriceBySubscriptionPlanId(int subcriptionPlanId)
        {
            string key = string.Format(Constants.Messages.SubscriptionPlanKey, subcriptionPlanId);
            string value = ConfigurationManager.AppSettings[key];

            decimal price = 0M;
            if (value != null)
            {
                decimal.TryParse(value, out price);
            }

            return price;
        }

        #endregion SubscriptionPlan
    }
}

using System;
using System.Collections.Generic;
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
using Phidgets;
using Phidgets.Events;
using System.Windows.Threading;

namespace Bettery.Kiosk
{
    /// <summary>
    /// Interaction logic for Exchange.xaml
    /// </summary>
    public partial class Exchange : Page
    {
        //Declare an InterfaceKit object
        static Stepper ifKit;

        private int _aaExchange = 0;
        private int _aaaExchange = 0;
        private bool _cartridgeInserted = true;
        
        public Exchange()
        {
            InitializeComponent();
            UPCCode.Focus();
            InitPhidget();

        }


        private void DoneButton_Click(object sender, RoutedEventArgs e)
        {
            // Remove event handler and close Phidget
            ifKit.InputChange -= new InputChangeEventHandler(ifKit_InputChange);
            ifKit.close();
            
            // Go to Return Summary Page
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

        }
        private void LogInButton_Click(object sender, RoutedEventArgs e)
        {

        }
        private void StartOverButton_Click(object sender, RoutedEventArgs e)
        {
            Start page = new Start();
            this.NavigationService.Navigate(page);
        }
        private void Terms_PrivacyButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            Welcome page = new Welcome();
            this.NavigationService.Navigate(page);
        }
        private void InitPhidget()
        {
            try
            {
                //Initialize the InterfaceKit object
                ifKit = new Stepper();

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
        static void ifKit_Error(object sender, ErrorEventArgs e)
        {
            throw new Exception(e.exception.Message);
        }

        //Input Change event handler
        void ifKit_InputChange(object sender, InputChangeEventArgs e)
        {
            // A cartridge has been inserted
            _cartridgeInserted = true;
            
        }
    }
}

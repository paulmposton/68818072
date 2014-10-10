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

namespace Bettery.Kiosk
{
    /// <summary>
    /// Interaction logic for ReturnSummary.xaml
    /// </summary>
    /// 
    
    public partial class ReturnSummary : Page
    {
        private int _aaCartridge;
        public ReturnSummary()
        {
            InitializeComponent();
        }
        public ReturnSummary(int aaCartridge)
        {
            InitializeComponent();
            _aaCartridge = aaCartridge;
            Message.Text = "You have returned " + aaCartridge.ToString() + " AA Batteries and 0 AAA Batteries";
            //aaSummary.Text = aaCartridge.ToString();
        }
        private void YesButton_Click(object sender, RoutedEventArgs e)
        {
            GetBatteries page = new GetBatteries(_aaCartridge);
            this.NavigationService.Navigate(page);
        }

        private void NoButton_Click(object sender, RoutedEventArgs e)
        {
            Checkout page = new Checkout(_aaCartridge, 0, 0);
            this.NavigationService.Navigate(page);
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
    }
}

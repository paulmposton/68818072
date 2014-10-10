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
using System.Windows.Threading;

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
        public GetBatteries()
        {
            InitializeComponent();
            //StartTimer();

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
            _aaCartridge = aaCartridge;
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

        private void RemoveAAButton_Click(object sender, RoutedEventArgs e)
        {
            _aa = 0;
            aaTextbox.Text = _aa.ToString();
        }

        private void RemoveAAAButton_Click(object sender, RoutedEventArgs e)
        {
            _aaa = 0;
            aaaTextbox.Text = _aaa.ToString();
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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using BKiosk.BService;

namespace Bettery.Kiosk
{
    /// <summary>
    /// Interaction logic for Login.xaml
    /// </summary>
    public partial class Login : Page
    {
        public Login()
        {
            InitializeComponent();
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
            Start page = new Start();
            this.NavigationService.Navigate(page);
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            
            
            KioskServiceClient BKiosk = new KioskServiceClient();
            //bool authed = BKiosk.AuthenticateUser(UserName.Text, Password.Text);

        }

    }
}

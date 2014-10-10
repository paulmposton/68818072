﻿using System;
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
    /// Interaction logic for Page2.xaml
    /// </summary>
    public partial class FAQ : Page
    {
        public FAQ()
        {
            InitializeComponent();
        }

        private void YesButton_Click(object sender, RoutedEventArgs e)
        {
            Exchange page = new Exchange();
            this.NavigationService.Navigate(page);
        }
        private void NoButton_Click(object sender, RoutedEventArgs e)
        {
            GetBatteries page = new GetBatteries();
            this.NavigationService.Navigate(page);
        }
        private void FAQButton_Click(object sender, RoutedEventArgs e)
        {

        }
        private void LogInButton_Click(object sender, RoutedEventArgs e)
        {
            Login page = new Login();
            this.NavigationService.Navigate(page);
        }
        private void StartOverButton_Click(object sender, RoutedEventArgs e)
        {
            Start page = new Start();
            this.NavigationService.Navigate(page);
        }
        private void Terms_PrivacyButton_Click(object sender, RoutedEventArgs e)
        {
            Privacy page = new Privacy();
            this.NavigationService.Navigate(page);

        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            Start page = new Start();
            this.NavigationService.GoBack();
        }

    }
}
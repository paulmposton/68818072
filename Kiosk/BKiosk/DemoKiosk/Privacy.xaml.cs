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
    /// Interaction logic for Page2.xaml
    /// </summary>
    public partial class Privacy : Page
    {
        public Privacy()
        {
            InitializeComponent();
        }

        private void Terms_PrivacyButton_Click(object sender, RoutedEventArgs e)
        {
            FAQ page = new FAQ();
            this.NavigationService.GoBack();
        }
        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            Start page = new Start();
            this.NavigationService.GoBack();
        }

    }
}

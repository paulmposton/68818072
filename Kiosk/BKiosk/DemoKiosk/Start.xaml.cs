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
    /// Interaction logic for Page1.xaml
    /// </summary>
    public partial class Start : Page
    {
        public Start()
        {
            InitializeComponent();
        }

        private void GetStartedButton_Click(object sender, RoutedEventArgs e)
        {
                        Welcome page = new Welcome();
            this.NavigationService.Navigate(page);
            
        }
    }
}

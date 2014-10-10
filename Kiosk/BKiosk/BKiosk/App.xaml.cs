using System;
using System.Windows;
using System.Windows.Controls;
using Bettery.Kiosk;
using System.Configuration;

namespace BKiosk
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
       public App()
       {
           //  TODO: storre all this in a config file
           string serviceBusconnectionString = "Endpoint=sb://BTransactionService-Dev.servicebus.windows.net/;SharedSecretIssuer=owner;SharedSecretValue=gkGyPUVrW/BHynikrHJAZrFzXTjhveyR0fucpbU18OQ=";
           Application.Current.Properties["serviceBusconnectionString"] = serviceBusconnectionString;
           string apiLogin = "52Hzc4Ac4LP";
           string transactionKey = "5N3M567We9g4bGgQ";
           Application.Current.Properties["apiLogin"] = apiLogin;
           Application.Current.Properties["transactionKey"] = transactionKey;

           // Init timer for timeout processing
           int maxIdleTime = Convert.ToInt32(ConfigurationManager.AppSettings["MaxIdleTime"]);
           BaseController.InitTimeoutTimer(maxIdleTime);
           BaseController.OnSessionTimeout += new EventHandler(OnSessionTimeout);
       }

       /// <summary>
       /// Called when [session timeout].
       /// </summary>
       /// <param name="sender">The sender.</param>
       /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
       protected void OnSessionTimeout(object sender, EventArgs e)
       {
           //Navigate to the start page
           if (sender != null)
           {
               //Navigate to start page
               Start page = new Start();
               BaseController.CurrentPage.NavigationService.Navigate(page);

               //Reset timeout state
               BaseController.CurrentPage = null;
               BaseController.ResetLastActiveTime();
           }
       }
   }
}

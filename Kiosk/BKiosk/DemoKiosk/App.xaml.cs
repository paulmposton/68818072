using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Windows;

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
       }
   }
}

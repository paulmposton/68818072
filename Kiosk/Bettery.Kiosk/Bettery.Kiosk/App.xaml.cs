using System;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using Bettery.Kiosk.Common;
using Bettery.Kiosk.Controllers;

namespace Bettery.Kiosk
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="App" /> class.
        /// </summary>
        public App()
        {

        }

        /// <summary>
        /// Raises the <see cref="E:System.Windows.Application.Startup"/> event.
        /// </summary>
        /// <param name="e">A <see cref="T:System.Windows.StartupEventArgs"/> that contains the event data.</param>
        protected override void OnStartup(StartupEventArgs e)
        {
            bool appRunning = IsAppAlreadyRunning();

            //
            //  Log a report on inventory here, on startup
            //
            Logger.Log(EventLogEntryType.Warning, "Bkiosk Application Startup", BaseController.StationId);
            if (!appRunning)
            {
                base.OnStartup(e);
            }
        }

        /// <summary>
        /// Raises the <see cref="E:System.Windows.Application.OnExit"/> event.
        /// </summary>
        /// <param name="e">A <see cref="T:System.Windows.StartupEventArgs"/> that contains the event data.</param>
        protected override void OnExit(ExitEventArgs e)
        {
            //
            //  Log an app exit.
            //
            Logger.Log(EventLogEntryType.Warning, "BKiosk Application Exit", BaseController.StationId);

            base.OnExit(e);
        }


        /// <summary>
        /// Determines whether [is app already running].
        /// </summary>
        private static bool IsAppAlreadyRunning()
        {
            Process currentProcess = Process.GetCurrentProcess();

            if (Process.GetProcessesByName(currentProcess.ProcessName).Any(p => p.Id != currentProcess.Id))
            {
                Current.Shutdown();
                return true;
            }

            return false;
        }

        /// <summary>
        /// Gets or sets a value indicating whether [do handle].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [do handle]; otherwise, <c>false</c>.
        /// </value>
        public bool DoHandle { get; set; }

        /// <summary>
        /// Handles the Startup event of the Application control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.StartupEventArgs"/> instance containing the event data.</param>
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
        }

        /// <summary>
        /// Handles the DispatcherUnhandledException event of the Application control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.Threading.DispatcherUnhandledExceptionEventArgs"/> instance containing the event data.</param>
        private void Application_DispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            if (DoHandle)
            {
                //Handling the exception within the UnhandledException handler.

                try
                {
                    Logger.Log(EventLogEntryType.Error, e.Exception, BaseController.StationId);
                }
                catch
                {
                    Debug.WriteLine(e.Exception.Message);
                }

                e.Handled = true;
            }
            else
            {
                try
                {
                    Logger.Log(EventLogEntryType.Error, e.Exception, BaseController.StationId);
                }
                catch
                {
                    Debug.WriteLine(e.Exception.Message);
                }

                //If you do not set e.Handled to true, the application will close due to crash.
                e.Handled = true;

                // 1st way to shut down: kill process
                Process.GetCurrentProcess().Kill();

                // 2nd way to shut down: normal procedure
                // Shutdown();

                // 3rd way to shut down: call terminal process of windows
                // Environment.Exit(0);
            }
        }

        /// <summary>
        /// Handles the UnhandledException event of the CurrentDomain control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.UnhandledExceptionEventArgs"/> instance containing the event data.</param>
        private void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            Exception ex = e.ExceptionObject as Exception;
            if (ex == null)
            {
                return;
            }

            try
            {
                Logger.Log(EventLogEntryType.Error, ex, BaseController.StationId);
            }
            catch
            {
                Debug.WriteLine(ex.Message);
            }
        }
    }
}
using System;
using System.Configuration;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using BKiosk;
using BKiosk.HelperClasses;

namespace Bettery.Kiosk
{
    /// <summary>
    /// class Base Controller
    /// </summary>
    public class BaseController
    {
        private static int _maxIdleTime;

        /// <summary>
        /// Local Sql connection string
        /// </summary>
        public readonly static string ConnectionString = ConfigurationManager.AppSettings["ConnectionString"];

        /// <summary>
        /// Gets or sets the on session timeout.
        /// </summary>
        /// <value>
        /// The on session timeout.
        /// </value>
        public static EventHandler OnSessionTimeout { get; set; }

        /// <summary>
        /// Gets or sets the last active time.
        /// </summary>
        /// <value>
        /// The last active time.
        /// </value>
        public static DateTime LastActiveTime { get; set; }

        /// <summary>
        /// Gets or sets the current page.
        /// </summary>
        /// <value>
        /// The current page.
        /// </value>
        public static Page CurrentPage { get; set; }

        /// <summary>
        /// Gets or sets the previos page.
        /// </summary>
        /// <value>
        /// The previos page.
        /// </value>
        public static Page PreviousPage { get; set; }

        /// <summary>
        /// Gets or sets the timeout timer.
        /// </summary>
        /// <value>
        /// The timeout timer.
        /// </value>
        public static Timer TimeoutTimer { get; set; }

        /// <summary>
        /// Gets or sets the user.
        /// </summary>
        /// <value>
        /// The user.
        /// </value>
        public static User LoggedOnUser { get; set; }

        /// <summary>
        /// Gets a value indicating whether this instance is login.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is login; otherwise, <c>false</c>.
        /// </value>
        public static bool IsLoggedOnUser
        {
            get
            {
                if (LoggedOnUser == null)
                {
                    return false;
                }

                return true;
            }
        }

        /// <summary>
        /// Logouts this instance.
        /// </summary>
        public static void Logout()
        {
            LoggedOnUser = null;

            EventHandler handler = OnSessionTimeout;
            if (handler != null)
            {
                App.Current.Dispatcher.BeginInvoke(new Action(() => { handler(CurrentPage, EventArgs.Empty); }));
            }
        }

        /// <summary>
        /// Validates the membership inputs.
        /// </summary>
        /// <param name="firstname">The firstname.</param>
        /// <param name="lastname">The lastname.</param>
        /// <param name="email">The email.</param>
        /// <param name="password">The password.</param>
        /// <param name="confirmPassword">The confirm password.</param>
        /// <param name="zipcode">The zipcode.</param>
        /// <returns></returns>
        public static bool ValidateMembershipInputs(string firstname, string lastname, string email, string password, string confirmPassword, string zipcode)
        {
            if (firstname.Length == 0)
            {
                return false;
            }

            if (lastname.Length == 0)
            {
                return false;
            }

            if (email.Length == 0)
            {
                return false;
            }

            if (password.Length == 0)
            {
                return false;
            }

            if (password != confirmPassword)
            {
                return false;
            }

            bool zipcodeValid = ValidateZipcode(zipcode);
            if (!zipcodeValid)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Validates the zipcode.
        /// </summary>
        /// <param name="zipcode">The zipcode.</param>
        /// <returns></returns>
        public static bool ValidateZipcode(string zipcode)
        {
            if (zipcode.Length != 5)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Resets the last active time.
        /// </summary>
        public static void ResetLastActiveTime()
        {
            LastActiveTime = DateTime.Now;
        }

        /// <summary>
        /// Inits the timeout timer.
        /// </summary>
        public static void InitTimeoutTimer(int maxIdleTime)
        {
            _maxIdleTime = maxIdleTime;
            ResetLastActiveTime();
            TimeoutTimer = new Timer
            {
                Enabled = true,
                AutoReset = true,
                Interval = 10000
            };

            TimeoutTimer.Elapsed += new ElapsedEventHandler(TimeoutTimer_Elapsed);
        }

        /// <summary>
        /// Handles the Elapsed event of the TimeoutTimer control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Timers.ElapsedEventArgs"/> instance containing the event data.</param>
        protected static void TimeoutTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            bool isSessionTimeout = IsSessionTimeout(_maxIdleTime);

            if (isSessionTimeout)
            {
                Logout();
            }
        }

        /// <summary>
        /// Determines whether [is session timeout] [the specified max idle seconds].
        /// </summary>
        /// <param name="maxIdleSeconds">The max idle seconds.</param>
        /// <returns>
        ///   <c>true</c> if [is session timeout] [the specified max idle seconds]; otherwise, <c>false</c>.
        /// </returns>
        private static bool IsSessionTimeout(int maxIdleSeconds)
        {
            TimeSpan idleTimeSpan = DateTime.Now - LastActiveTime;

            return (idleTimeSpan.TotalSeconds >= maxIdleSeconds);
        }
    }
}
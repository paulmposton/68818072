using System;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using Bettery.Kiosk.Entities;

namespace Bettery.Kiosk.Common
{
    /// <summary>
    /// class of Logger
    /// </summary>
    public class Logger
    {
        /// <summary>
        /// Logs the specified type.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="message">The message.</param>
        /// <param name="stationId">The station id.</param>
        public static void Log(EventLogEntryType type, string message, string stationId)
        {
            Log(type, string.Empty, message, string.Empty, stationId);
        }

        /// <summary>
        /// Logs the specified type.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="ex">The ex.</param>
        /// <param name="stationId">The station id.</param>
        public static void Log(EventLogEntryType type, Exception ex, string stationId)
        {
            Log(type, string.Empty, ex.Message, ex.StackTrace, stationId);
        }

        /// <summary>
        /// Logs the specified type.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="ex">The ex.</param>
        /// <param name="stationId">The station id.</param>
        public static void Log(EventLogEntryType type, KioskException ex, string stationId)
        {
            if (ex.OriginalException != null)
            {
                Log(type, string.Empty, ex.Message, ex.StackTrace, stationId);
            }
            else
            {
                Log(type, string.Empty, ex.CustomMessage, string.Empty, stationId);
            }
        }

        /// <summary>
        /// Logs the specified type.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="source">The source.</param>
        /// <param name="message">The message.</param>
        /// <param name="stackTrace">The stack trace.</param>
        /// <param name="stationId">The station id.</param>
        public static void Log(EventLogEntryType type, string source, string message, string stackTrace, string stationId)
        {
            try
            {
                // Write to file
                WriteToFile(type, message, stackTrace, stationId);

                // Write to EventLog
                bool log = false;

                if (ConfigurationManager.AppSettings["WriteEventLog"] != null)
                {
                    log = Convert.ToBoolean(ConfigurationManager.AppSettings["WriteEventLog"]);
                }

                if (log)
                {
                    EventLog entry = new EventLog();
                    entry.Source = "BKiosk_" + stationId;

                    if (!string.IsNullOrEmpty(source))
                    {
                        entry.Source += " (" + source + ")";
                    }

                    entry.WriteEntry(message, type);
                }
            }
            catch
            {
                Debug.Write(message);
            }
        }

        /// <summary>
        /// Writes to file.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="message">The message.</param>
        /// <param name="stackTrace">The stack trace.</param>
        /// <param name="stationId">The station id.</param>
        private static void WriteToFile(EventLogEntryType type, string message, string stackTrace, string stationId)
        {
            try
            {
                DateTime currentDateTime = DateTime.Now;

                string logFile = string.Format("logs\\logfile_{0}.txt", currentDateTime.ToString("MM_dd_yyyy"));

                using (StreamWriter log = new StreamWriter(logFile, true))
                {
                    log.WriteLine(type.ToString() + "_" + currentDateTime.ToString("MM/dd/yyyy hh:mm:ss ---------------------------------------"));
                    log.WriteLine(message);
                    log.WriteLine(stackTrace);
                    log.WriteLine();
                }
            }
            catch
            {
                Debug.WriteLine(message);
            }
        }
    }
}
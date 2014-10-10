using System;

namespace Bettery.Kiosk.Entities
{
    /// <summary>
    /// Class KioskException
    /// </summary>
    public class KioskException : ApplicationException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="KioskException" /> class.
        /// </summary>
        /// <param name="customMessage">The custom message.</param>
        public KioskException(string customMessage) : base(customMessage)
        {
            CustomMessage = customMessage;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="KioskException"/> class.
        /// </summary>
        /// <param name="customMessage">The custom message.</param>
        /// <param name="originalException">The original exception.</param>
        public KioskException(string customMessage, Exception originalException)
            : base(customMessage)
        {
            CustomMessage = customMessage;
            OriginalException = originalException;
        }

        /// <summary>
        /// Gets or sets the custom message.
        /// </summary>
        /// <value>
        /// The custom message.
        /// </value>
        public string CustomMessage { get; set; }

        /// <summary>
        /// Gets or sets the original exception.
        /// </summary>
        /// <value>
        /// The original exception.
        /// </value>
        public Exception OriginalException { get; set; }
    }
}
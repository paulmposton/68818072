using System;

namespace Bettery.Kiosk.Entities
{
    /// <summary>
    /// Class Vend EventArgs
    /// </summary>
    public class VendEventArgs : EventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="VendEventArgs" /> class.
        /// </summary>
        /// <param name="totalProductAA">The total product AA.</param>
        /// <param name="totalProductAAA">The total product AAA.</param>
        public VendEventArgs(int totalProductAA, int totalProductAAA)
        {
            TotalProductAA = totalProductAA;
            TotalProductAAA = totalProductAAA;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="VendEventArgs" /> class.
        /// </summary>
        /// <param name="totalVendingEmptyCases">The total vending empty cases.</param>
        public VendEventArgs(int totalVendingEmptyCases)
        {
            TotalVendingEmptyCases = totalVendingEmptyCases;
        }

        /// <summary>
        /// Gets or sets the vended empty cases.
        /// </summary>
        /// <value>
        /// The vended empty cases.
        /// </value>
        public int VendedEmptyCases { get; set; }

        /// <summary>
        /// Gets or sets the total vending empty cases.
        /// </summary>
        /// <value>
        /// The total vending empty cases.
        /// </value>
        public int TotalVendingEmptyCases { get; set; }

        /// <summary>
        /// Gets or sets the current product AA.
        /// </summary>
        /// <value>
        /// The current product AA.
        /// </value>
        public int CurrentProductAA { get; set; }

        /// <summary>
        /// Gets or sets the current product AAA.
        /// </summary>
        /// <value>
        /// The current product AAA.
        /// </value>
        public int CurrentProductAAA { get; set; }

        /// <summary>
        /// Gets or sets the total product AA.
        /// </summary>
        /// <value>
        /// The total product AA.
        /// </value>
        public int TotalProductAA { get; set; }

        /// <summary>
        /// Gets or sets the total product AAA.
        /// </summary>
        /// <value>
        /// The total product AAA.
        /// </value>
        public int TotalProductAAA { get; set; }
    }
}
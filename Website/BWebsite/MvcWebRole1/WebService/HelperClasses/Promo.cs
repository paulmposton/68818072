using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebRole1.HelperClasses
{
    public class Promo
    {
        /// <summary>
        /// Gets or sets the Promo Type.
        /// </summary>
        /// <value>
        /// Promo Type.
        /// </value>
        public int PromoType { get; set; }

        /// <summary>
        /// Gets or sets the Amount.
        /// </summary>
        /// <value>
        /// Amount.
        /// </value>
        public decimal Amount{ get; set; }
    }
}
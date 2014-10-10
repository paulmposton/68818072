using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebRole1.HelperClasses
{
    public class BetteryMember
    {

        /// <summary>
        /// Gets or sets the First Name.
        /// </summary>
        /// <value>
        /// First Name.
        /// </value>
        public string MemberFirstName { get; set; }

        /// <summary>
        /// Gets or sets the Last Name.
        /// </summary>
        /// <value>
        /// Last Name.
        /// </value>
        public string MemberLastName { get; set; }

        /// <summary>
        /// Gets or sets the Outstanding Credit.
        /// </summary>
        /// <value>
        /// Outstanding Credit.
        /// </value>
        public decimal AccountBalance { get; set; }

        /// <summary>
        /// Gets or sets how many batteries are currently checked out.
        /// </summary>
        /// <value>
        /// Batteries Checked Out.
        /// </value>
        public int BatteryPacksCheckedOut { get; set; }

        /// <summary>
        /// Gets or sets how many batteries are in the Member's Subscription Plan.
        /// </summary>
        /// <value>
        /// Batteries In Subscription Plan.
        /// </value>
        public int BatteryPacksInPlan { get; set; }

        /// <summary>
        /// Gets or sets the MemberID.
        /// </summary>
        /// <value>
        /// Member ID.
        /// </value>
        public int MemberID { get; set; }

        /// <summary>
        /// Gets or sets the Customer Profile ID.
        /// </summary>
        /// <value>
        /// Customer Profile ID.
        /// </value>
        public string CustomerProfileID { get; set; }

        /// <summary>
        /// Gets or sets the Customer Profile ID.
        /// </summary>
        /// <value>
        /// Customer Profile ID.
        /// </value>
        public string PaymentProfileID { get; set; }
        
        /// <summary>
        /// Gets or sets the Customer Profile ID.
        /// </summary>
        /// <value>
        /// Customer Profile ID.
        /// </value>
        public int MemberTotalBatteries { get; set; }

        /// <summary>
        /// Gets or sets the Customer Profile ID.
        /// </summary>
        /// <value>
        /// Customer Profile ID.
        /// </value>
        public int FreeCases { get; set; }

        /// <summary>
        /// Gets or sets the Credit Card Expiration Date.
        /// </summary>
        /// <value>
        /// Credit Card Expire Date.
        /// </value>
        public string CCExPireDate { get; set; }

        /// <summary>
        /// Gets or sets the Credit Card last four digits .
        /// </summary>
        /// <value>
        /// Credit Card Last Four.
        /// </value>
        public string CCLastFourDigits { get; set; }

        /// <summary>
        /// Gets or sets the GroupID.
        /// </summary>
        /// <value>
        /// Group ID
        /// </value>
        public int GroupID { get; set; }


        
    }
}
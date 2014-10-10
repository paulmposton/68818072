namespace Bettery.Kiosk.Entities
{
    /// <summary>
    /// Class Bettery User
    /// </summary>
    public class BetteryUser
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BetteryUser" /> class.
        /// </summary>
        public BetteryUser()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BetteryUser" /> class.
        /// </summary>
        /// <param name="username">The username.</param>
        /// <param name="password">The password.</param>
        public BetteryUser(string username, string password)
        {
            Email = username;
            Password = password;
        }

        /// <summary>
        /// Gets the name of the user.
        /// </summary>
        /// <value>
        /// The name of the user.
        /// </value>
        public string UserName { get { return Email; } }

        /// <summary>
        /// Gets or sets the password.
        /// </summary>
        /// <value>
        /// The password.
        /// </value>
        public string Password { get; set; }

        /// <summary>
        /// Gets or sets the password Digest.
        /// </summary>
        /// <value>
        /// The password digest.
        /// </value>
        public string PasswordDigest { get; set; }

        /// <summary>
        /// Gets or sets the batteries checked out.
        /// </summary>
        /// <value>
        /// The batteries checked out.
        /// </value>
        public int BatteriesCheckedOut { get; set; }

        /// <summary>
        /// Gets or sets the batteries in plan.
        /// </summary>
        /// <value>
        /// The batteries in plan.
        /// </value>
        public int BatteriesInPlan { get; set; }

        /// <summary>
        /// Gets or sets the customer profile ID.
        /// </summary>
        /// <value>
        /// The customer profile ID.
        /// </value>
        public string CustomerProfileId { get; set; }

        /// <summary>
        /// Gets or sets the Payment profile ID.
        /// </summary>
        /// <value>
        /// The customer profile ID.
        /// </value>
        public string PaymentProfileId { get; set; }

        /// <summary>
        /// Gets or sets the free cases remaining.
        /// </summary>
        /// <value>
        /// The free cases remaining.
        /// </value>
        public int FreeCasesRemaining { get; set; }

        /// <summary>
        /// Gets or sets the member first name field.
        /// </summary>
        /// <value>
        /// The member first name field.
        /// </value>
        public string MemberFirstName { get; set; }

        /// <summary>
        /// Gets or sets the member ID.
        /// </summary>
        /// <value>
        /// The member ID.
        /// </value>
        public int MemberId { get; set; }

        /// <summary>
        /// Gets or sets the member last name field.
        /// </summary>
        /// <value>
        /// The member last name field.
        /// </value>
        public string MemberLastName { get; set; }

        /// <summary>
        /// Gets or sets the member total batteries.
        /// </summary>
        /// <value>
        /// The member total batteries.
        /// </value>
        public int MemberTotalBatteries { get; set; }

        /// <summary>
        /// Gets or sets the outstanding credit field.
        /// </summary>
        /// <value>
        /// The outstanding credit field.
        /// </value>
        public decimal OutstandingCredit { get; set; }

        /// <summary>
        /// Gets or sets the email.
        /// </summary>
        /// <value>
        /// The email.
        /// </value>
        public string Email { get; set; }

        /// <summary>
        /// Gets or sets the zip code.
        /// </summary>
        /// <value>
        /// The zip code.
        /// </value>
        public string ZipCode { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [get emails].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [get emails]; otherwise, <c>false</c>.
        /// </value>
        public bool GetEmails { get; set; }

        /// <summary>
        /// Gets or sets the new batteries in plan.
        /// </summary>
        /// <value>
        /// The new batteries in plan.
        /// </value>
        public int NewBatteriesInPlan { get; set; }

        /// <summary>
        /// Gets or sets the Credit Card Expiration Date.
        /// </summary>
        /// <value>
        /// Credit Card Expire Date.
        /// </value>
        public string CCOnFileExPireDate { get; set; }

        /// <summary>
        /// Gets or sets the Credit Card last four digits .
        /// </summary>
        /// <value>
        /// Credit Card Last Four.
        /// </value>
        public string CCOnFileLastFourDigits { get; set; }

        /// <summary>
        /// Gets or sets the GroupID .
        /// </summary>
        /// <value>
        /// GroupID.
        /// </value>
        public int GroupID { get; set; }
    }
}
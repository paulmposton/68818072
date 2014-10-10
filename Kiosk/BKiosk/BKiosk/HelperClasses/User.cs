namespace BKiosk.HelperClasses
{
    /// <summary>
    /// Class User
    /// </summary>
    public class User
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="User" /> class.
        /// </summary>
        public User()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="User" /> class.
        /// </summary>
        /// <param name="username">The username.</param>
        /// <param name="password">The password.</param>
        public User(string username, string password)
        {
            UserName = username;
            Password = password;
        }

        /// <summary>
        /// Gets or sets the name of the user.
        /// </summary>
        /// <value>
        /// The name of the user.
        /// </value>
        public string UserName { get; set; }

        /// <summary>
        /// Gets or sets the password.
        /// </summary>
        /// <value>
        /// The password.
        /// </value>
        public string Password { get; set; }

        /// <summary>
        /// Gets or sets the batteries checked out.
        /// </summary>
        /// <value>
        /// The batteries checked out.
        /// </value>
        public int BatteriesCheckedOut { get; set; }

        /// <summary>
        /// Gets or sets how many batteries are in the Member's Subscription Plan.
        /// </summary>
        /// <value>
        /// Batteries In Subscription Plan.
        /// </value>
        public int BatteriesInPlan { get; set; }

        /// <summary>
        /// Gets or sets the customer profile ID.
        /// </summary>
        /// <value>
        /// The customer profile ID.
        /// </value>
        public string CustomerProfileID { get; set; }

        /// <summary>
        /// Gets or sets the member ID.
        /// </summary>
        /// <value>
        /// The member ID.
        /// </value>
        public int MemberID { get; set; }

        /// <summary>
        /// Gets or sets the first name field.
        /// </summary>
        /// <value>
        /// The first name field.
        /// </value>
        public string FirstName { get; set; }

        /// <summary>
        /// Gets or sets the last name field.
        /// </summary>
        /// <value>
        /// The member name field.
        /// </value>
        public string LastName { get; set; }

        /// <summary>
        /// Gets or sets the outstanding credit field.
        /// </summary>
        /// <value>
        /// The outstanding credit field.
        /// </value>
        public decimal OutstandingCredit { get; set; }

        /// <summary>
        /// Gets or sets the zipcode.
        /// </summary>
        /// <value>
        /// The zipcode.
        /// </value>
        public uint Zipcode { get; set; }

        /// <summary>
        /// Gets or sets the email.
        /// </summary>
        /// <value>
        /// The email.
        /// </value>
        public string Email { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is email subscription.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is email subscription; otherwise, <c>false</c>.
        /// </value>
        public bool IsEmailSubscription { get; set; }

        /// <summary>
        /// Gets or sets the subscription plan.
        /// </summary>
        /// <value>
        /// The subscription plan.
        /// </value>
        public int SubscriptionPlan { get; set; }
    }
}
namespace Bettery.Kiosk.Controllers
{
    /// <summary>
    /// Class MembershipSubscription Controller
    /// </summary>
    public sealed class MembershipSubscriptionController
    {
        /// <summary>
        /// Initializes the <see cref="MembershipSubscriptionController" /> class.
        /// </summary>
        static MembershipSubscriptionController()
        {
            SubscriptionPlan1Message = BaseController.GetSubscriptionPlanMessage(1);
            SubscriptionPlan2Message = BaseController.GetSubscriptionPlanMessage(2);
            SubscriptionPlan3Message = BaseController.GetSubscriptionPlanMessage(3);
            SubscriptionPlan4Message = BaseController.GetSubscriptionPlanMessage(4);
            SubscriptionPlan5Message = BaseController.GetSubscriptionPlanMessage(5);
            SubscriptionPlan6Message = BaseController.GetSubscriptionPlanMessage(6);
        }

        /// <summary>
        /// Gets the subscription plan1 message.
        /// </summary>
        /// <value>
        /// The subscription plan1 message.
        /// </value>
        public static string SubscriptionPlan1Message { get; private set; }

        /// <summary>
        /// Gets the subscription plan2 message.
        /// </summary>
        /// <value>
        /// The subscription plan2 message.
        /// </value>
        public static string SubscriptionPlan2Message { get; private set; }

        /// <summary>
        /// Gets the subscription plan3 message.
        /// </summary>
        /// <value>
        /// The subscription plan3 message.
        /// </value>
        public static string SubscriptionPlan3Message { get; private set; }

        /// <summary>
        /// Gets the subscription plan4 message.
        /// </summary>
        /// <value>
        /// The subscription plan4 message.
        /// </value>
        public static string SubscriptionPlan4Message { get; private set; }

        /// <summary>
        /// Gets the subscription plan5 message.
        /// </summary>
        /// <value>
        /// The subscription plan5 message.
        /// </value>
        public static string SubscriptionPlan5Message { get; private set; }

        /// <summary>
        /// Gets the subscription plan6 message.
        /// </summary>
        public static string SubscriptionPlan6Message { get; private set; }
    }
}
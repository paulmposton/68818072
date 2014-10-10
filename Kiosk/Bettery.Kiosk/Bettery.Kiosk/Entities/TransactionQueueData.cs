namespace Bettery.Kiosk.Entities
{
    /// <summary>
    /// Class Transaction Queue Data
    /// </summary>
    public class TransactionQueueData
    {
        /// <summary>
        /// Gets or sets the order number.
        /// </summary>
        /// <value>
        /// The order number.
        /// </value>
        public string OrderNumber { get; set; }

        /// <summary>
        /// Gets or sets the Sub Total amount.
        /// </summary>
        /// <value>
        /// The Sub Total amount.
        /// </value>
        public decimal SubTotalAmount { get; set; }

        /// <summary>
        /// Gets or sets the charge amount.
        /// </summary>
        /// <value>
        /// The charge amount.
        /// </value>
        public decimal ChargeAmount { get; set; }

        /// <summary>
        /// Gets or sets the tax amount.
        /// </summary>
        /// <value>
        /// The tax amount.
        /// </value>
        public decimal TaxAmount { get; set; }

        /// <summary>
        /// Gets or sets the Name on Card.
        /// </summary>
        /// <value>
        /// The Name on the Credit Card.
        /// </value>
        public string NameOnCard { get; set; }

        /// <summary>
        /// Gets or sets the CardInfo.
        /// </summary>
        /// <value>
        /// The CardInfo.
        /// </value>
        public string CardInfo { get; set; }


        /// <summary>
        /// Gets or sets the Card Hash.
        /// </summary>
        /// <value>
        /// The Card Hash.
        /// </value>
        public string CardHash { get; set; }


        /// <summary>
        /// Gets or sets the email.
        /// </summary>
        /// <value>
        /// The email.
        /// </value>
        public string Email { get; set; }

        /// <summary>
        /// Gets or sets the customer profile id.
        /// </summary>
        /// <value>
        /// The customer profile id.
        /// </value>
        public string CustomerProfileId { get; set; }

        /// <summary>
        /// Gets or sets the customer profile id.
        /// </summary>
        /// <value>
        /// The Payment profile id.
        /// </value>
        public string PaymentProfileID { get; set; }

        /// <summary>
        /// Gets or sets the aa vend.
        /// </summary>
        /// <value>
        /// The aa vend.
        /// </value>
        public int AaVend { get; set; }

        /// <summary>
        /// Gets or sets the aaa vend.
        /// </summary>
        /// <value>
        /// The aaa vend.
        /// </value>
        public int AaaVend { get; set; }

        /// <summary>
        /// Gets or sets the aa forgot vend.
        /// </summary>
        /// <value>
        /// The aa forgot vend.
        /// </value>
        public int AaForgotVend { get; set; }

        /// <summary>
        /// Gets or sets the aaa forgot vend.
        /// </summary>
        /// <value>
        /// The aaa forgot vend.
        /// </value>
        public int AaaForgotVend { get; set; }

        /// <summary>
        /// Gets or sets the aa return.
        /// </summary>
        /// <value>
        /// The aa return.
        /// </value>
        public int AaReturn { get; set; }

        /// <summary>
        /// Gets or sets the aaa return.
        /// </summary>
        /// <value>
        /// The aaa return.
        /// </value>
        public int AaaReturn { get; set; }

        /// <summary>
        /// Gets or sets the authorization.
        /// </summary>
        /// <value>
        /// The authorization.
        /// </value>
        public string Authorization { get; set; }

        /// <summary>
        /// Gets or sets the transaction id.
        /// </summary>
        /// <value>
        /// The Transaction ID.
        /// </value>
        public string TransactionID { get; set; }


        /// <summary>
        /// Gets or sets the promo code.
        /// </summary>
        /// <value>
        /// The promo code.
        /// </value>
        public string PromoCode { get; set; }

        /// <summary>
        /// Gets or sets the promo code.
        /// </summary>
        /// <value>
        /// The promo code Amount.
        /// </value>
        public decimal PromoCodeAmount { get; set; }

        /// <summary>
        /// Gets or sets the BatteryPacksCheckedOut.
        /// </summary>
        /// <value>
        /// The Number Battery Packs Checked Out.
        /// </value>
        public int BatteryPacksCheckedOut { get; set; }


        /// <summary>
        /// Gets or sets the zip code.
        /// </summary>
        /// <value>
        /// The zip code.
        /// </value>
        public string ZipCode { get; set; }
    }
}
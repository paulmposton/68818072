namespace Bettery.Kiosk.Entities
{
    /// <summary>
    /// Class Bettery Vend
    /// </summary>
    public class BetteryVend
    {
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
        public int AaVendRemaining { get; set; }

        /// <summary>
        /// Gets or sets the aa forgot drained vend.
        /// </summary>
        /// <value>
        /// The aa forgot drained vend.
        /// </value>
        public int AaForgotDrainedVend { get; set; }

        /// <summary>
        /// Gets or sets the aaa vend.
        /// </summary>
        /// <value>
        /// The aaa vend.
        /// </value>
        public int AaaVend { get; set; }

        /// <summary>
        /// Gets or sets the aaa vend.
        /// </summary>
        /// <value>
        /// The aaa vend.
        /// </value>
        public int AaaVendRemaining { get; set; }

        /// <summary>
        /// Gets or sets the aaa forgot drained vend.
        /// </summary>
        /// <value>
        /// The aaa forgot drained vend.
        /// </value>
        public int AaaForgotDrainedVend { get; set; }

        /// <summary>
        /// Gets the total cartridges.
        /// </summary>
        /// <value>
        /// The total cartridges.
        /// </value>
        public int TotalVendCartridges
        {
            get { return AaVend + AaaVend; }
        }

        /// <summary>
        /// Gets the total forgot drained vend cartridges.
        /// </summary>
        public int TotalForgotDrainedVendCartridges
        {
            get { return AaForgotDrainedVend + AaaForgotDrainedVend; }
        }

        /// <summary>
        /// Gets the new batteries.
        /// </summary>
        /// <value>
        /// The new batteries.
        /// </value>
        public int NewBatteries
        {
            get { return TotalVendCartridges * 4; }
        }

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
        /// Gets the return cartridges.
        /// </summary>
        /// <value>
        /// The return cartridges.
        /// </value>
        public int ReturnedCartridges
        {
            get { return AaReturn + AaaReturn; }
        }

        /// <summary>
        /// Gets the returned batteries.
        /// </summary>
        /// <value>
        /// The returned batteries.
        /// </value>
        public int ReturnedBatteries
        {
            get { return ReturnedCartridges * 4; }
        }

        /// <summary>
        /// Gets the new cartridges.
        /// </summary>
        /// <value>
        /// The new cartridges.
        /// </value>
        public int NewCartridges
        {
            get { return TotalVendCartridges - ReturnedCartridges; }
        }

        /// <summary>
        /// Gets or sets the swapped.
        /// </summary>
        /// <value>
        /// The swapped.
        /// </value>
        public int Swapped { get; set; }

        /// <summary>
        /// Gets or sets the new.
        /// </summary>
        /// <value>
        /// The new.
        /// </value>
        public int CalculatedNew { get; set; }

        /// <summary>
        /// Gets or sets the calculated new amount.
        /// </summary>
        /// <value>
        /// The calculated new amount.
        /// </value>
        public decimal CalculatedNewAmount { get; set; }

        /// <summary>
        /// Gets or sets the returned.
        /// </summary>
        /// <value>
        /// The returned.
        /// </value>
        public int CalculatedReturned { get; set; }

        /// <summary>
        /// Gets or sets the calculated returned amount.
        /// </summary>
        /// <value>
        /// The calculated returned amount.
        /// </value>
        public decimal CalculatedReturnedAmount { get; set; }

        /// <summary>
        /// Gets or sets the swapped amount.
        /// </summary>
        /// <value>
        /// The swapped amount.
        /// </value>
        public decimal SwappedAmount { get; set; }

        /// <summary>
        /// Gets or sets the aa amount.
        /// </summary>
        /// <value>
        /// The aa amount.
        /// </value>
        public decimal CalculatedAaAmount { get; set; }

        /// <summary>
        /// Gets or sets the aa Tax amount.
        /// </summary>
        /// <value>
        /// The aa tax amount.
        /// </value>
        public decimal CalculatedAaTaxAmount { get; set; }

        /// <summary>
        /// Gets or sets the aa new forgot drained amount.
        /// </summary>
        /// <value>
        /// The aa new forgot drained amount.
        /// </value>
        public decimal AaNewForgotDrainedAmount { get; set; }

        /// <summary>
        /// Gets or sets the aaa amount.
        /// </summary>
        /// <value>
        /// The aaa amount.
        /// </value>
        public decimal CalculatedAaaAmount { get; set; }

        /// <summary>
        /// Gets or sets the aaa Tax amount.
        /// </summary>
        /// <value>
        /// The aaa tax amount.
        /// </value>
        public decimal CalculatedAaaTaxAmount { get; set; }

        /// <summary>
        /// Gets or sets the aaa new forgot drained amount.
        /// </summary>
        /// <value>
        /// The aaa new forgot drained amount.
        /// </value>
        /// 
        public decimal AaaNewForgotDrainedAmount { get; set; }

        /// <summary>
        /// Gets or sets the new amount.
        /// </summary>
        /// <value>
        /// The new amount.
        /// </value>
        public decimal NewAmount
        {
            get { return CalculatedAaAmount + CalculatedAaaAmount; }
        }

        /// <summary>
        /// Gets the new forgot drained amount.
        /// </summary>
        public decimal TotalForgotDrainedAmount
        {
            get { return AaNewForgotDrainedAmount + AaaNewForgotDrainedAmount; }
        }

        /// <summary>
        /// Gets or sets the tax amount.
        /// </summary>
        /// <value>
        /// The tax amount.
        /// </value>
        public decimal TotalTaxAmount { get; set; }

        /// <summary>
        /// Gets or sets the Deposit total amount.
        /// </summary>
        /// <value>
        /// The Subtotal amount.
        /// </value>
        public decimal DepositAmount { get; set; }

        /// <summary>
        /// Gets or sets the SubSub total amount.
        /// </summary>
        /// <value>
        /// The Subtotal amount.
        /// </value>
        public decimal SubSubTotalAmount { get; set; }


        /// <summary>
        /// Gets or sets the Sub total amount.
        /// </summary>
        /// <value>
        /// The Subtotal amount.
        /// </value>
        public decimal SubTotalAmount { get; set; }
       
        
        /// <summary>
        /// Gets or sets the total amount.
        /// </summary>
        /// <value>
        /// The total amount.
        /// </value>
        public decimal TotalAmount { get; set; }

        /// <summary>
        /// Gets or sets the promotional amount.
        /// </summary>
        /// <value>
        /// The promotional amount.
        /// </value>
        public decimal PromotionalAmount { get; set; }

        /// <summary>
        /// Gets or sets the promotion code.
        /// </summary>
        /// <value>
        /// The promotion code.
        /// </value>
        public string PromotionCode { get; set; }
    }
}
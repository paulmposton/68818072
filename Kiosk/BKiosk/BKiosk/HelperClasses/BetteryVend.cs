namespace BKiosk.HelperClasses
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
        public int AaaVend { get; set; }

        /// <summary>
        /// Gets the total cartridges.
        /// </summary>
        /// <value>
        /// The total cartridges.
        /// </value>
        public int TotalCartridges
        {
            get { return AaVend + AaaVend; }
        }

        /// <summary>
        /// Gets the new batteries.
        /// </summary>
        /// <value>
        /// The new batteries.
        /// </value>
        public int NewBatteries
        {
            get { return TotalCartridges * 4; }
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
            get { return TotalCartridges - ReturnedCartridges; }
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
        /// Gets or sets the aa returned amount.
        /// </summary>
        /// <value>
        /// The aa returned amount.
        /// </value>
        public decimal AaReturnedAmount { get; set; }

        /// <summary>
        /// Gets or sets the aaa returned amount.
        /// </summary>
        /// <value>
        /// The aaa returned amount.
        /// </value>
        public decimal AaaReturnedAmount { get; set; }

        /// <summary>
        /// Gets the returned amount.
        /// </summary>
        /// <value>
        /// The returned amount.
        /// </value>
        public decimal ReturnedAmount
        {
            get { return AaReturnedAmount + AaaReturnedAmount; }
        }

        /// <summary>
        /// Gets or sets the aa new amount.
        /// </summary>
        /// <value>
        /// The aa new amount.
        /// </value>
        public decimal AaNewAmount { get; set; }

        /// <summary>
        /// Gets or sets the aaa new amount.
        /// </summary>
        /// <value>
        /// The aaa new amount.
        /// </value>
        public decimal AaaNewAmount { get; set; }

        /// <summary>
        /// Gets or sets the new amount.
        /// </summary>
        /// <value>
        /// The new amount.
        /// </value>
        public decimal NewAmount
        {
            get { return AaNewAmount + AaaNewAmount; }
        }

        /// <summary>
        /// Gets or sets the total amount.
        /// </summary>
        /// <value>
        /// The total amount.
        /// </value>
        public decimal TotalAmount { get; set; }
    }
}
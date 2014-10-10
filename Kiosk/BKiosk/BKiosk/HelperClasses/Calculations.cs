using System;

namespace BKiosk.HelperClasses
{
    /// <summary>
    /// class of Calculations
    /// </summary>
    public class Calculations
    {
        /// <summary>
        /// Calcs the charges.
        /// </summary>
        /// <param name="aaReturn">The aa return.</param>
        /// <param name="aaaReturn">The aaa return.</param>
        /// <param name="aaVend">The aa vend.</param>
        /// <param name="aaaVend">The aaa vend.</param>
        /// <returns></returns>
        public static BetteryVend CalcCharges(int aaReturn, int aaaReturn, int aaVend, int aaaVend)
        {
            BetteryVend betteryMath = new BetteryVend();
            betteryMath.AaReturn = aaReturn;
            betteryMath.AaVend = aaVend;
            betteryMath.AaaReturn = aaaReturn;
            betteryMath.AaaVend = aaaVend;

            try
            {
                // Get Battery info (prices)
                Battery battery = new Battery();

                betteryMath.Swapped = Math.Min(betteryMath.TotalCartridges, betteryMath.ReturnedCartridges);
                betteryMath.SwappedAmount = betteryMath.Swapped * battery.SwapPrice;
                // Calc additional new Cartridges charges and Refund for returned cartridges if any
                if (betteryMath.TotalCartridges > betteryMath.ReturnedCartridges)
                {
                    betteryMath.CalculatedNew = betteryMath.NewCartridges;
                    betteryMath.CalculatedNewAmount = betteryMath.CalculatedNew * battery.NewPrice;
                    betteryMath.TotalAmount = betteryMath.CalculatedNewAmount + betteryMath.SwappedAmount;
                }
                else if (betteryMath.ReturnedCartridges > betteryMath.TotalCartridges)
                {
                    betteryMath.CalculatedReturned = Math.Abs(betteryMath.NewCartridges);
                    betteryMath.CalculatedReturnedAmount = - betteryMath.CalculatedReturned * battery.ReturnPrice;
                    betteryMath.TotalAmount = betteryMath.CalculatedReturnedAmount + betteryMath.SwappedAmount;
                }
                else
                {
                    betteryMath.TotalAmount = betteryMath.Swapped * battery.SwapPrice;
                }

                betteryMath.AaNewAmount = betteryMath.AaVend * battery.NewPrice;
                betteryMath.AaaNewAmount = betteryMath.AaaVend * battery.NewPrice;
                betteryMath.AaReturnedAmount = betteryMath.AaReturn * battery.ReturnPrice;
                betteryMath.AaaReturnedAmount = betteryMath.AaaReturn * battery.ReturnPrice;
            }
            catch (Exception ex)
            {
                // TODO: Log Error message
                throw ex;
            }

            return betteryMath;
        }
    }
}
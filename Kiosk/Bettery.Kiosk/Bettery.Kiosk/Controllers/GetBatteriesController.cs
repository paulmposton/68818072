using System;
using System.Diagnostics;
using Bettery.Kiosk.Common;
using Bettery.Kiosk.DataAccess;
using Bettery.Kiosk.Entities;

namespace Bettery.Kiosk.Controllers
{
    /// <summary>
    /// Class Get Batteries Controller
    /// </summary>
    public sealed class GetBatteriesController
    {
        /// <summary>
        /// Gets the max aa product.
        /// </summary>
        /// <returns>Max Aa Product</returns>
        public static int? GetMaxAaProduct()
        {
            try
            {
                int maxAaProduct = BaseDAL.GetTotalQuantitybyProduct(ProductTypes.AA);

                if (maxAaProduct > Constants.BetteryProduct.AaMax)
                {
                    return Constants.BetteryProduct.AaMax;
                }

                return maxAaProduct;
            }
            catch (Exception ex)
            {
                Logger.Log(EventLogEntryType.Error, ex, BaseController.StationId);
                AlertController.TransactionFailureAlert(ex.Message);
                BaseController.RaiseOnThrowExceptionEvent();
            }

            return null;
        }

        /// <summary>
        /// Gets the max aaa product.
        /// </summary>
        /// <returns>Max Aaa Product</returns>
        public static int? GetMaxAaaProduct()
        {
            try
            {
                int maxAaaProduct = BaseDAL.GetTotalQuantitybyProduct(ProductTypes.AAA);

                if (maxAaaProduct > Constants.BetteryProduct.AaaMax)
                {
                    return Constants.BetteryProduct.AaaMax;
                }

                return maxAaaProduct;
            }
            catch (Exception ex)
            {
                Logger.Log(EventLogEntryType.Error, ex, BaseController.StationId);
                AlertController.TransactionFailureAlert(ex.Message);
                BaseController.RaiseOnThrowExceptionEvent();
            }

            return null;
        }

        /// <summary>
        /// Determines whether [has total transaction not zero].  In case user doesn't have any return batteries and no new.
        /// </summary>
        /// <returns>
        ///   <c>true</c> if [has total transaction not zero]; otherwise, <c>false</c>.
        /// </returns>
        public static bool HasTotalTransactionNotZero()
        {
            bool result = false;

            BetteryVend bettery = BaseController.SelectedBettery;
            if (bettery != null && (bettery.TotalAmount != 0 || bettery.TotalVendCartridges > 0 || bettery.ReturnedCartridges > 0 || bettery.TotalForgotDrainedVendCartridges > 0))
            {
                result = true;
            }

            return result;
        }
    }
}

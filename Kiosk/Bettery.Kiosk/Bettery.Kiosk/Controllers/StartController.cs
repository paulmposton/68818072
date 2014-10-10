using System;
using System.Diagnostics;
using Bettery.Kiosk.Common;
using Bettery.Kiosk.DataAccess;

namespace Bettery.Kiosk.Controllers
{
    /// <summary>
    /// Class Start Controller
    /// </summary>
    public sealed class StartController
    {
        /// <summary>
        /// Checks the bettery store.
        /// </summary>
        public static bool BetteryStoreExists()
        {
            try
            {
                int aa = BaseDAL.GetTotalQuantitybyProduct(ProductTypes.AA);
                int aaa = BaseDAL.GetTotalQuantitybyProduct(ProductTypes.AAA);

                if (aa > 0 || aaa > 0)
                {
                    return true;
                }

                return false;
            }
            catch (Exception ex)
            {
                Logger.Log(EventLogEntryType.Error, ex, BaseController.StationId);
                AlertController.TransactionFailureAlert(ex.Message);
                BaseController.RaiseOnThrowExceptionEvent();

                return false;
            }
        }

        /// <summary>
        /// Gets the out of battery message.
        /// </summary>
        /// <returns></returns>
        public static bool GetOutOfBatteryMessage(out string message)
        {
            message = string.Empty;

            try
            {
                int aaCount = BaseDAL.GetTotalQuantitybyProduct(ProductTypes.AA);
                int aaaCount = BaseDAL.GetTotalQuantitybyProduct(ProductTypes.AAA);

                if (aaCount <= 0 && aaaCount <= 0)
                {
                    message = Constants.Messages.OutOfBatteries;
                    Logger.Log(EventLogEntryType.Error, "Station out of Batteries", BaseController.StationId);
                }
                else if (aaCount <= 0)
                {
                    message= Constants.Messages.OutOfAABatteries;
                    Logger.Log(EventLogEntryType.Error, "Station out of AA Batteries", BaseController.StationId);
                }
                else if (aaaCount <= 0)
                {
                    message= Constants.Messages.OutOfAAABatteries;
                    Logger.Log(EventLogEntryType.Error, "Station out of AAA Batteries", BaseController.StationId);
                }

                return true;
            }
            catch (Exception ex)
            {
                Logger.Log(EventLogEntryType.Error, ex, BaseController.StationId);
                AlertController.TransactionFailureAlert(ex.Message);
                BaseController.RaiseOnThrowExceptionEvent();

                return false;
            }
        }
    }
}
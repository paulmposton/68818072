using System;
using System.Diagnostics;
using Bettery.Kiosk.BService;
using Bettery.Kiosk.Common;
using Bettery.Kiosk.DataAccess;

namespace Bettery.Kiosk.Controllers
{
    public class AlertController
    {
        /// <summary>
        /// Pings the service.
        /// </summary>
        /// <returns></returns>
        public static bool PingService()
        {
            bool result = false;
            try
            {
                using (KioskServiceClient bKioskService = new KioskServiceClient())
                {
                    result = bKioskService.KioskPing();
                    bKioskService.Close();
                }
            }
            catch (Exception ex)
            {
                Logger.Log(EventLogEntryType.Error, ex, BaseController.StationId);
                result = false;
            }

            return result;
        }

        /// <summary>
        /// Alerts this instance.
        /// </summary>
        /// <returns></returns>
        public static bool Alert()
        {
            bool result = false;

            try
            {

                //int totalAaRemaining,
                //    totalAaaRemaining,
                //    totalEmptyPackagesRemaining,
                //    totalQuantityReturned;

                //try
                //{
                //    totalAaRemaining = BaseDAL.GetTotalQuantitybyProduct(ProductTypes.AA);
                //    totalAaaRemaining = BaseDAL.GetTotalQuantitybyProduct(ProductTypes.AAA);
                //    totalEmptyPackagesRemaining = BaseDAL.GetTotalQuantitybyProduct(ProductTypes.Cartridge);

                //    totalQuantityReturned = BaseDAL.GetTotalQuantityReturned();
                //}
                //catch (Exception ex)
                //{
                //    //Alert if the have issue with local SQL
                //    TransactionFailureAlert(ex.Message);

                //    throw ex;
                //}

                //using (KioskServiceClient bKioskService = new KioskServiceClient())
                //{

                //    //bool isReturnedBinFull = totalQuantityReturned >= BaseController.ReturnBinCapacity;

                    ////Return bin full
                    //if (isReturnedBinFull)
                    //{
                    //    bKioskService.ReturnBinFullAlert(BaseController.StationId);
                    //}

                    ////inventory status
                    //if (totalAaRemaining <= BaseController.MinAaRemainingAlert)
                    //{
                    //    bKioskService.ProductInventoryAlert(BaseController.StationId, (int)ProductTypes.AA);
                    //}

                    //if (totalAaaRemaining <= BaseController.MinAaaRemainingAlert)
                    //{
                    //    bKioskService.ProductInventoryAlert(BaseController.StationId, (int)ProductTypes.AAA);
                    //}

                    //if (totalEmptyPackagesRemaining <= BaseController.MinCartridgeRemainingAlert)
                    //{
                    //    bKioskService.ProductInventoryAlert(BaseController.StationId, (int)ProductTypes.Cartridge);
                    //}

                //    bKioskService.Close();
                //}
            }
            catch (Exception ex)
            {
                Logger.Log(EventLogEntryType.Error, ex, BaseController.StationId);
            }

            return result;
        }

        /// <summary>
        /// Transactions the failure alert.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <returns></returns>
        public static bool TransactionFailureAlert(string message)
        {
            bool result = false;
            try
            {
                using (KioskServiceClient bKioskService = new KioskServiceClient())
                {
                    bKioskService.TransactionFailureAlert(BaseController.StationId, message);
                    bKioskService.Close();
                }
            }
            catch (Exception ex)
            {
                Logger.Log(EventLogEntryType.Error, ex, BaseController.StationId);
            }

            return result;
        }

        /// <summary>
        /// Authorizes the dot net alert.
        /// </summary>
        /// <param name="authorizeException">The authorize exception.</param>
        public static void AuthorizeDotNetAlert(Exception authorizeException)
        {
            try
            {
                using (KioskServiceClient bKioskService = new KioskServiceClient())
                {
                    bKioskService.AuthorizeDotNetAlert(BaseController.StationId, authorizeException.Message);
                    bKioskService.Close();
                }
            }
            catch (Exception ex)
            {
                Logger.Log(EventLogEntryType.Error, ex, BaseController.StationId);
            }
        }

        /// <summary>
        /// Sends the logs.
        /// </summary>
        /// <returns></returns>
        public static bool SendLogs()
        {
            return true;
        }
    }
}

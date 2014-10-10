using System;
using System.Diagnostics;
using Bettery.Kiosk.Common;
using Bettery.Kiosk.DataAccess;

namespace Bettery.Kiosk.Controllers
{
    public class SwapController
    {
        /// <summary>
        /// Determines whether this instance has credit.
        /// </summary>
        /// <returns>
        ///   <c>true</c> if this instance has credit; otherwise, <c>false</c>.
        /// </returns>
        public static bool HasCredit(out int batteryPackages)
        {
            batteryPackages = 0;

            // if (BaseController.SelectedBettery != null && BaseController.SelectedBettery.TotalAmount < 0)
            if (BaseController.SelectedBettery != null && BaseController.SelectedBettery.TotalVendCartridges < BaseController.SelectedBettery.ReturnedCartridges)
            {
                batteryPackages = Math.Abs(BaseController.SelectedBettery.NewCartridges);

                return true;
            }

            return false;
        }

        /// <summary>
        /// Determines whether this instance has credit.
        /// </summary>
        /// <returns>
        ///   <c>true</c> if this instance has credit; otherwise, <c>false</c>.
        /// </returns>
        public static bool HasCredit()
        {
            // if (BaseController.SelectedBettery != null && BaseController.SelectedBettery.TotalAmount < 0)
            if (BaseController.SelectedBettery != null && BaseController.SelectedBettery.TotalVendCartridges < BaseController.SelectedBettery.ReturnedCartridges)
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Returns the cartridge.
        /// </summary>
        public static void ReturnCartridge()
        {
            try
            {
                BaseDAL.ReturnCartridge(1);
            }
            catch (Exception ex)
            {
                Logger.Log(EventLogEntryType.Error, ex, BaseController.StationId);
                BaseController.RaiseOnThrowExceptionEvent();
            }
        }
    }
}
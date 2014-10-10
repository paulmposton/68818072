using Bettery.Kiosk.Entities;

namespace Bettery.Kiosk.Controllers
{
    /// <summary>
    /// Class CountDown Controller
    /// </summary>
    public sealed class CountDownController
    {
        /// <summary>
        /// Returneds the batteries exists.
        /// </summary>
        /// <returns></returns>
        public static bool ReturnedBatteriesExists()
        {
            BetteryVend bettery = BaseController.SelectedBettery;
            if (bettery != null && bettery.ReturnedCartridges > 0)
            {
                return true;
            }

            return false;
        }
    }
}
using Bettery.Kiosk.Common;
using Bettery.Kiosk.DataAccess;

namespace Bettery.Kiosk.Controllers
{
    /// <summary>
    /// Class GetCase Contrtoller
    /// </summary>
    public sealed class GetCaseController
    {
        /// <summary>
        /// Gets the max empty cases.
        /// </summary>
        /// <returns></returns>
        public static int GetMaxEmptyCases()
        {
            int maxEmptyCases = BaseDAL.GetTotalQuantitybyProduct(ProductTypes.Cartridge);
            int emptyCasesRemaining = 0;

            if (BaseController.LoggedOnUser != null)
            {
                emptyCasesRemaining = BaseController.LoggedOnUser.FreeCasesRemaining;
            }

            return Utils.Min(emptyCasesRemaining, maxEmptyCases, Constants.BetteryProduct.EmptyCasesReturnMax);
        }
    }
}
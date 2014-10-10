using System;
using System.Diagnostics;
using Bettery.Kiosk.BService;
using Bettery.Kiosk.Common;
using Bettery.Kiosk.Entities;

namespace Bettery.Kiosk.Controllers
{
    /// <summary>
    /// Class PromoCodes Controller
    /// </summary>
    public class PromoCodesController
    {
        /// <summary>
        /// Gets the promotional amount.
        /// </summary>
        /// <param name="promotionalCode">The promotional code.</param>
        /// <returns></returns>
        public static decimal? GetPromotionalAmount(string promotionalCode, out string invalidReason)
        {
            decimal? promoCredit = null;
            Promo promo = new Promo();
            invalidReason = String.Empty;

            using (KioskServiceClient proxy = new KioskServiceClient())
            {
                try
                {
                    promo = proxy.GetPromoCredit(promotionalCode);
                    switch (promo.PromoType)
                    {
                        case Bettery.Kiosk.Common.Constants.PromotionType.Purchase:
                            if ((BaseController.SelectedBettery.AaVend + BaseController.SelectedBettery.AaaVend) > BaseController.SelectedBettery.AaReturn)
                                return promo.Amount;
                            else
                            {
                                invalidReason = Constants.Messages.InvalidNewPurchasePromotionCode;
                                return 0M;
                            }
                                
                        case Bettery.Kiosk.Common.Constants.PromotionType.Swap:
                            if ((BaseController.SelectedBettery.AaVend > 0 && BaseController.SelectedBettery.AaReturn > 0) || (BaseController.SelectedBettery.AaaVend > 0 && BaseController.SelectedBettery.AaReturn > 0))
                                return promo.Amount;
                            else
                            {
                                invalidReason = Constants.Messages.InvalidSwapPromotionCode;
                                return 0M;
                            }
                                
                        case Bettery.Kiosk.Common.Constants.PromotionType.SwapAndPurchase:
                            if ((BaseController.SelectedBettery.AaVend > 0) || (BaseController.SelectedBettery.AaaVend > 0 ))
                                return promo.Amount;
                            else
                                return 0M;
                        default:
                            return 0M;

                    }
                }
                catch (Exception ex)
                {
                    Logger.Log(EventLogEntryType.Error, ex, BaseController.StationId);
                    BaseController.RaiseOnThrowExceptionEvent();
                }
                finally
                {
                    proxy.Close();
                }
            }

            return promoCredit;
        }
    }
}
using System;
using System.Collections.Generic;
using System.Diagnostics;
using Bettery.Kiosk.BService;
using Bettery.Kiosk.Common;
using Bettery.Kiosk.DataAccess;
using Bettery.Kiosk.Entities;

namespace Bettery.Kiosk.Controllers
{
    /// <summary>
    /// Class Vending Controller
    /// </summary>
    public class VendingController
    {
        public delegate void VendEventHandler(VendEventArgs e);

        /// <summary>
        /// Occurs when [on vending].
        /// </summary>
        public static event VendEventHandler OnVending;

        /// <summary>
        /// Occurs when [on vending case].
        /// </summary>
        public static event VendEventHandler OnVendingCase;

        /// <summary>
        /// Vends the batteries to user.
        /// </summary>
        public static bool Vending()
        {
            try
            {
                // Product: AA
                if (BaseController.SelectedBettery != null)
                {
                    int totalBins;

                    int vendPauseCount = BaseController.VendPauseCount;
                    bool isVendBinSuccess = false;

                    VendEventArgs vendEventArgs;

                    vendEventArgs = new VendEventArgs(BaseController.SelectedBettery.AaVend, BaseController.SelectedBettery.AaaVend);

                    if (BaseController.SelectedBettery.AaVendRemaining > 0 && vendPauseCount > 0)
                    {
                        totalBins = BaseDAL.GetTotalQuantitybyProduct(ProductTypes.AA);
                        if (totalBins > 0)
                        {

                            List<BinProduct> aaBins = BaseDAL.GetBinsbyProduct(ProductTypes.AA);
                            foreach (BinProduct bin in aaBins)
                            {
                                isVendBinSuccess = false;

                                if (bin.Quantity > 0)
                                {

                                    for (int i = 0; i < bin.Quantity; i++)
                                    {
                                        if (BaseController.SelectedBettery.AaVendRemaining > 0)
                                        {

                                            // Update the Vending message
                                            if (OnVending != null)
                                            {
                                                vendEventArgs.CurrentProductAA = BaseController.SelectedBettery.AaVend - BaseController.SelectedBettery.AaVendRemaining;
                                                vendEventArgs.CurrentProductAAA = BaseController.SelectedBettery.AaaVend - BaseController.SelectedBettery.AaaVendRemaining;
                                                OnVending.Invoke(vendEventArgs);
                                            }

                                            // Send the Vend command to the serial port
                                            isVendBinSuccess = BaseController.SendCommandToSerialPort(bin.BinId);

                                            if (isVendBinSuccess)
                                            {
                                                vendPauseCount--;

                                                // Decrement Remaining to vend
                                                BaseController.SelectedBettery.AaVendRemaining--;

                                                // Decrement Bin Inventory
                                                BaseDAL.DecrementBinInventory(bin.BinId);

                                                //// Update the Vending message
                                                //if (OnVending != null)
                                                //{
                                                //    vendEventArgs.CurrentProductAA = BaseController.SelectedBettery.AaVend - BaseController.SelectedBettery.AaVendRemaining;
                                                //    vendEventArgs.CurrentProductAAA = BaseController.SelectedBettery.AaaVend - BaseController.SelectedBettery.AaaVendRemaining;
                                                //    OnVending.Invoke(vendEventArgs);
                                                //}

                                                if (vendPauseCount == 0)
                                                    return true;

                                            }
                                            else
                                            {
                                                // Disable the bin
                                                BaseDAL.DisableBin(bin.BinId);

                                                AlertController.TransactionFailureAlert("Bin " + bin.BinId.ToString() + " Disabled");

                                                // Don't continue iterating through bin
                                                break;

                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }


                    // Product: AAA
                    if (BaseController.SelectedBettery.AaaVendRemaining > 0 && vendPauseCount > 0)
                    {
                        totalBins = BaseDAL.GetTotalQuantitybyProduct(ProductTypes.AAA);

                        if (totalBins > 0)
                        {

                            List<BinProduct> aaaBins = BaseDAL.GetBinsbyProduct(ProductTypes.AAA);
                            foreach (BinProduct bin in aaaBins)
                            {
                                isVendBinSuccess = false;

                                if (bin.Quantity > 0)
                                {

                                    for (int i = 0; i < bin.Quantity; i++)
                                    {
                                        if (BaseController.SelectedBettery.AaaVendRemaining > 0)
                                        {

                                            // Update the vending message
                                            if (OnVending != null)
                                            {
                                                vendEventArgs.CurrentProductAA = BaseController.SelectedBettery.AaVend - BaseController.SelectedBettery.AaVendRemaining;
                                                vendEventArgs.CurrentProductAAA = BaseController.SelectedBettery.AaaVend - BaseController.SelectedBettery.AaaVendRemaining;
                                                OnVending.Invoke(vendEventArgs);
                                            }

                                            // Send the Vend command to the serial port
                                            isVendBinSuccess = BaseController.SendCommandToSerialPort(bin.BinId);

                                            if (isVendBinSuccess)
                                            {
                                                vendPauseCount--;

                                                // Decrement Remaining to vend
                                                BaseController.SelectedBettery.AaaVendRemaining--;

                                                // Decrement Bin Inventory
                                                BaseDAL.DecrementBinInventory(bin.BinId);

                                                //// Update the vending message
                                                //if (OnVending != null)
                                                //{
                                                //    vendEventArgs.CurrentProductAA = BaseController.SelectedBettery.AaVend - BaseController.SelectedBettery.AaVendRemaining;
                                                //    vendEventArgs.CurrentProductAAA = BaseController.SelectedBettery.AaaVend - BaseController.SelectedBettery.AaaVendRemaining;
                                                //    OnVending.Invoke(vendEventArgs);
                                                //}

                                                if (vendPauseCount == 0)
                                                    return true;

                                            }
                                            else
                                            {
                                                // Disable the bin
                                                BaseDAL.DisableBin(bin.BinId);

                                                AlertController.TransactionFailureAlert("Bin " + bin.BinId.ToString() + " Disabled");

                                                // Don't continue iterating through bin
                                                break;
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }

            catch (Exception ex)
            {
                Logger.Log(EventLogEntryType.Error, ex, BaseController.StationId);
                AlertController.TransactionFailureAlert(ex.Message);
                BaseController.RaiseOnThrowExceptionEvent();
                return false;
            }
            return true;

        }

        /// <summary>
        /// Vends the empty case.
        /// </summary>
        /// <param name="totalEmptyCases">The total empty cases.</param>
        public static void VendEmptyCase(int totalEmptyCases)
        {
            var totalBins = BaseDAL.GetTotalQuantitybyProduct(ProductTypes.Cartridge);
            bool isVendBinSuccess = false;

            int vendedFreeCases = 0;

            if (totalBins > 0)
            {
                VendEventArgs vendEventArgs = new VendEventArgs(totalEmptyCases);

                try
                {
                    
                    List<BinProduct> cartridgeBins = BaseDAL.GetBinsbyProduct(ProductTypes.Cartridge);
                    foreach (BinProduct bin in cartridgeBins)
                    {
                        if (totalEmptyCases == 0)
                        {
                            break;
                        }

                        if (bin.Quantity > 0)
                        {
                            BinProduct decrementBin = new BinProduct { BinId = bin.BinId, Quantity = 0 };

                            for (int i = 0; i < bin.Quantity; i++)
                            {
                                if (totalEmptyCases == 0)
                                {
                                    break;
                                }

                                 isVendBinSuccess = BaseController.SendCommandToSerialPort(bin.BinId);

                                 if (isVendBinSuccess)
                                 {
                                     totalEmptyCases--;
                                     vendedFreeCases++;
                                     decrementBin.Quantity++;
                                     if (OnVending != null)
                                     {
                                         vendEventArgs.VendedEmptyCases = vendedFreeCases;
                                         OnVendingCase.Invoke(vendEventArgs);
                                     }
                                 }
                                 else
                                 {
                                     BaseDAL.DisableBin(bin.BinId);
                                     AlertController.TransactionFailureAlert("Bin " + bin.BinId.ToString() + " Disabled");
                                     // Don't continue iterating through bin
                                     break;

                                 }
                            }

                            if (isVendBinSuccess && decrementBin.Quantity > 0)
                            {
                                BaseDAL.DecrementBinInventory(decrementBin);
                            }
                            isVendBinSuccess = false;
                        }
                    }

                    using (KioskServiceClient proxy = new KioskServiceClient())
                    {
                        //  Update the members remaining free cases.
                        proxy.EmptyCaseVend(BaseController.LoggedOnUser.MemberId, vendedFreeCases);
                        proxy.Close();
                    }
                }
                catch (Exception ex)
                {
                    Logger.Log(EventLogEntryType.Error, ex, BaseController.StationId);
                    AlertController.TransactionFailureAlert(ex.Message);
                    BaseController.RaiseOnThrowExceptionEvent();
                }
            }
        }
    }
}
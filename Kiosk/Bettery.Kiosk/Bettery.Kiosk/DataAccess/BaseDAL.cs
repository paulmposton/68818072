using System.Collections.Generic;
using System.Data.SqlClient;
using Bettery.Kiosk.Common;
using Bettery.Kiosk.Controllers;
using Bettery.Kiosk.Entities;

namespace Bettery.Kiosk.DataAccess
{
    public class BaseDAL
    {
        /// <summary>
        /// Gets the binsby product.
        /// </summary>
        /// <param name="productType">Type of the product.</param>
        /// <returns></returns>
        public static List<BinProduct> GetBinsbyProduct(ProductTypes productType)
        {
            List<BinProduct> binVends = new List<BinProduct>();
            SqlParameter[] paramenters = new SqlParameter[1];
            paramenters[0] = new SqlParameter("@Product", productType.ToString());

            SqlDataReader reader = SqlHelper.ExecuteReader(BaseController.ConnectionString, "GetBinsbyProduct", paramenters);

            while (reader.Read())
            {
                BinProduct bin = new BinProduct();
                bin.BinId = SqlHelper.ToInt32(reader, "BinID");
                bin.Quantity = SqlHelper.ToInt32(reader, "PackageQuantity");

                binVends.Add(bin);
            }

            return binVends;
        }

        /// <summary>
        /// Decrements the bin inventory.
        /// </summary>
        /// <param name="bin">The bin.</param>
        /// <returns></returns>
        public static int UpdateInventory(int BinID, int Quantity, int ProductID)
        {
            SqlParameter[] paramenters = new SqlParameter[3];
            paramenters[0] = new SqlParameter("@BinID", BinID);
            paramenters[1] = new SqlParameter("@Quantity", Quantity);
            paramenters[2] = new SqlParameter("@ProductID", ProductID);

            int result = SqlHelper.ExecuteNonQuery(BaseController.ConnectionString, "UpdateInventory", paramenters);

            return result;
        }

        /// <summary>
        /// Disables a bin.
        /// </summary>
        /// <param name="bin">The bin.</param>
        /// <returns></returns>
        public static int DisableBin(int BinID)
        {
            SqlParameter[] paramenters = new SqlParameter[1];
            paramenters[0] = new SqlParameter("@BinID", BinID);

            int result = SqlHelper.ExecuteNonQuery(BaseController.ConnectionString, "DisableBin", paramenters);

            return result;
        }

        /// <summary>
        /// Decrements the bin inventory.
        /// </summary>
        /// <param name="bin">The bin.</param>
        /// <returns></returns>
        public static int DecrementBinInventory(BinProduct bin)
        {
            List<BinProduct> binVends = new List<BinProduct>();
            SqlParameter[] paramenters = new SqlParameter[2];
            paramenters[0] = new SqlParameter("@BinID", bin.BinId);
            paramenters[1] = new SqlParameter("@Quantity", bin.Quantity);

            int result = SqlHelper.ExecuteNonQuery(BaseController.ConnectionString, "DecrementBinInventory", paramenters);

            return result;
        }

        public static int DecrementBinInventory(int binID)
        {
            List<BinProduct> binVends = new List<BinProduct>();
            SqlParameter[] paramenters = new SqlParameter[2];
            paramenters[0] = new SqlParameter("@BinID", binID);
            paramenters[1] = new SqlParameter("@Quantity", 1);

            int result = SqlHelper.ExecuteNonQuery(BaseController.ConnectionString, "DecrementBinInventory", paramenters);

            return result;
        }



        /// <summary>
        /// Gets the price by product.
        /// </summary>
        /// <param name="productType">Product ID.</param>
        /// <returns></returns>
        public static decimal GetNewPricebyProduct(int productID)
        {
            decimal newPrice;
            newPrice = Constants.BetteryPrice.NewPrice;
            
            SqlParameter[] paramenters = new SqlParameter[1];
            paramenters[0] = new SqlParameter("@ProductID", productID);

            SqlDataReader reader = SqlHelper.ExecuteReader(BaseController.ConnectionString, "GetPricebyProduct", paramenters);

            while (reader.Read())
            {
                newPrice = (decimal)reader["NewPrice"];
            }

            return newPrice;
        }

        /// <summary>
        /// Gets the Return Price by product.
        /// </summary>
        /// <param name="productType">Product ID</param>
        /// <returns></returns>
        public static decimal GetReturnPricebyProduct(int productID)
        {
            decimal returnPrice;
            returnPrice = Constants.BetteryPrice.ReturnPrice;
            

            SqlParameter[] paramenters = new SqlParameter[1];
            paramenters[0] = new SqlParameter("@ProductID", productID);

            SqlDataReader reader = SqlHelper.ExecuteReader(BaseController.ConnectionString, "GetPricebyProduct", paramenters);

            while (reader.Read())
            {
                returnPrice = (decimal)reader["ReturnPrice"];
            }

            return returnPrice;
        }

        /// <summary>
        /// Gets the Swap Price by product.
        /// </summary>
        /// <param name="productType">Product ID</param>
        /// <returns></returns>
        public static decimal GetSwapPricebyProduct(int productID)
        {
            decimal swapPrice;
            swapPrice = Constants.BetteryPrice.SwapPrice;

            SqlParameter[] paramenters = new SqlParameter[1];
            paramenters[0] = new SqlParameter("@ProductID", productID);

            SqlDataReader reader = SqlHelper.ExecuteReader(BaseController.ConnectionString, "GetPricebyProduct", paramenters);

            while (reader.Read())
            {
                swapPrice = (decimal)reader["SwapPrice"];
            }

            return swapPrice;
        }

        /// <summary>
        /// Gets the Swap Price by product.
        /// </summary>
        /// <param name="productType">Product ID</param>
        /// <returns></returns>
        public static decimal GetTaxbyProduct(int productID)
        {
            decimal tax;
            tax = 0;

            SqlParameter[] paramenters = new SqlParameter[1];
            paramenters[0] = new SqlParameter("@ProductID", productID);

            SqlDataReader reader = SqlHelper.ExecuteReader(BaseController.ConnectionString, "GetPricebyProduct", paramenters);

            while (reader.Read())
            {
                tax = (decimal)reader["Tax"];
            }

            return tax;
        }



        /// <summary>
        /// Gets the total quantityby product.
        /// </summary>
        /// <param name="productType">Type of the product.</param>
        /// <returns></returns>
        public static int GetTotalQuantitybyProduct(ProductTypes productType)
        {
            int totalQuantity = 0;
            SqlParameter[] paramenters = new SqlParameter[1];
            paramenters[0] = new SqlParameter("@ProductDescription", productType.ToString());

            SqlDataReader reader = SqlHelper.ExecuteReader(BaseController.ConnectionString, "GetTotalQuantitybyProduct", paramenters);

            while (reader.Read())
            {
                totalQuantity = SqlHelper.ToInt32(reader);
            }

            return totalQuantity;
        }

        /// <summary>
        /// Gets the total quantity returned.
        /// </summary>
        /// <returns></returns>
        public static int GetTotalQuantityReturned()
        {
            int totalQuantity = 0;
            SqlDataReader reader = SqlHelper.ExecuteReader(BaseController.ConnectionString, "GetTotalQuantityReturned");

            while (reader.Read())
            {
                totalQuantity = SqlHelper.ToInt32(reader);
            }

            return totalQuantity;
        }
        /// <summary>
        /// Gets the total quantityby product.
        /// </summary>
        /// <param name="productType">Type of the product.</param>
        /// <returns></returns>
        public static BinProduct GetInventorybyBin(int BinID)
        {
            BinProduct binProduct = new BinProduct();
            SqlParameter[] paramenters = new SqlParameter[1];
            paramenters[0] = new SqlParameter("@BinID", BinID);

            SqlDataReader reader = SqlHelper.ExecuteReader(BaseController.ConnectionString, "GetInventorybyBin", paramenters);

            while (reader.Read())
            {
                binProduct.Quantity = int.Parse(reader["PackageQuantity"].ToString());
                binProduct.ProductID = int.Parse(reader["ProductID"].ToString());
                binProduct.Enabled = bool.Parse(reader["Enabled"].ToString());
            }

            return binProduct;
        }
        /// <summary>
        /// Returns the cartridge.
        /// </summary>
        /// <param name="count">The count.</param>
        public static void ReturnCartridge(int count)
        {
            SqlParameter[] paramenters = new SqlParameter[1];
            paramenters[0] = new SqlParameter("@Count", count.ToString());

            SqlHelper.ExecuteNonQuery(BaseController.ConnectionString, "ReturnCartridge", paramenters);
        }
    }
}

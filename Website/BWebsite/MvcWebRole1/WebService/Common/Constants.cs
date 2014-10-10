using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebRole1.Common
{
    public class Constants
    {
        public class AlertType
        {
            public const int ReturnBinFull = 1;
            public const int ProductInventoryAlert = 2;
            public const int TransactionFailureAlert = 3;
        }
        public class Product
        {
            public const int AA = 1;
            public const int AAA = 2;
            public const int Cartridge = 3;

        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MvcWebRole1.Common
{
    public class Constants
    {
        public class Messages
        {

            public const string MatchEmailPattern =
                @"^(([\w-]+\.)+[\w-]+|([a-zA-Z]{1}|[\w-]{2,}))@"
                + @"((([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])\.([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])\."
                + @"([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])\.([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])){1}|"
                + @"([a-zA-Z]+[\w-]+\.)+[a-zA-Z]{2,4})$";
        }
        /// <summary>
        /// Class Bettery Price
        /// </summary>
        public class Group
        {
            public const int SuperUser = 1;
            public const int SwapStationAdmin = 2;
            public const int CorpAccount = 3;
        }

    }
}

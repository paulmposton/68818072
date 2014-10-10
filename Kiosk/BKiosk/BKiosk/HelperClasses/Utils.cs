using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BKiosk.HelperClasses
{
    public class Utils
    {
        public static string ToString(object value)
        {
            if (value != null)
            {
                return value.ToString();
            }
            else
            {
                return string.Empty;
            }
        }

        public static string ToUpperString(object value)
        {
            if (value != null)
            {
                return value.ToString().ToUpper();
            }
            else
            {
                return string.Empty;
            }
        }

        public static string ToLowerString(object value)
        {
            if (value != null)
            {
                return value.ToString().ToLower();
            }
            else
            {
                return string.Empty;
            }
        }
    }
}

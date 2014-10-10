using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BKiosk.HelperClasses
{
    class Battery
    {
        public decimal SwapPrice { get; set; }
        public decimal NewPrice { get; set; }
        public decimal ReturnPrice { get; set; }
        public Battery()
        {
            // Eventually we'll call a service to get prices but setting them here for now
            SwapPrice = 1.99M;
            NewPrice = 9.99M;
            ReturnPrice = 8.00M;
        }
    }
}

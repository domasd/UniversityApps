using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DailyPriceNotifier_Decorator.PriceGetters;

namespace DailyPriceNotifier_Decorator
{
    public struct PriceUrl
    {
        public Uri PriceUri { get; set; }
        public IPriceParser parser { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DailyPriceNotifier_ExtensionObejcts.PriceGetters
{
    public interface IPriceGetter
    {
        decimal PriceToCheck { get; set; }

        void CheckPrice();

        string FormMessage();
    }
}

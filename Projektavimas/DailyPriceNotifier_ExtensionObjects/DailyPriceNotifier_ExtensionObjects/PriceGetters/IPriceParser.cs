using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DailyPriceNotifier_ExtensionObejcts.PriceGetters
{
    public interface IPriceParser
    {
        decimal Parse(string htmlContents);
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DailyPriceNotifier_Decorator.Notifiers;

namespace DailyPriceNotifier_Decorator
{
    public interface INotifier
    {
        string Message { get; set; }
        void Notify();

        void OnSuccessAppendLog(string componentName);

        int LoggedCount();
    }
}

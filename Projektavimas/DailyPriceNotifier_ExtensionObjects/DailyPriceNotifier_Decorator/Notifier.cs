using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DailyPriceNotifier_ExtensionObejcts.Notifiers;

namespace DailyPriceNotifier_ExtensionObejcts
{
    public abstract class Notifier
    {
        private StringBuilder logStringBuilder;
        protected Dictionary<String, IExternalNotifierExtension> _extensions = new Dictionary<string, IExternalNotifierExtension>();

        public string Message { get; set; }
        public Notifier(string message, StringBuilder log)
        {
            this.logStringBuilder = log;
            this.Message = message;
        }
       public abstract void Notify();

        public void OnSuccessAppendLog(string componentName)
        {
            logStringBuilder.AppendLine($"Component {componentName} notified it's target successfuly at {DateTime.Now}");
        }

        public void AddExtension(string name, IExternalNotifierExtension extension)
        {
            _extensions.Add(name, extension);
        }

        public IExternalNotifierExtension GetExtension(string name)
        {
            return _extensions[name];
        }

    }
}

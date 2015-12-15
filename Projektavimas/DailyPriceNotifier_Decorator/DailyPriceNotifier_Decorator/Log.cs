using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DailyPriceNotifier_Decorator
{
    public static class Log
    {
        public static void Append(string input)
        {
            File.AppendAllText(ConfigurationManager.AppSettings["LogPath"], Environment.NewLine + $"[{DateTime.Now}]: {input}");
        }
    }
}

using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DailyPriceNotifier_ExtensionObejcts.Notifiers;
using DailyPriceNotifier_ExtensionObejcts.PriceGetters;

namespace DailyPriceNotifier_ExtensionObejcts
{
    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            decimal price = Decimal.Parse(ConfigurationManager.AppSettings["PriceBelow_Sonysmartwatch3"]);

            IPriceParser technoramaParser = new TechnoramaPriceParser();
            IPriceParser _1aParser = new _1APriceParser();
            IPriceParser elektroParser = new ElektroMarktParser();

            CommonPricegetter priceGetter = new CommonPricegetter(new Dictionary<Uri, IPriceParser>()
            {
                {
                     new Uri("http://www.technorama.lt/Mobilus-telefonai-ir-navigacijos/Ismanieji-laikrodziai/Ismanusis-laikrodis-Sony-smartwatch-3-SWR50-white.html"),
                     technoramaParser

                },
                {
                     new Uri("http://www.1a.lt/telefonai_plansetiniai_kompiuteriai/priedai_mobiliems_telefonams/ismanieji_laikrodziai_apyrankes/sony_smartwatch_3_black"),
                     _1aParser

                },
                {
                     new Uri("http://www.elektromarkt.lt/Foto-Video-GPS-GSM-MP3/Ismanieji-laikrodziai/SWR50-Sony-Smart-Watch-3-ismanusis-laikrodis-black.html"),
                     elektroParser

                }
            }, price);
            priceGetter.CheckPrice();

            string producstAboveThePrice = priceGetter.FormMessage();

            if (!string.IsNullOrEmpty(producstAboveThePrice))
            {
                StringBuilder sb = new StringBuilder();

                Notifier notifier = new SimpleNotifier(producstAboveThePrice, sb);

                notifier.AddExtension("EmailNotifier",new EmailNotifierExtension(notifier));
                notifier.AddExtension("NirvanaNotifier", new NirvanaNotifierExtension(notifier));

                SendNotification(notifier);

                Log.Append(sb.ToString());
  
                Application.Run();
            }
        }

        private static void SendNotification(Notifier notifier)
        {
            notifier.Notify();
        }
    }
}

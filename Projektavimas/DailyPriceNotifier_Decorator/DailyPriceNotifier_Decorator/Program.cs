using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DailyPriceNotifier_Decorator.Notifiers;
using DailyPriceNotifier_Decorator.PriceGetters;

namespace DailyPriceNotifier_Decorator
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
            IPriceParser egnetasParser = new EgnetasPriceParser();
            IPriceParser omnitelParser = new OmnitelPriceParser();

            // These links are different eshops
            // Parsers could not work ant throw an exception (or show bad value) if a html code structure changed 
            // Checked on 2015-11-28
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
                },
                {
                     new Uri("https://www.egnetas.lt/sony-smartwatch-3-swr50-black/kaina"),
                     egnetasParser
                },
                {
                     new Uri("https://order.omnitel.lt/priedai/sony-smart3-swr50/?color=321"),
                     omnitelParser
                }

            }, price);
            try
            {
                priceGetter.CheckPrice();
            }
            catch(Exception e)
            {
                Log.Append($"Getting price failed with exception! Exception details: {e}");

            }

            string producstAboveThePrice = priceGetter.FormMessage();

            if (!string.IsNullOrEmpty(producstAboveThePrice))
            {
                StringBuilder sb = new StringBuilder();

                var decoratedNotifier =
                    (INotifier)
                        new EmailNotifierDecorator(
                            new NirvanaNotifierDecorator(new SimpleNotifier(producstAboveThePrice, sb)));

                // Code for meeting the task requirments
                // although it does not do anything useful
                // It was added just to show coders ability and understanding of some things

                #region requirments

                // Remove the email decorator
                decoratedNotifier =
                    ((ExternalNotifierDecorator) decoratedNotifier).RemoveDecorator(typeof (EmailNotifierDecorator));

                //Add the email decorator
                decoratedNotifier =
                    ((ExternalNotifierDecorator) decoratedNotifier).AddDecorator(typeof (EmailNotifierDecorator));

                // Does it have NirvanaNotifierdecorator?
                bool nirvanaIsInChain = decoratedNotifier is ExternalNotifierDecorator &&
                                        ((ExternalNotifierDecorator) decoratedNotifier).HasDecorator<Type>(
                                            typeof (NirvanaNotifierDecorator));

                #endregion

                decoratedNotifier.Notify();

                Log.Append(sb.ToString());

                Application.Run();
            }
            else
            {
                Log.Append($"Did not find any prices below {price} - {DateTime.Now}");
            }
        }
    }
}

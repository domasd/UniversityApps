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
using System.Xml.Linq;

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

            Dictionary<string, IPriceParser> parsers = new Dictionary<string, IPriceParser>()
            {
                {nameof(technoramaParser), technoramaParser},
                {nameof(_1aParser), _1aParser},
                {nameof(elektroParser), elektroParser},
                {nameof(egnetasParser), egnetasParser},
                {nameof(omnitelParser), omnitelParser}

            };

            // Read urls
            var urlsPath = ConfigurationManager.AppSettings["UrlsPath"];

            XDocument xmlList = XDocument.Load(urlsPath);
            List<PriceUrl> urisWithParsers = (from m in xmlList.Descendants("Url")
                select new PriceUrl() {parser = parsers[(string) m.Attribute("Parser")], PriceUri = new Uri(m.Value.Trim('\"'))}).ToList();

            CommonPricegetter priceGetter = new CommonPricegetter(urisWithParsers, price);


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

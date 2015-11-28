using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using HtmlAgilityPack;

namespace DailyPriceNotifier_ExtensionObejcts.PriceGetters
{
    public class CommonPricegetter : IPriceGetter
    {
        public decimal PriceToCheck { get; set; }

        public Dictionary<Uri, IPriceParser> ProductsUrlList;

        private List<string> aboveThePrice = new List<string>();

        public CommonPricegetter(Dictionary<Uri,IPriceParser> products, decimal priceTocheck)
        {
            ProductsUrlList = products;
            PriceToCheck = priceTocheck;
        }

        public void CheckPrice()
        {
            using (WebClient client = new WebClient())
            {
                client.Encoding = Encoding.UTF8;
                foreach (var product in ProductsUrlList)
                {
                    string htmlCode = client.DownloadString(product.Key);

                    decimal priceRetrieved = product.Value.Parse(htmlCode);

                    if (priceRetrieved < PriceToCheck)
                    {
                        aboveThePrice.Add(string.Format("{0} {1}", product.Key.AbsoluteUri, priceRetrieved));
                    }

                }
            }
        }

        public string FormMessage()
        {
            StringBuilder sb = new StringBuilder();
            aboveThePrice.ForEach(x=>sb.AppendLine(x));
            return sb.ToString();
        }

    }
}

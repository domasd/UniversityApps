using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace DailyPriceNotifier_Decorator.PriceGetters
{
    public class CommonPricegetter : IPriceGetter
    {
        public decimal PriceToCheck { get; set; }

        public IEnumerable<PriceUrl> ProductsUrlList;

        private List<string> aboveThePrice = new List<string>();

        public CommonPricegetter(IEnumerable<PriceUrl> products, decimal priceTocheck)
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
                    string htmlCode = client.DownloadString(product.PriceUri);

                    decimal priceRetrieved = product.parser.Parse(htmlCode);

                    if (priceRetrieved < PriceToCheck)
                    {
                        aboveThePrice.Add(string.Format("{0} {1}", product.PriceUri.AbsoluteUri, priceRetrieved));
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

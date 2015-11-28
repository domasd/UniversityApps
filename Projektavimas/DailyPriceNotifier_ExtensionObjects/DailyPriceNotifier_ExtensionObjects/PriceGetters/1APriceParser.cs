using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using HtmlAgilityPack;

namespace DailyPriceNotifier_ExtensionObejcts.PriceGetters
{
    public class _1APriceParser : IPriceParser
    {
        public decimal Parse(string htmlContents)
        {
            decimal retrievedPrice = Decimal.MaxValue;
            var doc = new HtmlDocument();
            doc.LoadHtml(htmlContents);

            var element =
                        doc.DocumentNode.Descendants("strong")
                            .Where(x => x.Attributes.Contains("class") && x.Attributes["class"].Value == "price");

            var priceTag = element.FirstOrDefault();
            if (priceTag != null)
            {
                var innerText = priceTag.FirstChild.InnerText;
                if (!string.IsNullOrEmpty(innerText))
                {
                    retrievedPrice = decimal.Parse(innerText.Replace('.',','));
                }
                else
                {
                    Log.Append($"Failed parsing 1A price. Innertext - {innerText}");
                }

            }
            else
            {
                Log.Append($"Failed parsing 1A price. Pricetag not found");

            }

            return retrievedPrice;
        }
    }
}

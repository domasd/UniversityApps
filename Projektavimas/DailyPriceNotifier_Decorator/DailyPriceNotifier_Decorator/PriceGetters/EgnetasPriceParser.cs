﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using HtmlAgilityPack;

namespace DailyPriceNotifier_Decorator.PriceGetters
{
    public class EgnetasPriceParser : IPriceParser
    {
        private string regexpToFindPrice = @"\d+(\.|,)\d{2}\s?€";
        public decimal Parse(string htmlContents)
        {
            decimal retrievedPrice = Decimal.MaxValue;
            var doc = new HtmlDocument();
            doc.LoadHtml(htmlContents);

            var element =
                        doc.DocumentNode.Descendants("span")
                            .Where(x => x.Attributes.Contains("class") && x.Attributes["class"].Value == "price");

            Regex regExp = new Regex(regexpToFindPrice);
            var priceTag = element.FirstOrDefault();
            if (!string.IsNullOrEmpty(priceTag?.InnerText))
            {
                var innerText = priceTag.InnerText;
                var match = regExp.Match(innerText);
                if (!string.IsNullOrEmpty(match.Value))
                {
                    var priceString = match.Value;
                    priceString = priceString.Replace('.', ',').TrimEnd('€').TrimEnd();
                    retrievedPrice = decimal.Parse(priceString);
                }
                else
                {
                    Log.Append($"Failed parsing egnetas price. Innertext - {innerText}");
                }

            }
            else
            {
                Log.Append($"Failed parsing egnetas price. Innertex is null or empty");

            }

            return retrievedPrice;
        }
    }
}

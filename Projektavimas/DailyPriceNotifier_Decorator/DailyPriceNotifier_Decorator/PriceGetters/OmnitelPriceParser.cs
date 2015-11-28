﻿using System;
using System.Linq;
using System.Text.RegularExpressions;
using HtmlAgilityPack;

namespace DailyPriceNotifier_Decorator.PriceGetters
{
    public class OmnitelPriceParser : IPriceParser
    {
            private string regexpToFindPrice = @"\d+(\.|,)?\d{0,2}";

            public decimal Parse(string htmlContents)
            {
                decimal retrievedPrice = Decimal.MaxValue;
                var doc = new HtmlDocument();
                doc.LoadHtml(htmlContents);

                var element =
                    doc.DocumentNode.Descendants("div")
                        .Where(x => x.Attributes.Contains("class") && x.Attributes["class"].Value == "full_price");

                Regex regExp = new Regex(regexpToFindPrice);
                var priceTag = element.FirstOrDefault();
                if (!string.IsNullOrEmpty(priceTag?.InnerText))
                {
                    var innerText = priceTag.InnerText;
                    var match = regExp.Match(innerText);
                    if (!string.IsNullOrEmpty(match.Value))
                    {
                        var priceString = match.Value;
                        priceString = priceString.Replace('.', ',').Replace("Eur", "").TrimEnd(' ');
                        retrievedPrice = decimal.Parse(priceString);
                    }
                    else
                    {
                        Log.Append($"Failed parsing elektromarkt price. Innertext - {innerText}");
                    }

                }
                else
                {
                    Log.Append($"Failed parsing elektromarkt price. Innertex is null or empty");

                }

                return retrievedPrice;
            }
    }
}
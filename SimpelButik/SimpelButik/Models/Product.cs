using SimpelButik.Models.Enums;
using System.Globalization;

namespace SimpelButik.Models;

public class Product
{ 
    public string Name { get; set; }
    public double Price { get; set; }

    public Product(string name, double price)
        {
            Name = name;
            Price = price;
        }

    public string Print(Market market)
    {
         var cultureInfo = market == Market.SE ? "sv-SE" : market == Market.UK ? "en-GB" : "rs-RS";
         double convertedPrice;
         switch (market)
         {
             case Market.UK:
                convertedPrice = Price / 10;
                 break;
             case (Market.RS):
                 convertedPrice = Price * 10;
                 break;
             default:
                 convertedPrice = Price;
                 break;
         }
         return string.Format("{0,-10} | {1,10}", Name.First().ToString().ToUpper() + Name.Substring(1), convertedPrice.ToString("C", new CultureInfo(cultureInfo)));
    }
}




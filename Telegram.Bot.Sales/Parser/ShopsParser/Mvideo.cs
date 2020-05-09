using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Telegram.Bot.Sales.EF;
using Telegram.Bot.Sales.Models;
using Telegram.Bot.Sales.Parser.Extensions;
using Telegram.Bot.Sales.Repos;

namespace Telegram.Bot.Sales.Parser.ShopsParser
{
    public class Mvideo : IParser
    {
        private readonly ApplicationContext _context;
        public Mvideo(ApplicationContext context)
        {
            _context = context;
        }
       
        public async Task<(Product product, string msgAlarm)> ParsingProductAsync(string urlProduct)
        {
            if (urlProduct != null)
            {
                HtmlWeb web = new HtmlWeb();
                try
                {
                    var htmlDoc = await web.LoadFromWebAsync(urlProduct);

                    var name = htmlDoc.DocumentNode.SelectSingleNode(".//h1[@class='e-h1 sel-product-title']");
                    if (name == null)
                    {
                        return (product: null, msgAlarm: "Product name not found!");
                    }
                    // var price = htmlDoc.DocumentNode.SelectSingleNode(".//span[@class='final-cost']");
                    // var productCod = htmlDoc.DocumentNode.SelectSingleNode(".//div[@class='article']");
                    var productCod = htmlDoc.DocumentNode.SelectSingleNode(".//p[@class='c-product-code']");
                    var descr = htmlDoc.DocumentNode.SelectSingleNode(".//div[@class='u-mb-16 u-font-xlarge']");
                    // var descr = htmlDoc.DocumentNode.SelectSingleNode(".//div[@class='j-description description-text collapsable-content j-toogle-height-instance']");

                    // Defenition type Shop.
                    Type myType = typeof(Mvideo);
                    string type = myType.ToString();
                    string[] arr = type.Split(".");
                    string nameClass = arr[arr.Length - 1];
                    Shop shop = null;
                    BaseRepo<Shop> db = new BaseRepo<Shop>(_context);
                    shop = db.GetAll().Find(x => x.Name == nameClass);
                    if (shop == null)
                    {
                        return (product: null, msgAlarm: "Your Shop not found in the database.");
                    }
                    shop.Parser = new Mvideo(_context);

                    return (product: new Product { Name = name?.InnerText, Url = urlProduct, ProductCod = productCod?.InnerText, Shop = shop }, msgAlarm: null);
                }
                catch (Exception e)
                { return (product: null, msgAlarm: e.Message); }
            }
            else
            {
                return (product: null, msgAlarm: "Argument must not be null.");
            }
        }
        public async Task<(ProductPriceHistory productPriceHistory, string msgAlarm)> ParsingPriceAsync(Product product)
        {
            if (product != null)
            {
                // Defenition type Shop.
                Type myType = typeof(Mvideo);
                string type = myType.ToString();
                string[] arr = type.Split(".");
                string nameClass = arr[arr.Length - 1];

                if (product.Shop.Name == nameClass)
                {
                    HtmlWeb web = new HtmlWeb();
                    try
                    {
                        var htmlDoc = await web.LoadFromWebAsync(product.Url);
                        var price = htmlDoc.DocumentNode.SelectSingleNode(".//div[@class='c-pdp-price__current sel-product-tile-price']");
                        if (price == null)
                        {
                            return (productPriceHistory: null, msgAlarm: "Product price not found!");
                        }
                        string strFirst = price.InnerText.Replace("&nbsp;", "");
                        string priceStr = new String(strFirst.Where(Char.IsDigit).ToArray());
                        double pr = Double.Parse(priceStr);
                        var currency = price.InnerText.GetCurrency();
                        Currency cur;
                        BaseRepo<Currency> db = new BaseRepo<Currency>(_context);
                        cur = db.GetAll().Find(x => x.Id == (int)currency);
                        if (cur == null)
                        {
                            return (productPriceHistory: null, msgAlarm: "Currency not found in the database.");
                        }


                        return (productPriceHistory: new ProductPriceHistory { Product = product, DateTime = DateTime.Now, Price = pr, Currency = cur }, msgAlarm: null);
                    }
                    catch (Exception e)
                    { return (productPriceHistory: null, msgAlarm: e.Message); }
                }
                else
                {
                    return (productPriceHistory: null, msgAlarm: "Invalid parser for this product.");
                }
            }
            else
            {
                return (productPriceHistory: null, msgAlarm: "Argument must not be null.");
            }
        }
    }
}

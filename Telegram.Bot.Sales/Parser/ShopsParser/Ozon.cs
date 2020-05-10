using HtmlAgilityPack;
using System;
using System.Linq;
using System.Threading.Tasks;
using Telegram.Bot.Sales.EF;
using Telegram.Bot.Sales.Models;
using Telegram.Bot.Sales.Parser.Extensions;
using Telegram.Bot.Sales.Repos;

namespace Telegram.Bot.Sales.Parser.ShopsParser
{
    public class Ozon : IParser
    {
        private readonly ApplicationContext _context;
        public Ozon(ApplicationContext context)
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
                    //<div data-v-469cfe6c="" class="" data-widget="webProductHeading"><h1 data-v-469cfe6c="" class="b6o0">Стеклоочиститель KARCHER WV 50 Plus (1.633-117.0)</h1></div>
                    var name = htmlDoc.DocumentNode.SelectSingleNode(".//h1[@class='b6o0']");
                    if (name==null)
                    {
                        name = htmlDoc.DocumentNode.SelectSingleNode(".//h1[@class='b5u5']");
                    }
                    if (name == null)
                    {
                        return (product: null, msgAlarm: "Product name not found!");
                    }
                    // var price = htmlDoc.DocumentNode.SelectSingleNode(".//span[@class='final-cost']");
                    // var productCod = htmlDoc.DocumentNode.SelectSingleNode(".//div[@class='article']");
                    var productCod = htmlDoc.DocumentNode.SelectSingleNode(".//span[@class='b7w8 b7x']");
                    if (productCod==null)
                    {
                        productCod = htmlDoc.DocumentNode.SelectSingleNode(".//span[@class='b6a6 b6a5']");
                    }
                    var descr = htmlDoc.DocumentNode.SelectSingleNode(".//div[@class='b7o8']");
                    // var descr = htmlDoc.DocumentNode.SelectSingleNode(".//div[@class='j-description description-text collapsable-content j-toogle-height-instance']");

                    // Defenition type Shop.
                    Type myType = typeof(Ozon);
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
                    shop.Parser = new Ozon(_context);
                    return (product: new Product
                    {
                        Name = name?.InnerText,
                        Url = urlProduct,
                        ProductCod = new string( productCod?.InnerText.Where(char.IsDigit).ToArray()),
                        Shop = shop
                    },
                        msgAlarm: null);
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
                Type myType = typeof(Ozon);
                string type = myType.ToString();
                string[] arr = type.Split(".");
                string nameClass = arr[arr.Length - 1];

                if (product.Shop.Name == nameClass)
                {
                    HtmlWeb web = new HtmlWeb();
                    try
                    {
                        var htmlDoc = await web.LoadFromWebAsync(product.Url);
                        var price = htmlDoc.DocumentNode.SelectSingleNode(".//span[@class='b6t0 b6u3']");//".//span[@class='b6t0 b6u3']"
                        if (price == null)
                        {
                            price = htmlDoc.DocumentNode.SelectSingleNode(".//span[@class='b6t0']");
                        }
                        if (price == null)
                        {
                            price = htmlDoc.DocumentNode.SelectSingleNode(".//span[@class='b6i5']");
                        }
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

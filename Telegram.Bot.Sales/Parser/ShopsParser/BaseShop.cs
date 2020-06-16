using HtmlAgilityPack;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;
using Telegram.Bot.Sales.EF;
using Telegram.Bot.Sales.Models;
using Telegram.Bot.Sales.Parser.Extensions;
using Telegram.Bot.Sales.Repos;

namespace Telegram.Bot.Sales.Parser.ShopsParser
{
    public abstract class BaseShop : IParser
    {
        private readonly ApplicationContext _context;
        private readonly ILogger _logger;

        public abstract string Name { get; }
        public abstract string ProductCod { get; }
        public abstract string Description { get; }
        public abstract string Price { get; }
        public abstract string PriceCleaning { get; }

        public BaseShop(ApplicationContext context, ILogger logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<(Product product, string msgAlarm)> ParsingProductAsync(string urlProduct)
        {
            string name;
            string descr;
            string cod;
            if (urlProduct != null)
            {
                HtmlWeb web = new HtmlWeb();
                try
                {
                    var htmlDoc = await web.LoadFromWebAsync(urlProduct);
                    if (Name != string.Empty)
                    {
                        var nameHtml = htmlDoc.DocumentNode.SelectSingleNode(Name);
                        if (nameHtml == null)
                        {
                            _logger.LogError($"{DateTime.Now} -- {nameof(BaseShop)} -- Product name not found! {urlProduct}");
                            return (product: null, msgAlarm: "Product name not found!");

                        }
                        name = nameHtml.InnerText.Replace("\n","").Trim(new char[] {' '});
                    }
                    else
                        name = "unknown";
                    if (ProductCod != string.Empty)
                    {
                        var codHtml = htmlDoc.DocumentNode.SelectSingleNode(ProductCod);
                        cod = codHtml.InnerText;
                    }
                    else
                        cod = "unknown";
                    if (Description != string.Empty)
                    {
                        var descrHtml = htmlDoc.DocumentNode.SelectSingleNode(Description);
                        descr = descrHtml.InnerText;
                    }
                    else
                        descr = "unknown";
                    

                    // Defenition type Shop.
                    var uri = new Uri(urlProduct);
                    string host = uri.Host;
                    if (!host.Contains("www."))
                    {
                        host = "www." + host;
                    }
                   
                    Shop shop = null;
                    BaseRepo<Shop> db = new BaseRepo<Shop>(_context);
                    shop = db.GetAll().Find(x => x.Url == host);
                    if (shop == null)
                    {
                        _logger.LogError($"{DateTime.Now} -- {nameof(BaseShop)} --  Your Shop not found in the database.");
                        return (product: null, msgAlarm: "Your Shop not found in the database.");
                    }

                    return (product: new Product { Name = name, Url = urlProduct, ProductCod = cod, Shop = shop }, msgAlarm: null);
                }
                catch (Exception e)
                {
                    _logger.LogError($"{DateTime.Now} -- {nameof(BaseShop)} --  {e.Message}");
                    return (product: null, msgAlarm: e.Message);
                }
            }
            else
            {
                _logger.LogError($"{DateTime.Now} -- {nameof(BaseShop)} --  Argument must not be null.");
                return (product: null, msgAlarm: "Argument must not be null.");
            }
        }
        public async Task<(ProductPriceHistory productPriceHistory, string msgAlarm)> ParsingPriceAsync(Product product)
        {
            if (product != null)
            {
                HtmlWeb web = new HtmlWeb();
                try
                {
                    var htmlDoc = await web.LoadFromWebAsync(product.Url);
                    var price = htmlDoc.DocumentNode.SelectSingleNode(Price);
                    if (price == null)
                    {
                        return (productPriceHistory: null, msgAlarm: $"{product.Url} Product price not found!");
                    }
                    string strFirst = price.InnerText;
                    if (PriceCleaning!=string.Empty)
                    {
                        strFirst = strFirst.Replace(PriceCleaning, "");
                    }
                    string priceStr = new String(strFirst.Where(Char.IsDigit).ToArray());
                    double pr = Double.Parse(priceStr);
                    var currency = price.InnerText.GetCurrency();
                    //Currency cur;
                    //BaseRepo<Currency> db = new BaseRepo<Currency>(_context);
                    //cur = db.GetAll().Find(x => x.Id == (int)currency);
                    //if (cur == null)
                    //{
                    //    return (productPriceHistory: null, msgAlarm: "Currency not found in the database.");
                    //}
                    return (productPriceHistory: new ProductPriceHistory { Product = product, DateTime = DateTime.Now, Price = pr, CurrencyId = 1 }, msgAlarm: null);
                }
                catch (Exception e)
                {
                    _logger.LogError($"{DateTime.Now} -- {nameof(BaseShop)} --  {e.Message}");
                    return (productPriceHistory: null, msgAlarm: e.Message);
                }
            }
            else
            {
                _logger.LogError($"{DateTime.Now} -- {nameof(BaseShop)} --  Argument must not be null.");
                return (productPriceHistory: null, msgAlarm: $"Argument must not be null.");
            }
        }
    }
}

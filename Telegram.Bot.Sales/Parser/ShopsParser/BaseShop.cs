﻿using HtmlAgilityPack;
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

        public abstract string Name { get; }
        public abstract string ProductCod { get; }
        public abstract string Description { get; }
        public abstract string Price { get; }
        public abstract string PriceCleaning { get; }

        public BaseShop(ApplicationContext context)
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

                    var name = htmlDoc.DocumentNode.SelectSingleNode(Name);
                    if (name == null)
                    {
                        return (product: null, msgAlarm: "Product name not found!");
                    }
                    var productCod = htmlDoc.DocumentNode.SelectSingleNode(ProductCod);
                    var descr = htmlDoc.DocumentNode.SelectSingleNode(Description);

                    // Defenition type Shop.
                    Type myType = typeof(Bask);
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
                HtmlWeb web = new HtmlWeb();
                try
                {
                    var htmlDoc = await web.LoadFromWebAsync(product.Url);
                    var price = htmlDoc.DocumentNode.SelectSingleNode(Price);
                    if (price == null)
                    {
                        return (productPriceHistory: null, msgAlarm: "Product price not found!");
                    }
                    string strFirst = price.InnerText.Replace(PriceCleaning, "");
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
                return (productPriceHistory: null, msgAlarm: "Argument must not be null.");
            }
        }
    }
}

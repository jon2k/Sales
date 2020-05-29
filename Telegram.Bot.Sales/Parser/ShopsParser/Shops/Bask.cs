﻿using Microsoft.Extensions.Logging;
using Telegram.Bot.Sales.EF;

namespace Telegram.Bot.Sales.Parser.ShopsParser
{
    public class Bask : BaseShop
    {
        public Bask(ApplicationContext context, ILogger logger) : base(context,logger)
        {
        }
        public override string Name => ".//div[@class='caption text-center border-bottom fs21 text-bold']";
        public override string ProductCod => ".//div[@class='article-item']";
        public override string Description => "";
        public override string Price => ".//div[@class='price']";
        public override string PriceCleaning => "";
    }
}

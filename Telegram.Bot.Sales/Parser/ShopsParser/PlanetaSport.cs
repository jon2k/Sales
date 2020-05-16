using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Telegram.Bot.Sales.EF;

namespace Telegram.Bot.Sales.Parser.ShopsParser
{
    public class PlanetaSport : BaseShop
    {
        public PlanetaSport(ApplicationContext context, ILogger logger) : base(context, logger)
        {

        }
        public override string Name => ".//h1[@class='productHeader']";

        public override string ProductCod => "0";

        public override string Description => ".//div[@class='detail-products__mobiletabs-text']";

        public override string Price => ".//span[@class='onlyMoney']";

        public override string PriceCleaning => " ";
    }
}

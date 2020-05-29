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
        public override string Name => ".//span[@class='hitnProductName']";

        public override string ProductCod => "";

        public override string Description => "";

        public override string Price => ".//span[@class='onlyMoney']";

        public override string PriceCleaning => " ";
    }
}

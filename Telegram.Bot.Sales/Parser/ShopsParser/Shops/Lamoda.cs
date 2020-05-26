using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Telegram.Bot.Sales.EF;

namespace Telegram.Bot.Sales.Parser.ShopsParser
{
    public class Lamoda : BaseShop
    {
        public Lamoda(ApplicationContext context, ILogger logger):base(context, logger)
        {

        }
        public override string Name => ".//span[@class='heading_m ii-product__title']";

        public override string ProductCod => "";

        public override string Description => "";

        public override string Price => ".//span[@class='ii-product__price-current ii-product__price-current_big']";

        public override string PriceCleaning => "";
    }
}

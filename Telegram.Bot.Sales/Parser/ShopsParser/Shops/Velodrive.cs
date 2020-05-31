using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Telegram.Bot.Sales.EF;

namespace Telegram.Bot.Sales.Parser.ShopsParser
{
    public class Velodrive : BaseShop
    {
        public Velodrive(ApplicationContext context, ILogger logger) : base(context, logger)
        {

        }
        public override string Name => ".//div[@class='header__mobile_page-title d-flex flex-row d-l-none']";

        public override string ProductCod => ".//div[@class='card-page__article']";

        public override string Description => "";

        public override string Price => ".//span[@class='price price--sale']";

        public override string PriceCleaning => "";
    }
}

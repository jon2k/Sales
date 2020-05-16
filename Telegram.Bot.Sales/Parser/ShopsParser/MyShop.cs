using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Telegram.Bot.Sales.EF;

namespace Telegram.Bot.Sales.Parser.ShopsParser
{
    public class MyShop : BaseShop
    {
        public MyShop(ApplicationContext context, ILogger<Parsing> logger) : base(context, logger)
        {

        }
        public override string Name => ".//h1[@itemprop='name']";

        public override string ProductCod => throw new NotImplementedException();

        public override string Description => ".//div[@class='newtd']";

        public override string Price => throw new NotImplementedException();

        public override string PriceCleaning => throw new NotImplementedException();
    }
}

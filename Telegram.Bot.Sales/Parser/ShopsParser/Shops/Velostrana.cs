using Microsoft.Extensions.Logging;
using Telegram.Bot.Sales.EF;

namespace Telegram.Bot.Sales.Parser.ShopsParser
{
    public class Velostrana:BaseShop
    {
        public Velostrana(ApplicationContext context, ILogger logger) : base(context, logger)
        {
        }
        public override string Name => ".//h1[@class='page-title']";
        public override string ProductCod => ".//div[@class='productfull__code']";
        public override string Description => "";
        public override string Price => ".//div[@class='productfull__price-new']";
        public override string PriceCleaning => "";
    }
}

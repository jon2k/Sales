using Microsoft.Extensions.Logging;
using Telegram.Bot.Sales.EF;

namespace Telegram.Bot.Sales.Parser.ShopsParser
{
    public class Bask : BaseShop
    {
        public Bask(ApplicationContext context, ILogger logger) : base(context,logger)
        {
        }
        public override string Name => ".//h1[@class='p-block__name sm-hidden h3']";
        public override string ProductCod => ".//span[@class='art']";
        public override string Description => "";
        public override string Price => ".//span[@class='avail-b']";
        public override string PriceCleaning => "";
    }
}

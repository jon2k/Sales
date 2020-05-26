using Microsoft.Extensions.Logging;
using Telegram.Bot.Sales.EF;

namespace Telegram.Bot.Sales.Parser.ShopsParser
{
    public class Velosklad:BaseShop
    {
        public Velosklad(ApplicationContext context, ILogger logger) : base(context, logger)
        {
        }
        public override string Name => ".//div[@class='prod-cart-head']//h1";
        public override string ProductCod => ".//div[@class='prod-cart-art']//span";
        public override string Description => "";
        public override string Price => ".//div[@class='prod-cart-bl-r-price-left-bottom']";
        public override string PriceCleaning => "";
    }
}

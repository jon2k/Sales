using Telegram.Bot.Sales.EF;

namespace Telegram.Bot.Sales.Parser.ShopsParser
{
    public class Mvideo : BaseShop
    {       
        public Mvideo(ApplicationContext context):base(context)
        {
   
        }

        public override string Name => ".//h1[@class='e-h1 sel-product-title']";

        public override string ProductCod => ".//p[@class='c-product-code']";

        public override string Description => ".//div[@class='u-mb-16 u-font-xlarge']";

        public override string Price => ".//div[@class='c-pdp-price__current sel-product-tile-price']";

        public override string PriceCleaning => "&nbsp;";
    }
}

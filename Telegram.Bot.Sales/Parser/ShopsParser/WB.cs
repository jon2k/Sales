using Telegram.Bot.Sales.EF;

namespace Telegram.Bot.Sales.Parser.ShopsParser
{
    public class WB : BaseShop
    {
       
        public WB(ApplicationContext context):base(context)
        {
           
        }

        public override string Name => ".//div[@class='brand-and-name j-product-title']";

        public override string ProductCod => ".//span[@class='j-article']";

        public override string Description => ".//div[@class='description j-collapsable-description i-collapsable-v1']";

        public override string Price => ".//span[@class='final-cost']";

        public override string PriceCleaning => "&#xA0";
    }
}

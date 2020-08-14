using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Telegram.Bot.Sales.EF;

namespace Telegram.Bot.Sales.Parser.ShopsParser
{
    public class TrialSport : BaseShop
    {
        public TrialSport(ApplicationContext context, ILogger logger) : base(context, logger)
        {

        }
        public override string Name => ".//div[@class='card_right']//h2";

        public override string ProductCod => "";

        public override string Description => "";

        public override string Price => "//table[@class='b25 prices_for_popup']/tr[1]/td[2]/div[@class='price']";

        public override string PriceCleaning => "";
    }
}

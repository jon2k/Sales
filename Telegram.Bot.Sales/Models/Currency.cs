using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Telegram.Bot.Sales.Models
{
    public class Currency : EntityBase
    {
        [StringLength(10)]
        public string CurrencyType { get; set; }

        public List<ProductPriceHistory> ProductPriceHistories { get; set; }
    }
}

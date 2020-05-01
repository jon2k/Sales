using System;

namespace Telegram.Bot.Sales.Models
{
    public class ProductPriceHistory : EntityBase
    {
        public int ProductId { get; set; }
        public Product Product { get; set; }
        public double Price { get; set; }
        public int CurrencyId { get; set; }
        public Currency Currency { get; set; }
        public DateTime DateTime { get; set; }
    }
}

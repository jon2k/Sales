using System.Collections.Generic;

namespace Telegram.Bot.Sales.Models
{
    public class Product : EntityBase
    {
        // [StringLength(50)]
        public string Name { get; set; }
        // [StringLength(100)]
        public string Description { get; set; }
        // [StringLength(20)]
        public string ProductCod { get; set; }
        public string Url { get; set; }
        public int ShopId { get; set; }
        public Shop Shop { get; set; }

        public List<ProductPriceHistory> ProductPriceHistories { get; set; }
        public List<OrderHistory> OrderHistories { get; set; }
        public List<OrderCurrent> OrderCurrents { get; set; }
    }
}

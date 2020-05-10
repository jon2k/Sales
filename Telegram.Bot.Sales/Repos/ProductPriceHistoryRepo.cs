using System;
using System.Linq;
using System.Threading.Tasks;
using Telegram.Bot.Sales.EF;
using Telegram.Bot.Sales.Models;

namespace Telegram.Bot.Sales.Repos
{
    public class ProductPriceHistoryRepo : BaseRepo<ProductPriceHistory>
    {
        public ProductPriceHistoryRepo(ApplicationContext context) : base(context)
        {

        }
        public override Task<int> Add(ProductPriceHistory entity)
        {
            Context.Products.Attach(entity.Product);
            Context.Currencies.Attach(entity.Currency);

            return base.Add(entity);
        }
        public ProductPriceHistory GetByTime(Product product, DateTime time)
        {
           return _table.Where(n => n.DateTime == time && n.ProductId==product.Id)
                .FirstOrDefault();
        }
        public ProductPriceHistory GetLastByTime(Product product)
        {
            return _table.Where(n => n.ProductId == product.Id)
                .OrderByDescending(n=>n.DateTime)
                . FirstOrDefault();
        }
    }
}

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
    }
}

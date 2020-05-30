using System.Linq;
using System.Threading.Tasks;
using Telegram.Bot.Sales.EF;
using Telegram.Bot.Sales.Models;

namespace Telegram.Bot.Sales.Repos
{
    public class ProductRepo : BaseRepo<Product>
    {
        public ProductRepo(ApplicationContext context) : base(context)
        {

        }
        public Product GetProductByUrl(string url)
        {
            return _table.Where(x => x.Url.Equals(url)).FirstOrDefault();

        }
        public override Task<int> Add(Product entity)
        {
            Context.Shops.Attach(entity.Shop);
            return base.Add(entity);

        }
    }
}

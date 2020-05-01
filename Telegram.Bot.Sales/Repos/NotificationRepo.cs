using System.Threading.Tasks;
using Telegram.Bot.Sales.EF;
using Telegram.Bot.Sales.Models;

namespace Telegram.Bot.Sales.Repos
{
    public class NotificationRepo : BaseRepo<Notification>
    {
        public NotificationRepo(ApplicationContext context):base(context)
        {

        }
        public override Task<int> Add(Notification entity)
        {
            Context.Customers.Attach(entity.OrderCurrent.Customer);
            Context.Products.Attach(entity.OrderCurrent.Product);
            return base.Add(entity);
        }      
    }
}

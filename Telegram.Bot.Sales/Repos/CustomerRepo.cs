using System.Collections.Generic;
using System.Linq;
using Telegram.Bot.Sales.EF;
using Telegram.Bot.Sales.Models;

namespace Telegram.Bot.Sales.Repos
{ 
    public class CustomerRepo : BaseRepo<Customer>
    {
        public CustomerRepo(ApplicationContext context):base(context)
        {

        }
        public override List<Customer> GetAll()
            => Context.Customers.OrderBy(x => x.Name).ToList();

        public Customer GetOneByCodTelegram(long? id)
        {
            return Context.Customers.FirstOrDefault(x => x.CodeTelegram == id);
        }

        public List<Customer> GetActualCustomers(long codeTelegram)
        {
            return Context.Customers.Where(x => x.CodeTelegram == codeTelegram && x.IsDeleted == false).ToList();
        }
        public Customer GetDeletedCustomers(long codeTelegram)
        {
            return Context.Customers.FirstOrDefault(x => x.CodeTelegram == codeTelegram && x.IsDeleted == true);
        }
    }
}

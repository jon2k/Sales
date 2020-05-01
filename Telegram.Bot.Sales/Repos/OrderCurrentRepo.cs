using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Telegram.Bot.Sales.EF;
using Telegram.Bot.Sales.Models;

namespace Telegram.Bot.Sales.Repos
{
    public class OrderCurrentRepo : BaseRepo<OrderCurrent>
    {
        public OrderCurrentRepo(ApplicationContext context) : base(context)
        {

        }
        public bool CheckExistOrder(OrderCurrent entity)
        {
            var res = _table.Where(
                n => n.Customer.CodeTelegram == entity.Customer.CodeTelegram
                && n.Product.Id == entity.Product.Id)
                 .FirstOrDefault();
            return res == null ? false : true;
        }
        public override Task<int> Add(OrderCurrent entity)
        {
            Context.Customers.Attach(entity.Customer);
            Context.Products.Attach(entity.Product);
            return base.Add(entity);
        }

        public IEnumerable<OrderCurrent> GetAllOrderCurrentByCustomer(Customer customer)
        {
            return _table.Where(n => n.Customer.CodeTelegram == customer.CodeTelegram);
        }

        public OrderCurrent GetConcreteOrderCustomers(long codeTelegram, int idProduct)
        {
            return _table.Where(n => n.Customer.CodeTelegram == codeTelegram && n.ProductId == idProduct).FirstOrDefault();
        }

        public IEnumerable<Product> GetAllProductByCustomer(Customer customer)
        {
            var res = from c in Context.Products
                      join p in Context.OrdersCurrent on c.Id equals p.ProductId
                      select c;
            return res;
        }

        public async Task<int> MoveOrdersCurrentToOrdersHistory(IEnumerable<OrderCurrent> orderCurrents)
        {
            if (orderCurrents != null)
            {


                foreach (var item in orderCurrents)
                {

                    Context.OrdersHistory.Add(new OrderHistory()
                    {
                        CustomerId = item.CustomerId,
                        ProductId = item.ProductId,
                        TimeStartNotif = item.TimeStartNotif,
                        TimeStopNotif = DateTime.Now
                    });
                }
                _table.RemoveRange(orderCurrents);
                return await SaveChangesAsync();
            }
            else
                return 0;
        }

        public async Task<int> MoveOrderCurrentToOrderHistory(OrderCurrent orderCurrent)
        {
            if (orderCurrent != null)
            {
                Context.OrdersHistory.Add(new OrderHistory()
                {
                    CustomerId = orderCurrent.CustomerId,
                    ProductId = orderCurrent.ProductId,
                    TimeStartNotif = orderCurrent.TimeStartNotif,
                    TimeStopNotif = DateTime.Now
                });

                _table.RemoveRange(orderCurrent);

                var notif= Context.Notifications.Where(n => n.OrderCurrentId == orderCurrent.Id);
                foreach (var item in notif)
                {
                    Context.Entry(item).State = EntityState.Deleted;
                }

                return await SaveChangesAsync();
            }
            else
                return 0;
        }
    }
}

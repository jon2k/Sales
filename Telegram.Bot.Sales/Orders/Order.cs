using System;
using Telegram.Bot.Sales.Models;

namespace Telegram.Bot.Sales.Orders
{
    public class Order
    {
        public (OrderCurrent orderCurrent, string msgAlarm) CreateOrderCurrent(Customer customer, Product product, DateTime time, Byte percentDiscount)
        {
            if (customer != null && product != null)
            {
                OrderCurrent orderCurrent = new OrderCurrent();
                orderCurrent.Customer = customer;
                orderCurrent.Product = product;
                orderCurrent.TimeStartNotif = time;               
                orderCurrent.ExpectedPercentDiscount = percentDiscount;

                return (orderCurrent, null);
            }
            else
            {
                return (orderCurrent: null, msgAlarm: "Argument must not be null.");
            }

        }

        public (OrderHistory, string) CreateOrderHistiry(OrderCurrent orderCurrent)
        {
            if (orderCurrent != null)
            {
                OrderHistory orderHistory = new OrderHistory();
                orderHistory.Customer = orderCurrent.Customer;
                orderHistory.Product = orderCurrent.Product;
                orderHistory.TimeStartNotif = orderCurrent.TimeStartNotif;
                orderHistory.TimeStopNotif = DateTime.Now;

                return (orderHistory, null);
            }
            else
            {
                return (null, "Argument must not be null.");
            }
        }
    }
}

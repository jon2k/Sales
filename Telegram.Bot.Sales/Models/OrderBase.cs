using System;

namespace Telegram.Bot.Sales.Models
{
    public class OrderBase : EntityBase
    {
        public int CustomerId { get; set; }
        public Customer Customer { get; set; }
        public int ProductId { get; set; }
        public Product Product { get; set; }
        public DateTime TimeStartNotif { get; set; }
    }
}

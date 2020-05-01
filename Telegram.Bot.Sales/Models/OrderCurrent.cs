using System.Collections.Generic;

namespace Telegram.Bot.Sales.Models
{ 
    public class OrderCurrent : OrderBase
    {
        // Way notification Customer of discount.
       // public int NotificationId { get; set; }
       // public Notification Notification { get; set; }
        // Expected customer discount percentage.
        public byte ExpectedPercentDiscount { get; set; }
        public List<Notification> Notifications { get; set; }
    }
}

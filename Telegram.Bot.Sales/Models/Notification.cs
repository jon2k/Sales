namespace Telegram.Bot.Sales.Models
{
    public class Notification : EntityBase
    {
        public OrderCurrent OrderCurrent { get; set; }
        public int OrderCurrentId { get; set; }
        public TypeNotification TypeNotification { get; set; }
        public int TypeNotificationId { get; set; }
        //public bool EMail { get; set; }
        //public bool Telegram { get; set; }
        //public bool Viber { get; set; }
        //public bool WhatsApp { get; set; }
        //public bool SMS { get; set; }
        //public bool VK { get; set; }

      //  public List<OrderCurrent> OrderCurrents { get; set; }

    }
}

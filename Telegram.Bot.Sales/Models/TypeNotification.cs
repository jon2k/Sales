using System.Collections.Generic;

namespace Telegram.Bot.Sales.Models
{
    public class TypeNotification : EntityBase
    {
        public string Name { get; set; }
        public List<Notification> Notifications { get; set; }
    }
}

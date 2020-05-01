using System.ComponentModel.DataAnnotations;

namespace Telegram.Bot.Sales.Models
{
    public class EntityBase
    {
        [Key]
        public int Id { get; set; }
        [Timestamp]
        public byte[] Timestamp { get; set; }

    }
}

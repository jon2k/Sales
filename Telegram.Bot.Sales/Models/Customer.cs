using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Telegram.Bot.Sales.Models
{
    public class Customer : EntityBase
    {
        public long CodeTelegram { get; set; }

        [StringLength(50)]
        public string Name { get; set; }

        [StringLength(50)]
        public string Email { get; set; }

        public DateTime DateOfBirth { get; set; }

        public int? GenderId { get; set; }

        public Gender Gender { get; set; }

        public bool IsDeleted { get; set; }


        public List<OrderHistory> OrderHistories { get; set; }
        public List<OrderCurrent> OrderCurrents { get; set; }
    }
}

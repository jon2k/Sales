using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Telegram.Bot.Sales.Models
{
    public class Gender : EntityBase
    {
        [StringLength(10)]
        public string GenderType { get; set; }

        public List<Customer> Customers { get; set; }

    }
}

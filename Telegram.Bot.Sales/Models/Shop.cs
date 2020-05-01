using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Telegram.Bot.Sales.Parser;

namespace Telegram.Bot.Sales.Models
{
    public class Shop : EntityBase
    {
        [StringLength(50)]
        public string Name { get; set; }
        [StringLength(50)]
        public string Url { get; set; }

        public List<Product> Products { get; set; }

        [NotMapped]
        public IParser Parser { get; set; }

        public override bool Equals(object obj)
        {
            if (obj is Shop)
            {
                return ((Shop)obj).Name.Equals(Name);
            }
            else
                return false;
        }
        public override int GetHashCode()
        {
            return Name.GetHashCode();
        }
    }
}

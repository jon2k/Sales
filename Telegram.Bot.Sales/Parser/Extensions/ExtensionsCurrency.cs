using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Telegram.Bot.Sales.Parser.Extensions
{
    public enum CurrencyType
    {
        Empty,
        Russia,
        USA,
        Europa,
        England
    }

    public static class ExtensionsCurrency
    {
        public static CurrencyType GetCurrency(this string text)
        {
            if (text.Contains("Руб") || text.Contains("Р") || text.Contains("P") 
                || (text.Contains("руб") || text.Contains("р") || text.Contains("p") 
                || text.Contains("RUR") || text.Contains("rur") || text.Contains("₽") || text.Contains("¤")))
            {
                return CurrencyType.Russia;
            }
            else if (text.Contains("$"))
            {
                return CurrencyType.USA;
            }
            else if (text.Contains("EURO") || text.Contains("Euro") || text.Contains("euro"))
            {
                return CurrencyType.Europa;
            }
            else
                return CurrencyType.Empty;
        }
    }
}

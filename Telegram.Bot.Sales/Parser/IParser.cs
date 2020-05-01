using System.Threading.Tasks;
using Telegram.Bot.Sales.EF;
using Telegram.Bot.Sales.Models;

namespace Telegram.Bot.Sales.Parser
{
    public interface IParser
    {
        Task<(Product product, string msgAlarm)> ParsingProductAsync(string urlProduct);
        Task<(ProductPriceHistory productPriceHistory, string msgAlarm)> ParsingPriceAsync(Product product);
    }
}

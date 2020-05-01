using System.Threading.Tasks;
using Telegram.Bot.Types;

namespace Telegram.Bot.Sales.Services
{
    public interface IUpdateService
    {
        Task EchoAsync(Update update);
    }
}

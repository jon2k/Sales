using System.Threading.Tasks;
using Telegram.Bot.Types;

namespace Telegram.Bot.Sales.SelectService
{
    public interface ISelectService
    {
        Task SelectTypeUpdateAndExecute(Update update);
    }
}

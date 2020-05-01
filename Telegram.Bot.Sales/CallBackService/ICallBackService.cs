using System.Threading.Tasks;
using Telegram.Bot.Types;

namespace Telegram.Bot.Sales.CallBackService
{
    public interface ICallBackService
    {
        Task Execute(CallbackQuery callback);
    }
}

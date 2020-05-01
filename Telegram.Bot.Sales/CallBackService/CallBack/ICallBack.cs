using System.Threading.Tasks;
using Telegram.Bot.Types;

namespace Telegram.Bot.Sales.CallBackService.CallBack
{
    interface ICallBack
    {
        public string Name { get; }
        bool Contains(string messageButton);
        Task Execute(CallbackQuery callBack);
    }
}

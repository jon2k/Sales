using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Telegram.Bot.Sales.CallBackService.CallBack;
using Telegram.Bot.Sales.CommandService.Commands;
using Telegram.Bot.Sales.EF;
using Telegram.Bot.Sales.Services;
using Telegram.Bot.Types;

namespace Telegram.Bot.Sales.CallBackService
{
    public class CallBackService : ICallBackService
    {
        private List<ICallBack> _callBack;
        private readonly IBotService _botService;
        private readonly ILogger<CallBackService> _logger;
        private readonly ApplicationContext _context;

        public CallBackService(IBotService botService, ILogger<CallBackService> logger, ApplicationContext context)
        {
            _botService = botService;
            _logger = logger;
            _context = context;
            _callBack = new List<ICallBack>();
            _callBack.Add(new CallBackRemoveProductFromWaitingList(_botService, _logger, _context));
            _callBack.Add(new CallBackSetWaitingSale(_botService, _logger, _context));
        }
        public async Task Execute(CallbackQuery callback)
        {
            foreach (var item in _callBack)
            {
                if (item.Contains(callback.Data))
                {
                    await item.Execute(callback);
                    break;
                }
            }
        }
    }
}

using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Telegram.Bot.Sales.CommandService.Commands;
using Telegram.Bot.Sales.EF;
using Telegram.Bot.Sales.Services;
using Telegram.Bot.Types;

namespace Telegram.Bot.Sales.CallBackService.CallBack
{
    public class CallBackSetWaitingSale:ICallBack
    {
        private readonly IBotService _botService;
        private readonly ILogger<StartCommand> _logger;
        private readonly ApplicationContext _context;

        public CallBackSetWaitingSale(IBotService botService, ILogger<StartCommand> logger, ApplicationContext context)
        {
            _botService = botService;
            _logger = logger;
            _context = context;
        }

        public string Name => throw new NotImplementedException();

        public bool Contains(string messageButton)
        {
            throw new NotImplementedException();
        }

        public Task Execute(CallbackQuery callBack)
        {
            throw new NotImplementedException();
        }
    }
}

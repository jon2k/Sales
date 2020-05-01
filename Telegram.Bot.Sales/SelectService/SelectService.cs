using System.Threading.Tasks;
using Telegram.Bot.Sales.CallBackService;
using Telegram.Bot.Sales.CommandService;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace Telegram.Bot.Sales.SelectService
{
    public class SelectService : ISelectService
    {
        private readonly ICallBackService _callBackService;
        private readonly ICommandService _commandService;

        public SelectService(ICallBackService callBack, ICommandService command)
        {
            _callBackService = callBack;
            _commandService = command;
        }
        public async Task SelectTypeUpdateAndExecute(Update update)
        {
            switch (update.Type)
            {
                case UpdateType.CallbackQuery:
                    await _callBackService.Execute(update.CallbackQuery);
                    break;
                case UpdateType.Message:
                   await _commandService.Execute(update.Message);
                    break;
                case UpdateType.EditedMessage:
                   
                    break;
                case UpdateType.InlineQuery:
                   
                    break;
                case UpdateType.ChosenInlineResult:
                   
                    break;
            }
        }
    }
}

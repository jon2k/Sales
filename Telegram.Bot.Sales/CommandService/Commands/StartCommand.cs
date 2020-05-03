using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Telegram.Bot.Sales.Services;
using Telegram.Bot.Types;

namespace Telegram.Bot.Sales.CommandService.Commands
{
    public class StartCommand:ICommand
    {
        public string Name => @"/start";

        private readonly IBotService _botService;
        private readonly ILogger<CommandsService> _logger;

        public StartCommand(IBotService botService, ILogger<CommandsService> logger)
        {
            _botService = botService;
            _logger = logger;
        }
        public bool Contains(Message message)
        {
            if (message.Type != Telegram.Bot.Types.Enums.MessageType.Text)
                return false;

            return message.Text.Contains(this.Name);
        }

        public async Task Execute(Message message)
        {
            var chatId = message.Chat.Id;
            string MessageToCustomer =
@"List commands:
/register - registration in the system
/delete - delete my registration data
/show - show all my products on the waiting list
/shops - available shops for added product to the waiting list
http://example.com/product1 insert to add product to the waiting list";
            
            await _botService.Client.SendTextMessageAsync(chatId, MessageToCustomer, parseMode: Telegram.Bot.Types.Enums.ParseMode.Markdown);
        }

    }
}

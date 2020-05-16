using System;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Telegram.Bot.Sales.EF;
using Telegram.Bot.Sales.Models;
using Telegram.Bot.Sales.Repos;
using Telegram.Bot.Sales.Services;
using Telegram.Bot.Types;

namespace Telegram.Bot.Sales.CommandService.Commands
{
    public class ShowShopsCommand : ICommand
    {
        public string Name => @"/shops";

        private readonly IBotService _botService;
        private readonly ILogger<CommandsService> _logger;
        private readonly ApplicationContext _context;

        public ShowShopsCommand(IBotService botService, ILogger<CommandsService> logger, ApplicationContext context)
        {
            _botService = botService;
            _logger = logger;
            _context = context;
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
            string messageToCustomer = string.Empty;
            StringBuilder text = new StringBuilder();
            try
            {
                BaseRepo<Shop> repo = new BaseRepo<Shop>(_context);
                var shops = repo.GetAll();
                foreach (var shop in shops)
                {
                    text.Append(shop.Name + " " + shop.Url + "\n");
                }
                messageToCustomer = text.ToString();
            }
            catch (Exception e)
            {
                messageToCustomer = "Failed to get store data";
                _logger.LogError($"{DateTime.Now} -- {nameof(ShowShopsCommand)} --  {e.Message}");
            }
            await _botService.Client.SendTextMessageAsync(chatId, messageToCustomer, parseMode: Telegram.Bot.Types.Enums.ParseMode.Markdown);
        }
    }
}

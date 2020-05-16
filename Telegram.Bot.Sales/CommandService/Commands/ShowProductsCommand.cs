using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Telegram.Bot.Sales.EF;
using Telegram.Bot.Sales.Models;
using Telegram.Bot.Sales.Repos;
using Telegram.Bot.Sales.Services;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace Telegram.Bot.Sales.CommandService.Commands
{
    public class ShowProductsCommand : ICommand
    {
        public string Name => @"/show";
        private readonly IBotService _botService;
        private readonly ILogger<CommandsService> _logger;
        private readonly ApplicationContext _context;

        public ShowProductsCommand(IBotService botService, ILogger<CommandsService> logger, ApplicationContext context)
        {
            _botService = botService;
            _logger = logger;
            _context = context;
        }

        public bool Contains(Message message)
        {
            if (message.Type != Telegram.Bot.Types.Enums.MessageType.Text)
                return false;

            return (message.Text.Contains(this.Name) & message.Text.StartsWith(this.Name));
        }

        public async Task Execute(Message message)
        {
            string MessageToCustomer = string.Empty;
            bool success = false;
            try
            {
                IEnumerable<Product> products;
                var chatId = message.Chat.Id;
                Customer customer;
                CustomerRepo repo = new CustomerRepo(_context);
                customer = repo.GetOneByCodTelegram(chatId);

                if (customer != null && !customer.IsDeleted)
                {
                    OrderCurrentRepo repo2 = new OrderCurrentRepo(_context);
                    products = repo2.GetAllProductByCustomer(customer).ToList();
                    if (products != null && products.Count() != 0)
                    {
                        foreach (var product in products)
                        {
                            string data = $"Delete {product.Id}";
                            var inlineKeyboard = new InlineKeyboardMarkup(new[]
                            {
                                new []
                                {
                                    InlineKeyboardButton.WithCallbackData("Delete", data)                     
                                }
                            });

                            await _botService.Client.SendTextMessageAsync(message.Chat.Id, product.Url, replyMarkup: inlineKeyboard);
                        }
                        success = true;
                    }
                    else
                    {
                        MessageToCustomer = "You don't have product in the waiting list";
                    }
                }
                else
                    MessageToCustomer = "You are not registred!!";

            }
            catch (Exception e)
            {
                MessageToCustomer = e.Message;
                _logger.LogError($"{DateTime.Now} -- {nameof(ShowProductsCommand)} --  {e.Message}");
            }
            if (!success)
            {
                await _botService.Client.SendTextMessageAsync(message.Chat.Id, MessageToCustomer, parseMode: Telegram.Bot.Types.Enums.ParseMode.Markdown);
            }
        }
    }
}

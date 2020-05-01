using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Telegram.Bot.Sales.CommandService.Commands;
using Telegram.Bot.Sales.EF;
using Telegram.Bot.Sales.Repos;
using Telegram.Bot.Sales.Services;
using Telegram.Bot.Types;

namespace Telegram.Bot.Sales.CallBackService.CallBack
{
    public class CallBackRemoveProductFromWaitingList : ICallBack
    {
        private readonly IBotService _botService;
        private readonly ILogger<StartCommand> _logger;
        private readonly ApplicationContext _context;
        public CallBackRemoveProductFromWaitingList(IBotService botService, ILogger<StartCommand> logger, ApplicationContext context)
        {
            _botService = botService;
            _logger = logger;
            _context = context;
        }

        public string Name => "Delete";

        public bool Contains(string messageButton)
        {
            if (messageButton != null)
            {
                string[] textButton = messageButton.Split();
                return textButton[0] == Name ? true : false;
            }
            else
                return false;
        }

        public async Task Execute(CallbackQuery callback)
        {
            try
            {
                string[] data = callback.Data.Split();

                if (int.TryParse(data[1], out int idProduct))
                {
                    int success = 0;
                    OrderCurrentRepo repo = new OrderCurrentRepo(_context);
                    var order = repo.GetConcreteOrderCustomers(callback.From.Id, idProduct);
                    success = await repo.MoveOrderCurrentToOrderHistory(order);

                    if (success > 0)
                    {
                        await _botService.Client.AnswerCallbackQueryAsync(
                                              callbackQueryId: callback.Id,
                                              text: $"Deleted");

                        await _botService.Client.DeleteMessageAsync(callback.Message.Chat.Id, callback.Message.MessageId);
                    }

                }
                else
                {
                    await _botService.Client.AnswerCallbackQueryAsync(
                      callbackQueryId: callback.Id,
                      text: $"Cannot be deleted");
                }
            }
            catch (Exception e)
            {
                await _botService.Client.AnswerCallbackQueryAsync(
                     callbackQueryId: callback.Id,
                     text: $"Cannot be deleted");
            }          
        }
    }
}

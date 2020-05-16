using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using Telegram.Bot.Sales.EF;
using Telegram.Bot.Sales.Repos;
using Telegram.Bot.Sales.Services;
using Telegram.Bot.Types;

namespace Telegram.Bot.Sales.CallBackService.CallBack
{
    public class CallBackEndWaitingProduct : ICallBack
    {
        private readonly IBotService _botService;
        private readonly ILogger<CallBackService> _logger;
        private readonly ApplicationContext _context;
        public CallBackEndWaitingProduct(IBotService botService, ILogger<CallBackService> logger, ApplicationContext context)
        {
            _botService = botService;
            _logger = logger;
            _context = context;
        }

        public string Name => "EndWaiting";

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
                                             text: "Deleted");                     
                    }
                    else
                    {
                        await _botService.Client.AnswerCallbackQueryAsync(
                                               callbackQueryId: callback.Id,
                                               text: "Product has already been removed from the waiting list");
                    }
                }
                else
                {
                    await _botService.Client.AnswerCallbackQueryAsync(
                                             callbackQueryId: callback.Id,
                                             text: "Cannot be removed");
                }
            }
            catch (Exception e)
            {
                await _botService.Client.AnswerCallbackQueryAsync(
                                               callbackQueryId: callback.Id,
                                               text: "Product has already been removed from the waiting list");
                _logger.LogError($"{DateTime.Now} -- {nameof(CallBackEndWaitingProduct)} --  {e.Message}");
            }
        }
    }
}

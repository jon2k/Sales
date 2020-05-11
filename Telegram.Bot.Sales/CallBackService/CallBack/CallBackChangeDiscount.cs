using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Telegram.Bot.Sales.EF;
using Telegram.Bot.Sales.Repos;
using Telegram.Bot.Sales.Services;
using Telegram.Bot.Types;

namespace Telegram.Bot.Sales.CallBackService.CallBack
{
    public class CallBackChangeDiscount : ICallBack
    {
        private readonly IBotService _botService;
        private readonly ILogger<CallBackService> _logger;
        private readonly ApplicationContext _context;
        public CallBackChangeDiscount(IBotService botService, ILogger<CallBackService> logger, ApplicationContext context)
        {
            _botService = botService;
            _logger = logger;
            _context = context;
        }

        public string Name => "ChangeDiscount";

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

                if (int.TryParse(data[1], out int idOrderCurrent))
                {
                    int success = 0;
                    OrderCurrentRepo repo = new OrderCurrentRepo(_context);
                    var res=repo.GetOne(idOrderCurrent);
                    res.ExpectedPercentDiscount = (byte)(res.ExpectedPercentDiscount+5);
                    success= await repo.Change(res);
                   
                    if (success > 0)
                    {
                        string msg = $"Your expected discount is {res.ExpectedPercentDiscount} %";
                        await _botService.Client.AnswerCallbackQueryAsync(
                                              callbackQueryId: callback.Id,
                                              text: msg);
                    }
                }
                else
                {
                    //ToDo logging                                    
                }
            }
            catch (Exception e)
            {
                await _botService.Client.AnswerCallbackQueryAsync(
                                              callbackQueryId: callback.Id,
                                              text: "Product has already been removed from the waiting list");
            }
        }
    }
}

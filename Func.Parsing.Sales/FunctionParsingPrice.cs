using Microsoft.Azure.WebJobs;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;
using Telegram.Bot.Sales.EF;
using Telegram.Bot.Sales.Repos;
using Telegram.Bot.Sales.Parser;
using Telegram.Bot.Sales.Services;
using Telegram.Bot.Types.ReplyMarkups;

namespace Function.ParsingPrice.Sales
{
    public class FunctionParsingPrice
    {
        private readonly ApplicationContext _context;
        private readonly IBotService _botService;

        public FunctionParsingPrice(ApplicationContext context, IBotService botService)
        {
            _context = context;
            _botService = botService;
        }

        [FunctionName("ParsingPrice")]
        public async Task ParsingPrice([TimerTrigger("0 30 6 * * *")]TimerInfo myTimer, ILogger logger)
        {

            var productsOpderCurrent = _context.OrdersCurrent.Include(n => n.Product)
                                            .ThenInclude(n => n.Shop)
                                            .Select(n => n.Product)
                                            .Distinct()
                                            .ToList();

            Parsing parser = new Parsing(_context, logger);
            ProductPriceHistoryRepo repo2 = new ProductPriceHistoryRepo(_context);
            if (productsOpderCurrent != null && productsOpderCurrent.Count != 0)
            {
                var task = productsOpderCurrent.Select(n => parser.ParsingPriceAsync(n));
                var result = await Task.WhenAll(task);

                try
                {
                   // var test = result.Select(n => n.productPriceHistory).ToList();
                    await repo2.AddRange(result.Where(x=>x.msgAlarm==null).Select(n=>n.productPriceHistory).ToList());                 
                }
                catch(Exception e) { logger.LogError(e.Message); }

                var test = result.Where(x => x.msgAlarm != null).ToList();
                if (test!=null)
                {
                    foreach (var item in test)
                    {
                        await _botService.Client.SendTextMessageAsync(866216037, item.msgAlarm, parseMode: Telegram.Bot.Types.Enums.ParseMode.Markdown);
                    }
                }
            }
            else
            {
                logger.LogInformation($"Free waiting product List or no connect to DB {DateTime.Now}");
            }
            logger.LogInformation($"C# Timer trigger function executed at: {DateTime.Now}");
            await CheckPriceAndSendMessageToCustomerAboutLowPrice();
        }


        [FunctionName("CheckPriceAndSendMessageToCustomerAboutLowPrice")]
        public async Task CheckPriceAndSendMessageToCustomerAboutLowPrice()
        {
            var orderCurrent = _context.OrdersCurrent
                .Include(n => n.Customer)
                .Include(n => n.Product)
                .ToList();
            if (orderCurrent != null && orderCurrent.Count != 0)
            {
                ProductPriceHistoryRepo repo = new ProductPriceHistoryRepo(_context);
                foreach (var order in orderCurrent)
                {
                    var startPrice = repo.GetByTime(order.Product, order.TimeStartNotif);
                    var currentPrice = repo.GetLastByTime(order.Product);             
                    byte discount = CalculateDiscount(startPrice.Price, currentPrice.Price);
                    if (discount >= order.ExpectedPercentDiscount)
                    {
                        string msg = $" **The price has been reduced by. {discount}%. " +
                            $"Old price - {startPrice.Price}. " +
                            $"Current price - {currentPrice.Price}.** " +
                            $"{ order.Product.Url} ";
                        string data1 = $"EndWaiting {order.Product.Id}";
                        string data2 = $"ChangeDiscount {order.Id}";
                        var inlineKeyboard = new InlineKeyboardMarkup(new[]
                           {
                                new []
                                {
                                    InlineKeyboardButton.WithCallbackData("Thanks! Delete order", data1),
                                    InlineKeyboardButton.WithCallbackData("I want a discount more!", data2),
                                }
                            });
                        await _botService.Client.SendTextMessageAsync(order.Customer.CodeTelegram, msg, replyMarkup: inlineKeyboard);
                    }
                }
            }
        }

        private byte CalculateDiscount(double oldPrice, double newPrice)
        {
            if (oldPrice > newPrice)
            {
                return (byte)(100 - (newPrice / (oldPrice / 100)));
            }
            else
                return 0;
        }
    }
}
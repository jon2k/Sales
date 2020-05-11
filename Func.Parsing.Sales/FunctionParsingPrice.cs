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
        public async Task ParsingPrice([TimerTrigger("0 30 11 * * *")]TimerInfo myTimer, ILogger logger)
        {
            var productsOpderCurrent = _context.OrdersCurrent.Include(n => n.Product)
                                            .ThenInclude(n => n.Shop)
                                            .Select(n => n.Product)
                                            .Distinct()
                                            .ToList();

            Parsing parser = new Parsing(_context);
            ProductPriceHistoryRepo repo2 = new ProductPriceHistoryRepo(_context);
            if (productsOpderCurrent != null && productsOpderCurrent.Count != 0)
            {
                foreach (var item in productsOpderCurrent)
                {
                    var productPriceHistory = await parser.ParsingPriceAsync(item);
                    if (productPriceHistory.msgAlarm == null)
                    {
                        await repo2.Add(productPriceHistory.productPriceHistory);
                    }
                    else
                    {
                        Console.WriteLine(productPriceHistory.msgAlarm);
                        logger.LogInformation($"{productPriceHistory.msgAlarm} {DateTime.Now}");
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
                    double first = startPrice.Price / 100;
                    double second = currentPrice.Price / first;
                    byte discount = (byte)(100 - second);
                    if (discount>=order.ExpectedPercentDiscount)
                    {
                        string msg=$" **The price has been reduced by. {discount}%. " +
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
    }
}
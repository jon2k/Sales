using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Telegram.Bot.Sales.EF;
using Telegram.Bot.Sales.Models;
using Telegram.Bot.Sales.Orders;
using Telegram.Bot.Sales.Parser;
using Telegram.Bot.Sales.Repos;
using Telegram.Bot.Sales.Services;
using Telegram.Bot.Types;

namespace Telegram.Bot.Sales.CommandService.Commands
{
    public class AddProductCommand : ICommand
    {
        public string Name => @"http";
        private readonly IBotService _botService;
        private readonly ILogger<CommandsService> _logger;
        private readonly ApplicationContext _context;

        public AddProductCommand(IBotService botService, ILogger<CommandsService> logger, ApplicationContext context)
        {
            _botService = botService;
            _logger = logger;
            _context = context;
        }
        public bool Contains(Message message)
        {
            if (message.Type != Telegram.Bot.Types.Enums.MessageType.Text)
                return false;

            return (message.Text.Contains(this.Name) || message.Text.StartsWith(this.Name));
        }

        public async Task Execute(Message message)
        {
            string MessageToCustomer;
            try
            {
                //Get Customer ID
                var chatId = message.Chat.Id;
                // Get customer from DB.
                Customer customer;
                CustomerRepo db = new CustomerRepo(_context);
                customer = db.GetOneByCodTelegram(chatId);
                if (customer != null && !customer.IsDeleted)
                {
                    string[] msg = message.Text.Split();
                    string link = string.Empty;
                    foreach (var item in msg)
                    {
                        if (item.Contains(Name))
                        {
                            link = item;
                            break;
                        }
                    }
                    if (link != string.Empty)
                    {
                        // Get product.
                        Parsing parser = new Parsing(_context);
                        var product = await parser.ParsingProductAsync(link);
                        if (product.msgAlarm == null)
                        {
                            // Get ProductPriceHistory.
                            var productPriceHistory = await parser.ParsingPriceAsync(product.product);

                            if (productPriceHistory.msgAlarm == null)
                            {
                                Product pr;
                                ProductRepo repo = new ProductRepo(_context);
                                // Check same product to db.
                                pr = repo.GetProductByUrl(product.product.Url);
                                if (pr != null)
                                {                                  
                                    product.Item1 = pr;
                                }
                                else
                                {
                                    // Add new Product to DB.
                                    await repo.Add(product.product);
                                    pr = repo.GetProductByUrl(product.product.Url);
                                }
                                productPriceHistory.productPriceHistory.Product = pr;
                                ProductPriceHistoryRepo repo2 = new ProductPriceHistoryRepo(_context);
                                await repo2.Add(productPriceHistory.productPriceHistory);

                                // Notification's only telegram, because this command only for telegramm.

                                // Create Current Order.
                                Order order = new Order();
                                bool exist = false;
                                var currentOrder = order.CreateOrderCurrent(customer, pr, productPriceHistory.productPriceHistory.DateTime, 5);
                                OrderCurrentRepo repo3 = new OrderCurrentRepo(_context);
                                exist = repo3.CheckExistOrder(currentOrder.orderCurrent);
                                if (!exist)
                                {
                                    NotificationRepo repo4 = new NotificationRepo(_context);
                                    await repo4.Add(new Notification()
                                    {
                                        OrderCurrent = currentOrder.orderCurrent,
                                        TypeNotificationId = 1
                                    });

                                    MessageToCustomer = $"*** Your product has been added to the waiting list. ***" +
                                        $"{product.product.Url} ";
                                }
                                else
                                {
                                    MessageToCustomer = "You already added this product to the waiting list";
                                }
                            }
                            else
                            {
                                MessageToCustomer = productPriceHistory.msgAlarm;
                            }
                        }
                        else
                        {
                            MessageToCustomer = product.msgAlarm;
                        }
                    }
                    else
                    {
                        MessageToCustomer = "Invalid command. Please write your link";
                    }
                }
                else
                {
                    MessageToCustomer = "You are not registred!!";
                }
            }
            catch (Exception e)
            {
                MessageToCustomer = e.Message;
            }

            await _botService.Client.SendTextMessageAsync(message.Chat.Id, MessageToCustomer, parseMode: Telegram.Bot.Types.Enums.ParseMode.Markdown);
        }
    }
}

using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Telegram.Bot.Sales.EF;
using Telegram.Bot.Sales.Models;
using Telegram.Bot.Sales.Repos;
using Telegram.Bot.Sales.Services;
using Telegram.Bot.Types;

namespace Telegram.Bot.Sales.CommandService.Commands
{
    public class DeleteCustomerCommand : ICommand
    {
        public string Name => @"/delete";
        private readonly IBotService _botService;
        private readonly ILogger<StartCommand> _logger;
        private readonly ApplicationContext _context;

        public DeleteCustomerCommand(IBotService botService, ILogger<StartCommand> logger, ApplicationContext context)
        {
            _botService = botService;
            _logger = logger;
            _context = context;
        }

        public bool Contains(Message message)
        {
            if (message.Type != Telegram.Bot.Types.Enums.MessageType.Text)
                return false;

            return message.Text.Contains(this.Name) & message.Text.StartsWith(this.Name);
        }

        public async Task Execute(Message message)
        {
            bool _deleted = false;
            string MessageToCustomer;
            Customer delCustomer = new Customer();

            //Get Customer ID
            var chatId = message.Chat.Id;

            //Connect to DB
            try
            {
                CustomerRepo db = new CustomerRepo(_context);
                //check customer data in DB
                var customers = db.GetActualCustomers(chatId);

                if (customers.Count == 0)
                {
                    MessageToCustomer = "You are not registred.";
                }
                else if (customers.Count == 1)
                {
                    if (message.Text.Equals("/delete agree"))
                    {
                        delCustomer = customers.First();
                        delCustomer.IsDeleted = true;
                        await db.Change(delCustomer);
                        //db.DataCustomers.Remove(customers[0]);//delete
                        // await db.SaveChangesAsync();
                        _deleted = true;
                        MessageToCustomer = "Your data has been deleted.";
                    }
                    else
                    {
                        MessageToCustomer = "Are you sure you want to delete your profile? If yes then write: /delete agree";
                    }
                }
                else
                {
                    MessageToCustomer = "Multiple users have the same Telegram code!!! Write in support please.";
                }
                //}
            }
            catch (Exception e)
            {
                MessageToCustomer = "Exception. Write in support please.";
            }
            try
            {
                if (_deleted)
                {
                    OrderCurrentRepo repo = new OrderCurrentRepo(_context);
                    var allOrder = repo.GetAllOrderCurrentByCustomer(delCustomer);
                    await repo.MoveOrdersCurrentToOrdersHistory(allOrder);
                }
            }
            catch (Exception e)
            {
                MessageToCustomer = e.Message;
            }
            //Send message to Customer
            await _botService.Client.SendTextMessageAsync(chatId, MessageToCustomer, parseMode: Telegram.Bot.Types.Enums.ParseMode.Markdown);
        }
    }
}

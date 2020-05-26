using System;
using System.Globalization;
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
    public class CreateCustomerCommand : ICommand
    {
        public string Name => @"/register";
        private readonly IBotService _botService;
        private readonly ILogger<CommandsService> _logger;
        private readonly ApplicationContext _context;

        public CreateCustomerCommand(IBotService botService, ILogger<CommandsService> logger, ApplicationContext context)
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

            string MessageToCustomer;

            //Get Customer ID
            var chatId = message.Chat.Id;

            //Connect to DB
            try
            {
                CustomerRepo db = new CustomerRepo(_context);

                //check customer data in DB
                var customers = db.GetActualCustomers(chatId);

                if (customers.Count != 0 && customers[0].IsDeleted == false)
                    MessageToCustomer = "You have already been registered.";
                else
                {
                    //Check count of words in the message. Must be 4. Example /register Ivan ivanov@gmail.com 01.12.1999
                    string[] words = message.Text.Split(' ');
                    if (words.Length != 4)
                        MessageToCustomer = "To register, write the command as in the example: /register Ivan ivanov@gmail.com 01.12.1999";
                    else
                    {
                        string Email = words[2];
                        // string MiddleName = words[3];
                        string Name = words[1];

                        //Check the date entry according to the pattern
                        try
                        {
                            //If there is no exception, the date matches the pattern
                            CultureInfo provider = CultureInfo.InvariantCulture;
                            DateTime DateOfBirth = DateTime.ParseExact(words[3], "dd.MM.yyyy", provider);

                            //Change old Customer to DB                             
                            var deletedCustomer = db.GetDeletedCustomers(chatId);
                            if (deletedCustomer!=null)
                            {
                                deletedCustomer.DateOfBirth = DateOfBirth;
                                deletedCustomer.Email = Email;
                                deletedCustomer.Name = Name;
                                deletedCustomer.IsDeleted = false;
                                await db.Change(deletedCustomer);
                            }
                            else
                            {
                                // Write new Customer to DB
                                Customer newCustomer = new Customer() { CodeTelegram = chatId, Name = Name, Email = Email, DateOfBirth = DateOfBirth, IsDeleted = false, GenderId = null };//создаем клиента
                                await db.Add(newCustomer);
                            }

                            //Check save data
                            customers = db.GetActualCustomers(chatId);

                            if (customers.Count == 0)
                            {
                                MessageToCustomer = "Failed to register. Try again.";
                            }
                            else if (customers.Count == 1)
                            {
                                MessageToCustomer = "Registration completed successfully!";
                            }
                            else
                            {
                                MessageToCustomer = "Multiple users have the same Telegram code!!! Write in support please.";
                                _logger.LogError($"{DateTime.Now} -- {nameof(CreateCustomerCommand)} --  {MessageToCustomer}");
                            }
                        }
                        catch (Exception ee)
                        {
                            MessageToCustomer = "Date of birth does not match the example! To register, write the command as in the example: /register Ivan ivanov@gmail.com 01.12.1999";
                            // MessageToCustomer = ee.Message;
                        }
                    }
                }
                // }
            }
            catch (Exception e)
            {
                 MessageToCustomer = "Exception. Write in support please.";
                //MessageToCustomer = e.Message;
                _logger.LogError($"{DateTime.Now} -- {nameof(CreateCustomerCommand)} --  {e.Message}");
            }

            //Send message to Customer
            await _botService.Client.SendTextMessageAsync(chatId, MessageToCustomer, parseMode: Telegram.Bot.Types.Enums.ParseMode.Markdown);
        }
    }
}

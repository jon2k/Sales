using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Telegram.Bot.Sales.CommandService.Commands;
using Telegram.Bot.Sales.EF;
using Telegram.Bot.Sales.Services;
using Telegram.Bot.Types;

namespace Telegram.Bot.Sales.CommandService
{
    public class CommandsService: ICommandService
    {
        private List<ICommand> _commands;
        private readonly IBotService _botService;
        private readonly ILogger<CommandsService> _logger;
        private readonly ApplicationContext _context;

        public CommandsService(IBotService botService, ILogger<CommandsService> logger, ApplicationContext context)
        {
            _botService = botService;
            _logger = logger;
            _context = context;
            _commands = new List<ICommand>();
            _commands.Add(new StartCommand(_botService, _logger));
            _commands.Add(new AddProductCommand(_botService, _logger, _context));
            _commands.Add(new CreateCustomerCommand(_botService, _logger, _context));
            _commands.Add(new DeleteCustomerCommand(_botService, _logger, _context));
            _commands.Add(new ShowProductsCommand(_botService, _logger, _context));
            _commands.Add(new ShowShopsCommand(_botService, _logger, _context));
            ///
        }
        public async Task Execute(Message message)
        {
          
            foreach (var command in _commands)
            {
                if (command.Contains(message))
                {
                    await command.Execute(message);
                    break;
                }
            }
        }
      
    }
}

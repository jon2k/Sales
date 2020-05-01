using System.Threading.Tasks;
using Telegram.Bot.Types;

namespace Telegram.Bot.Sales.CommandService.Commands
{
    public interface ICommand
    {
        public string Name { get; }
        bool Contains(Message message);
        Task Execute(Message message);
    }
}

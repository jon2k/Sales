using System.Threading.Tasks;
using Telegram.Bot.Types;

namespace Telegram.Bot.Sales.CommandService
{
    public interface ICommandService
    {
        Task Execute(Message message);
    }
}

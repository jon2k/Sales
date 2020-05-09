using Microsoft.Azure.WebJobs;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using Telegram.Bot.Sales.EF;
using Telegram.Bot.Sales.Services;

namespace Function.Parsing.Sales
{
    public class Function1
    {
        private readonly ApplicationContext _context;
        private readonly IBotService _botService;
        public Function1(ApplicationContext context, IBotService botService)
        {
            _context = context;
            _botService = botService;
        }
        [FunctionName("Function1")]
        public  async Task Run([TimerTrigger("0 * * * * *")]TimerInfo myTimer, ILogger logger)
        {
          
                Console.WriteLine(  "!!!!!!!!!!!!");

            var res = await _context.Customers.ToListAsync();
            foreach (var item in res)
            {
                Console.WriteLine(item.Name);
                await _botService.Client.SendTextMessageAsync(item.CodeTelegram, "Хватит читать ерунду", parseMode: Telegram.Bot.Types.Enums.ParseMode.Markdown);
            }
            logger.LogInformation($"C# Timer trigger function executed at: {DateTime.Now}");



        }

        //[FunctionName("SaveNewPrice")]
        //public static async Task SaveNewPriceToDb(string test)
        //{
        //    Console.WriteLine(test);
        //    Console.WriteLine("Second function executed");
        //    if (true)
        //    {
        //        await SendMessageToCustomerAboutNewPrice("");
        //    }
        //}

        //[FunctionName("SendMessageToCustomerAboutNewPrice")]
        //public static async Task SendMessageToCustomerAboutNewPrice(string test)
        //{
        //    Console.WriteLine(test);
        //    Console.WriteLine("Third function executed");
        //}


    }
}

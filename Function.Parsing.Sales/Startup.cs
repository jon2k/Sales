using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
//using System.Configuration;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Sales;
using Telegram.Bot.Sales.EF;
using Telegram.Bot.Sales.Services;

[assembly: FunctionsStartup(typeof(Function.Parsing.Sales.Startup))]
namespace Function.Parsing.Sales
{
    public class Startup : FunctionsStartup
    {
       
        
            Configuration = configuration;
        
        public IConfiguration Configuration { get; }
       
        public override void Configure(IFunctionsHostBuilder builder)
        {

          
            string SqlConnection = Environment.GetEnvironmentVariable("SqlConnectionString");
            // builder.Services.AddLogging();
            builder.Services.AddSingleton<IBotService, BotService>();
            builder.Services.Configure<BotConfiguration>(Configuration.GetSection("BotConfiguration"));
            builder.Services.AddDbContext<ApplicationContext>(
                            options => options.UseSqlServer(SqlConnection));
            //builder.Services.AddSingleton<ILoggerProvider, MyLoggerProvider>();
        }
    }
}

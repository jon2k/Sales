using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using Telegram.Bot.Sales;
using Telegram.Bot.Sales.EF;
using Telegram.Bot.Sales.Services;

[assembly: FunctionsStartup(typeof(Function.ParsingPrice.Sales.Startup))]
namespace Function.ParsingPrice.Sales
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            string SqlConnection = Environment.GetEnvironmentVariable("SqlConnectionString");
           // builder.Services.AddApplicationInsightsTelemetry();
            builder.Services.AddSingleton<IBotService, BotService>();
            builder.Services.AddOptions<BotConfiguration>()
                .Configure<IConfiguration>((settings, configuration) =>
                {
                    configuration.GetSection("BotConfiguration").Bind(settings);
                });

            builder.Services.AddDbContext<ApplicationContext>(
                            options => options.UseSqlServer(SqlConnection));
        }
    }
}

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Telegram.Bot.Sales.CallBackService;
using Telegram.Bot.Sales.CommandService;
using Telegram.Bot.Sales.EF;
using Telegram.Bot.Sales.SelectService;
using Telegram.Bot.Sales.Services;

namespace Telegram.Bot.Sales
{
    public interface ITest
    { }
    public class Test:ITest
    { }
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddScoped<IUpdateService, UpdateService>();
            services.AddScoped<ICommandService, CommandsService>();
            services.AddScoped<ISelectService, SelectService.SelectService>();
            services.AddScoped<ICallBackService, CallBackService.CallBackService>();
            services.AddSingleton<IBotService, BotService>();
            services.Configure<BotConfiguration>(Configuration.GetSection("BotConfiguration"));
            services.AddDbContext<ApplicationContext>(options=>
            options.UseSqlServer(Configuration.GetConnectionString("SalesDatabase")));

            services
                .AddControllers()
                .AddNewtonsoftJson();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();
            app.UseCors();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}

using Microsoft.ApplicationInsights.AspNetCore.Extensions;
using Microsoft.ApplicationInsights.AspNetCore.TelemetryInitializers;
using Microsoft.ApplicationInsights.DependencyCollector;
using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Linq;
using Telegram.Bot.Sales.CallBackService;
using Telegram.Bot.Sales.CommandService;
using Telegram.Bot.Sales.EF;
using Telegram.Bot.Sales.SelectService;
using Telegram.Bot.Sales.Services;

namespace Telegram.Bot.Sales
{
    public class Startup
    {
      //  private readonly ILogger _logger;
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
           // _logger = logger;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            // Settings ApplicationInsights //
            // This link has description about aiOptions
            //https://github.com/microsoft/ApplicationInsights-dotnet/blob/develop/NETCORE/src/Shared/Extensions/ApplicationInsightsServiceOptions.cs
           /* ApplicationInsightsServiceOptions aiOptions
                = new ApplicationInsightsServiceOptions();
            aiOptions.EnableAdaptiveSampling = false;
            aiOptions.EnableQuickPulseMetricStream = false;
            aiOptions.EnablePerformanceCounterCollectionModule = false;
            aiOptions.EnableRequestTrackingTelemetryModule = false;
            aiOptions.EnableDebugLogger = false;
            aiOptions.EnableHeartbeat = false;
            aiOptions.AddAutoCollectedMetricExtractor = false;
            aiOptions.EnableRequestTrackingTelemetryModule = false;
            aiOptions.EnableEventCounterCollectionModule = false;
            aiOptions.EnableDependencyTrackingTelemetryModule = false;
            aiOptions.EnableAzureInstanceMetadataTelemetryModule = false;
            aiOptions.EnableAppServicesHeartbeatTelemetryModule = false;*/

           // services.AddApplicationInsightsTelemetry(aiOptions);
            services.AddScoped<ICommandService, CommandsService>();
            services.AddScoped<ISelectService, SelectService.SelectService>();
            services.AddScoped<ICallBackService, CallBackService.CallBackService>();
            services.AddSingleton<IBotService, BotService>();
            services.Configure<BotConfiguration>(Configuration.GetSection("BotConfiguration"));
            services.AddDbContext<ApplicationContext>(options =>
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

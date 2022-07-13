using System.IO;
using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Vertical.Product.Service.Functions.Logging;

[assembly: FunctionsStartup(typeof(Product.Functions.Startup))]
namespace Product.Functions
{
    internal class Startup : FunctionsStartup
    {
        private IConfigurationRoot _configurations;
        public override void ConfigureAppConfiguration(IFunctionsConfigurationBuilder builder)
        {
            var environmentName = builder.GetContext().EnvironmentName;
            _configurations = new ConfigurationBuilder()
                        .SetBasePath(Directory.GetCurrentDirectory())
                         .AddJsonFile($"appsettings.{environmentName}.json", optional: true, reloadOnChange: true)
                         .AddEnvironmentVariables()
                         .Build();

            var config = builder.GetContext().Configuration;

            var instrumentationKey = config.GetValue<string>("APPINSIGHTS_INSTRUMENTATIONKEY");

            //var logger = new LoggerConfiguration()
            //    .MinimumLevel.Debug()
            //    .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
            //    .Enrich.FromLogContext()
            //    .WriteTo.Console()
            //   // .WriteTo.AzureApplicationInsights(instrumentationKey)
            //    .CreateLogger();

            //builder.Services.AddLogging(loggingBuilder =>
            //{
            //    loggingBuilder.AddSerilog(logger);
            //});

            //var x = _configurations.GetSection(ApplicationInsightsOptions.ApplicationInsights).Get<ApplicationInsightsOptions>();

            //var basePath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "..");
            //var environmentName = builder.GetContext().EnvironmentName;
            //builder.ConfigurationBuilder
            //    .SetBasePath(basePath)
            //    .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
            //    .AddJsonFile($"appsettings.{environmentName}.json", optional: true, reloadOnChange: true)
            //    .AddEnvironmentVariables();
        }

        public override void Configure(IFunctionsHostBuilder builder)
        {
            builder.Services.AddSingleton<ITelemetryInitializer, CloudRoleTelemetryInitializer>();
        }
    }
}

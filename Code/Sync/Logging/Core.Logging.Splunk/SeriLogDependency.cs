using Serilog;
using Serilog.Events;

namespace Core.Logging.Serilog
{
    public static class SeriLogDependency
    {
        public static void Register()
        {

            Log.Logger = new LoggerConfiguration();
            //.MinimumLevel.Override("Microsoft", LogEventLevel.Information)
            //.Enrich.FromLogContext()
            //.WriteTo.EventCollector("https://mysplunk:8088/services/collector", "myeventcollectortoken");
            //.WriteTo.Console()
            //.WriteTo.File("logs/myapp.txt", rollingInterval: RollingInterval.Day)
            ////.WriteTo.ApplicationInsights(serviceProvider.GetRequiredService<TelemetryConfiguration>(), TelemetryConverter.Traces)
            //.CreateLogger();

            // builder.Host.UseSerilog();

            //Log.Logger = new LoggerConfiguration()
            //    .MinimumLevel.Debug()
            //    .WriteTo.Console()
            //    .WriteTo.File("logs/myapp.txt", rollingInterval: RollingInterval.Day)
            //    .CreateLogger();

            //Log.Information("Hello, world!");

            //int a = 10, b = 0;
            //try
            //{
            //    Log.Debug("Dividing {A} by {B}", a, b);
            //    Console.WriteLine(a / b);
            //}
            //catch (Exception ex)
            //{
            //    Log.Error(ex, "Something went wrong");
            //}
            //finally
            //{
            //    //Log.CloseAndFlush();
            //}
        }

       
    }
}

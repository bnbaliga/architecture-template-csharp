using Microsoft.ApplicationInsights.AspNetCore.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serilog;

namespace Core.Logging.Serilog
{
    public static class SeriLogDependency
    {
        public static void Register(WebApplicationBuilder builder)
        {
            //GOOGLE_APPLICATION_CREDENTIALS
            var logger = new LoggerConfiguration()
                .ReadFrom.Configuration(builder.Configuration)
                .CreateLogger();

            builder.Logging.ClearProviders();
            builder.Logging.AddSerilog(logger);

            var applicationInsightsOptions = builder.Configuration.GetSection(ApplicationInsightsOptions.ApplicationInsights).Get<ApplicationInsightsOptions>();
            if (applicationInsightsOptions != null)
            {
                var aiOptions = new ApplicationInsightsServiceOptions
                {
                    ConnectionString = applicationInsightsOptions.ConnectionString,
                    EnableAdaptiveSampling = applicationInsightsOptions.EnableAdaptiveSampling,
                    EnableQuickPulseMetricStream = applicationInsightsOptions.EnableQuickPulseMetricStream
                };
                builder.Services.AddApplicationInsightsTelemetry(aiOptions);
            }
        }
    }
}

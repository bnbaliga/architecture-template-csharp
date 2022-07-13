namespace Core.Logging.Serilog
{
    public class ApplicationInsightsOptions
    {
        public const string ApplicationInsights = "ApplicationInsights";

        public string ConnectionString { get; set; } = String.Empty;
        public bool EnableAdaptiveSampling { get; set; }
        public bool EnableQuickPulseMetricStream { get; set; }
    }
}

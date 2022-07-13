using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Microsoft.Extensions.Logging;

namespace Vertical.Product.Service.Functions.Functions.Activities
{
    public class HelloWorldActivity
    {
        [FunctionName(nameof(HelloWorldActivity))]
        public string Run(
            [ActivityTrigger] 
        IDurableActivityContext activityContext,
            //string name,
            ILogger logger)
        {
            var name = activityContext.GetInput<string>();
            logger.Log(LogLevel.Information, $"Triggered {nameof(HelloWorldActivity)} - instance {activityContext.InstanceId}");

            return $"Hello {name}!";
        }
    }
}

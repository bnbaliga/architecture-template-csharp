using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Vertical.Product.Service.Functions.Functions.Activities;

namespace Vertical.Product.Service.Functions.Functions.Orchestrations
{
    public class HelloWorldOrchestrator
    {
        [FunctionName(nameof(HelloWorldOrchestrator))]
        public async Task<List<string>> RunOrchestrator(
            [OrchestrationTrigger] IDurableOrchestrationContext context)
        {
            return new List<string>
            {
                await context.CallActivityAsync<string>(nameof(HelloWorldActivity), "Tokyo"),
                await context.CallActivityAsync<string>(nameof(HelloWorldActivity), "Seattle"),
                await context.CallActivityAsync<string>(nameof(HelloWorldActivity), "London")
            };
        }
    }
}
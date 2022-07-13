using Fairway.Core.Proxy.Http.Src;
using Microsoft.Extensions.DependencyInjection;
using Polly.Retry;
using System;
using System.Net.Http;

namespace Fairway.Core.Proxy.Http.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static void AddHttpProxy(this IServiceCollection services)
        {
            ProxyDependency.AddHttpProxy(services);
        }

        public static void AddHttpProxy(this IServiceCollection services, string name, int maxConnectionPerServer = int.MaxValue)
        {
            ProxyDependency.AddHttpProxy(services, name, maxConnectionPerServer);
        }      

        public static void AddHttpProxy(this IServiceCollection services, string name, 
            Func<IServiceProvider, HttpRequestMessage, AsyncRetryPolicy<HttpResponseMessage>> policySelector, int maxConnectionPerServer = int.MaxValue)
        {
            ProxyDependency.AddHttpProxy(services, name, policySelector, maxConnectionPerServer);
        }

        public static void AddHttpProxy(this IServiceCollection services, string name, AsyncRetryPolicy<HttpResponseMessage> policy, int maxConnectionPerServer = int.MaxValue)
        {
            ProxyDependency.AddHttpProxy(services, name, policy, maxConnectionPerServer);
        }

        public static void AddHttpProxy(this IServiceCollection services, string name, AsyncRetryPolicy<HttpResponseMessage>[] policies, int maxConnectionPerServer = int.MaxValue)
        { 
            ProxyDependency.AddHttpProxy(services, name, policies, maxConnectionPerServer);
        }
    }
}

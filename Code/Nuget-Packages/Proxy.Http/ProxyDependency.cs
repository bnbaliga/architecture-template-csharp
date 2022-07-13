using Microsoft.Extensions.DependencyInjection;
using Polly.Retry;
using System;
using System.Net.Http;

namespace Fairway.Core.Proxy.Http.Src
{
    internal static class ProxyDependency
    {
        internal static void AddHttpProxy(IServiceCollection services)
        {
            services.AddHttpClient();
        }

        internal static void AddHttpProxy(IServiceCollection services, string name, int maxConnectionPerServer = int.MaxValue)
        {
            services.AddAndGetDefaultHttpClient(name, maxConnectionPerServer);
        }

        internal static void AddHttpProxy(IServiceCollection services, string name,
            Func<IServiceProvider, HttpRequestMessage, AsyncRetryPolicy<HttpResponseMessage>> policySelector, int maxConnectionPerServer = int.MaxValue)
        {
            services.AddAndGetDefaultHttpClient(name, maxConnectionPerServer)
                    .AddPolicyHandler(policySelector);
        }

        internal static void AddHttpProxy(IServiceCollection services, string name, AsyncRetryPolicy<HttpResponseMessage> policy, int maxConnectionPerServer = int.MaxValue)
        {
            services.AddAndGetDefaultHttpClient(name, maxConnectionPerServer)
                    .AddPolicyHandler(policy);
        }

        internal static void AddHttpProxy(IServiceCollection services, string name, AsyncRetryPolicy<HttpResponseMessage>[] policies, int maxConnectionPerServer = int.MaxValue)
        {
            var httpClient = services.AddAndGetDefaultHttpClient(name, maxConnectionPerServer);

            foreach (var policy in policies)
                httpClient.AddPolicyHandler(policy);
        }

        private static IHttpClientBuilder AddAndGetDefaultHttpClient(this IServiceCollection services, string name, int maxConnectionPerServer = int.MaxValue)
        {
            return services.AddHttpClient(name)
                    .ConfigurePrimaryHttpMessageHandler(() =>
                    {
                        return new HttpClientHandler()
                        {
                            MaxConnectionsPerServer = maxConnectionPerServer
                        };
                    })
            .ConfigureHttpClient(client => { client.DefaultRequestHeaders.Add(HttpProxy.HttpHeaders.Connection, HttpProxy.HttpHeaders.KeepAlive); });
        }
    }
}

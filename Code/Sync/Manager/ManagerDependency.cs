using Microsoft.Extensions.DependencyInjection;
using MediatR;
using Vertical.Product.Service.Data.Dependency;
using Vertical.Product.Service.Contract.Playground;
using Microsoft.Extensions.Configuration;
using System.Reflection;

namespace Vertical.Product.Service.Manager
{
    public static class ManagerDependency
    {
        public static void Register(IServiceCollection sericeCollection, IConfiguration configuration)
        {
            sericeCollection.AddScoped<BankAccountRequest>();
            sericeCollection.AddMediatR(typeof(ManagerDependency).Assembly);
            sericeCollection.AddDataContext(configuration);
        }

        public static void RegisterAllTypes<T>(this IServiceCollection services, Assembly[] assemblies,
                ServiceLifetime lifetime = ServiceLifetime.Transient)
        {
            var typesFromAssemblies = assemblies.SelectMany(a => a.DefinedTypes.Where(x => x.GetInterfaces().Contains(typeof(T))));
            foreach (var type in typesFromAssemblies)
                services.Add(new ServiceDescriptor(typeof(T), type, lifetime));
        }
    }
}

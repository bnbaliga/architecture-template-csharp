using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Vertical.Product.Service.Contract.AppSettings;

namespace Vertical.Product.Service.Data.Dependency
{
    public static class DataDependency
    {
        public static void AddDataContext(this IServiceCollection sericeCollection, IConfiguration configuration)
        {
            sericeCollection.AddDbContext<AdventureWorksLT2019Context>(options => options.UseSqlServer(configuration[DataConfiguration.DBConnectionString]));
        }
    }
}

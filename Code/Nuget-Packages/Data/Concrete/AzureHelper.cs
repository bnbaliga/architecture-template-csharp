using Microsoft.Azure.Services.AppAuthentication;
using System.Threading.Tasks;

namespace Fairway.Core.Data.Sql.EF.Concrete
{
    public static class AzureHelper
    {
        public static async Task<string> GetAccessToken()
        {
            var tokenProvider = new AzureServiceTokenProvider();
            var accessToken = await tokenProvider.GetAccessTokenAsync("https://database.windows.net/").ConfigureAwait(false);
            return accessToken;
        }
    }
}

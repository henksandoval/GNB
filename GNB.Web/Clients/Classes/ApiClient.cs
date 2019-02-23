using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace GNB.Web.Clients
{
    public class ApiClient : IApiClient
    {

        private readonly HttpClient HttpClient;

        public ApiClient(HttpClient httpClient)
        {
            HttpClient = httpClient;
        }

        public async Task<string> GetStringTransactions()
        {
            try
            {
                var data = await HttpClient.GetStringAsync("transaction");
                return data;
            }
            catch (Exception e)
            {
                throw new Exception(e.InnerException?.Message, e.InnerException);
            }
        }
    }
}

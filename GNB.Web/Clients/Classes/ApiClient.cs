using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace GNB.Web.Clients.Classes
{
    public class ApiClient : IApiClient
    {

        private readonly HttpClient HttpClient;

        public ApiClient(HttpClient httpClient)
        {
            HttpClient = httpClient;
        }

        public Task<string> GetStringTransactions()
        {
            try
            {
                return HttpClient.GetStringAsync("transaction");
            }
            catch (Exception e)
            {
                throw new Exception(e.InnerException?.Message, e.InnerException);
            }
        }
    }
}

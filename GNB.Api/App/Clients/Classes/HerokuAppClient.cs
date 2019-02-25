using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

namespace GNB.Api.App.Clients
{
    public class HerokuAppClient : IHerokuAppClient
    {
        private readonly HttpClient HttpClient;
        private const string REQUEST_URI_RATES = "/rates.json";
        private const string REQUEST_URI_TRANSACTIONS = "/transactions.json";

        public HerokuAppClient(HttpClient httpClient)
        {
            HttpClient = httpClient;
        }

        public Task<string> GetStringRates() => HttpClient.GetStringAsync(REQUEST_URI_RATES);

        public Task<string> GetStringTransactions()
        {
            try
            {
                return HttpClient.GetStringAsync(REQUEST_URI_TRANSACTIONS);
            }
            catch (Exception e)
            {
                throw new Exception(e.InnerException?.Message, e.InnerException);
            }
        }
    }
}

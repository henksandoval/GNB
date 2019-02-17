using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

namespace GNB.Api.Clients
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

        public Task<Stream> GetStreamRates() => HttpClient.GetStreamAsync(REQUEST_URI_RATES);

        public Task<Stream> GetStreamTransactions() => HttpClient.GetStreamAsync(REQUEST_URI_TRANSACTIONS);
    }
}

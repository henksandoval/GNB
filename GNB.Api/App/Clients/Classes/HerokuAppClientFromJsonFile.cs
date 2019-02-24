using System;
using System.IO;
using System.Threading.Tasks;

namespace GNB.Api.App.Clients
{
    public class HerokuAppClientFromJsonFile : IHerokuAppClient
    {
        private const string REQUEST_URI_RATES = "rates.json";
        private const string REQUEST_URI_TRANSACTIONS = "transactions.json";

        public async Task<string> GetStringRates()
        {
            try
            {
                string data = await ReadJsonFile(REQUEST_URI_RATES);
                return data;
            }
            catch (Exception e)
            {
                throw new Exception(e.InnerException?.Message, e.InnerException);
            }
        }

        public async Task<string> GetStringTransactions()
        {
            try
            {
                string data = await ReadJsonFile(REQUEST_URI_TRANSACTIONS);
                return data;
            }
            catch (Exception e)
            {
                throw new Exception(e.InnerException?.Message, e.InnerException);
            }
        }

        private async Task<string> ReadJsonFile(string path)
        {
            string json;

            using (StreamReader reader = new StreamReader(path))
            {
                json = await reader.ReadToEndAsync();
            }

            return json;
        }
    }
}

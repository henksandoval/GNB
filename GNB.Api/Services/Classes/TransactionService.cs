using GNB.Api.Clients;
using GNB.Api.Utilities;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace GNB.Api.Services
{
    public class TransactionService<T> : ITransactionService<T> where T : class
    {
        private readonly IHerokuAppClient HerokuAppClient;
        private readonly IStreamUtility StreamUtility;

        public TransactionService(IHerokuAppClient herokuAppClient, IStreamUtility streamUtility)
        {
            HerokuAppClient = herokuAppClient;
            StreamUtility = streamUtility;
        }

        public async Task<IEnumerable<T>> GetTransactions()
        {
            string Transactions = await GetTransactionsOfClient();
            Stream stream = await StreamUtility.ConvertStringToStream(Transactions);
            return await ConvertStreamUtility<T>.ConvertStreamToModel(stream);
        }

        private async Task<string> GetTransactionsOfClient() => await HerokuAppClient.GetStringTransactions();
    }
}

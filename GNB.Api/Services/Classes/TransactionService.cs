using GNB.Api.Clients;
using GNB.Api.Utilities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace GNB.Api.Services
{
    public class TransactionService<T> : ITransactionService<T> where T : class
    {
        private readonly IHerokuAppClient herokuAppClient;
        private readonly IStreamUtility<T> streamUtility;

        public IStreamUtility<T> Stream { get; private set; }

        public TransactionService(IHerokuAppClient herokuAppClient, IStreamUtility<T> streamUtility)
        {
            this.herokuAppClient = herokuAppClient;
            this.streamUtility = streamUtility;
        }

        public async Task<IEnumerable<T>> TryGetTransactions() => await GetTransactions();

        public async Task<IEnumerable<T>> TryGetTransactions(Func<T, bool> predicate)
        {
            IEnumerable<T> data = await GetTransactions();
            return data.Where(predicate);
        }

        private async Task<IEnumerable<T>> GetTransactions()
        {
            string transactions = await GetTransactionsOfClient();
            Stream stream = await streamUtility.ConvertStringToStream(transactions);
            return await StreamUtility<T>.ConvertStreamToModel(stream);
        }

        private async Task<string> GetTransactionsOfClient() => await herokuAppClient.GetStringTransactions();
    }
}

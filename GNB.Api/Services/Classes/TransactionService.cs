using GNB.Api.Clients;
using GNB.Api.Utilities;
using Newtonsoft.Json;
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

        public TransactionService(IHerokuAppClient herokuAppClient, IStreamUtility<T> streamUtility)
        {
            this.herokuAppClient = herokuAppClient;
            this.streamUtility = streamUtility;
        }

        public async Task<IEnumerable<T>> TryGetTransactions()
        {
            try
            {
                return await GetTransactions();
            }
            catch (Exception e)
            {
                throw;
            }
        }

        public async Task<IEnumerable<T>> TryGetTransactions(Func<T, bool> predicate)
        {
            IEnumerable<T> data = await GetTransactions();
            return data.Where(predicate);
        }

        private async Task<IEnumerable<T>> GetTransactions()
        {
            try
            {
                return JsonConvert.DeserializeObject<IEnumerable<T>>(await herokuAppClient.GetStringTransactions());
            }
            catch (Exception e)
            {
                throw;
            }
        }
    }
}

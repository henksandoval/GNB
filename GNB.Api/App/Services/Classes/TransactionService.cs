using GNB.Api.App.Clients;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GNB.Api.App.Services
{
    public class TransactionService<T> : ITransactionService<T> where T : class
    {
        private readonly IHerokuAppClient herokuAppClient;
        private static IEnumerable<T> data;

        public TransactionService(IHerokuAppClient herokuAppClient)
        {
            this.herokuAppClient = herokuAppClient;
        }

        public async Task<IEnumerable<T>> TryGetTransactions()
        {
            try
            {
                return await GetTransactions();
            }
            catch (Exception)
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
                if (data == null)
                {
                    data = JsonConvert.DeserializeObject<IEnumerable<T>>(await herokuAppClient.GetStringTransactions());
                }
                return data;
                //return JsonConvert.DeserializeObject<IEnumerable<T>>(await herokuAppClient.GetStringTransactions());
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}

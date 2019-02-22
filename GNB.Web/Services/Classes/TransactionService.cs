using GNB.Web.Clients;
using GNB.Web.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GNB.Web.Services
{
    public class TransactionService : ITransactionService
    {
        private readonly IApiClient apiClient;

        public TransactionService(IApiClient apiClient)
        {
            this.apiClient = apiClient;
        }

        public async Task<IEnumerable<TransactionModel>> GetAllTransactions(Func<TransactionModel, bool> predicate = null)
        {
            var tempData = new List<TransactionModel>
            {
                new TransactionModel { }
            };

            await apiClient.GetStringTransactions();
            return tempData;
        }
    }
}

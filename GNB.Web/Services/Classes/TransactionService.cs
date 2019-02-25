using GNB.Web.Clients;
using GNB.Web.Models;
using Newtonsoft.Json;
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

        public async Task<IEnumerable<TransactionModel>> GetAllTransactions(TransactionModel model = null)
        {
            string data = await apiClient.GetStringTransactions(model);
            return JsonConvert.DeserializeObject<IEnumerable<TransactionModel>>(data);
        }
    }
}

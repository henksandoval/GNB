using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GNB.Web.Clients;
using GNB.Web.Models;

namespace GNB.Web.Services
{
    public class TransactionService : ITransactionService
    {
        private readonly IApiClient apiClient;

        public TransactionService(IApiClient apiClient)
        {
            this.apiClient = apiClient;
        }

        Task<IEnumerable<TransactionModel>> ITransactionService.GetAllTransactions() => throw new NotImplementedException();
    }
}

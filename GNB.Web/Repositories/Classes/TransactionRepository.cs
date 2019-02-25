using GNB.Web.Models;
using GNB.Web.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GNB.Web.Repositories
{
    public class TransactionRepository : ITransactionRepository
    {
        private readonly ITransactionService transactionService;

        public TransactionRepository(ITransactionService transactionService)
        {
            this.transactionService = transactionService;
        }

        public async Task<IEnumerable<TransactionModel>> TryGetAllTransactions(TransactionModel model = null)
        {
            return await transactionService.GetAllTransactions(model);
        }
    }
}

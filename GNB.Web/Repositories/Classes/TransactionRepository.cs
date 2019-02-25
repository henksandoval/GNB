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

        public async Task<IEnumerable<TransactionModel>> TryGetAllTransactions(Func<TransactionModel, bool> predicate = null)
        {
            return await transactionService.GetAllTransactions();
            //return (await transactionService.GetAllTransactions()).Where(predicate);
            //IEnumerable<TransactionModel> data = (await transactionService.GetAllTransactions()).Where(predicate);
            //return data.Where(predicate);
        }
    }
}

using GNB.Web.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GNB.Web.Repositories
{
    public class TransactionRepository : ITransactionRepository
    {
        public Task<IEnumerable<TransactionModel>> GetAllTransactions() => throw new NotImplementedException();

        public Task<IEnumerable<TransactionModel>> GetAllTransactions(Func<TransactionModel> predicate) => throw new NotImplementedException();
    }
}

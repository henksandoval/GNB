using GNB.Web.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GNB.Web.Repositories
{
    public interface ITransactionRepository
    {
        Task<IEnumerable<TransactionModel>> TryGetAllTransactions(TransactionModel model = null);
    }
}

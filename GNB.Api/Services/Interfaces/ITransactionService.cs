using System.Collections.Generic;
using System.Threading.Tasks;
using GNB.Api.Models;

namespace GNB.Api.Services
{
    public interface ITransactionService
    {
        Task<IEnumerable<TransactionModel>> GetTransactions();
    }
}
using GNB.Web.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GNB.Web.Services
{
    public interface ITransactionService
    {
        Task<IEnumerable<TransactionModel>> GetAllTransactions();
    }
}

using System.Collections.Generic;
using System.Threading.Tasks;
using GNB.Api.Models;
using GNB.Api.ViewModels;

namespace GNB.Api.Business
{
    public interface ITransactionBusiness
    {
        Task<IEnumerable<TransactionModel>> GetAllTransactions();
        Task<IEnumerable<TransactionViewModel>> GetAllTransactionsWithNewCurrency(string newCurrency);
    }
}
using System.Collections.Generic;
using System.Threading.Tasks;
using GNB.Api.App.Models;
using GNB.Api.App.ViewModels;

namespace GNB.Api.App.Business
{
    public interface ITransactionBusiness
    {
        Task<IEnumerable<TransactionViewModel>> GetAllTransactions();
        Task<IEnumerable<TransactionViewModel>> GetAllTransactionsWithNewCurrency(string newCurrency);
    }
}
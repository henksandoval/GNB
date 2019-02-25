using GNB.Api.App.Models;
using GNB.Api.App.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GNB.Api.App.Business
{
    public interface ITransactionBusiness
    {
        Task<IEnumerable<TransactionViewModel>> GetAllTransactions();
        Task<IEnumerable<TransactionViewModel>> GetAllTransactions(TransactionViewModel model);
    }
}
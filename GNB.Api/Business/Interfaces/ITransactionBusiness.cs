using System.Collections.Generic;
using System.Threading.Tasks;
using GNB.Api.Models;
using GNB.Api.ViewModels;

namespace GNB.Api.Business
{
    public interface ITransactionBusiness
    {
        Task<IEnumerable<TransactionViewModel>> GetAllTransactions();
        Task GetTransactionsBySkuCode(TransactionModel transactionModel);
    }
}
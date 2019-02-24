using System.Threading.Tasks;
using GNB.Api.App.ViewModels;

namespace GNB.Api.App.Utilities
{
    public interface ICurrencyConverter
    {
        Task<TransactionViewModel> ApplyConversion(TransactionViewModel transaction);
    }
}
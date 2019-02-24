using System.Threading.Tasks;
using GNB.Api.ViewModels;

namespace GNB.Api.Utilities
{
    public interface ICurrencyConverter
    {
        Task<TransactionViewModel> ApplyConversion(TransactionViewModel transaction);
    }
}
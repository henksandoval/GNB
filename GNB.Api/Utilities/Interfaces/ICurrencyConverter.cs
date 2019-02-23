using System.Threading.Tasks;
using GNB.Api.ViewModels;

namespace GNB.Api.Utilities
{
    public interface ICurrencyConverter
    {
        decimal AmountConverted { get; }
        decimal CurrentAmount { get; set; }
        string CurrentCurrency { get; set; }
        string RequiredCurrency { get; set; }

        Task ApplyConversion(CurrencyViewModel currency, string requiredCurrency);
    }
}
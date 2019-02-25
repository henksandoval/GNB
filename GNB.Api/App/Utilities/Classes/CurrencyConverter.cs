using GNB.Api.App.Models;
using GNB.Api.App.Services;
using GNB.Api.App.ViewModels;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GNB.Api.App.Utilities
{
    public class CurrencyConverter : ICurrencyConverter
    {
        private readonly IRateService<RateModel> service;
        private readonly IDuplicateRatesCleaner duplicateRatesCleaner;
        private decimal currentAmount;
        private string currentCurrency;
        private IEnumerable<RateModel> rates;
        private IList<RateModel> ratesUsed;
        private const int DECIMAL_PLACES = 2;

        public CurrencyConverter(IRateService<RateModel> service, IDuplicateRatesCleaner duplicateRatesCleaner)
        {
            this.service = service;
            this.duplicateRatesCleaner = duplicateRatesCleaner;
        }

        public async Task<TransactionViewModel> ApplyConversion(TransactionViewModel transaction) =>
            Equals(transaction.Currency, transaction.CurrencyConverted)
                ? AssignSameAmountToTransaction(transaction)
                : await CalculateNewAmountToTransaction(transaction);

        private TransactionViewModel AssignSameAmountToTransaction(TransactionViewModel transaction) =>
            new TransactionViewModel
            {
                Sku = transaction.Sku,
                Currency = transaction.Currency,
                Amount = transaction.Amount,
                CurrencyConverted = transaction.CurrencyConverted,
                AmountConverted = transaction.Amount
            };

        private async Task<TransactionViewModel> CalculateNewAmountToTransaction(TransactionViewModel transaction)
        {
            await CalculateNewAmount(transaction);

            return new TransactionViewModel
            {
                Sku = transaction.Sku,
                Currency = transaction.Currency,
                Amount = transaction.Amount,
                CurrencyConverted = transaction.CurrencyConverted,
                AmountConverted = currentAmount
            };
        }

        private async Task CalculateNewAmount(TransactionViewModel transaction)
        {
            currentAmount = transaction.Amount;
            currentCurrency = transaction.Currency;

            rates = await GetRates();

            bool pendingConverter = true;
            ratesUsed = new List<RateModel>();

            while (pendingConverter)
            {
                if (TryConversionFromTo(transaction))
                    break;
                if (TryConversionToFrom(transaction))
                    break;

                TryConversionAppliyingCascade();
                pendingConverter = !Equals(currentCurrency, transaction.CurrencyConverted);
            }
        }

        private bool TryConversionFromTo(TransactionViewModel transaction)
        {
            bool processedConversion = false;

            if (rates.Any(x => x.From == currentCurrency && x.To == transaction.CurrencyConverted))
            {
                RateModel rate = rates.Single(x => x.From == currentCurrency && x.To == transaction.CurrencyConverted);
                currentAmount = ConvertAmountFromTo(currentAmount, rate);
                currentCurrency = rate.To;
                return true;
            }
            return processedConversion;
        }

        private bool TryConversionToFrom(TransactionViewModel transaction)
        {
            bool processedConversion = false;

            if (rates.Any(x => x.From == transaction.CurrencyConverted && x.To == currentCurrency))
            {
                RateModel rate = rates.Single(x => x.From == transaction.CurrencyConverted && x.To == currentCurrency);
                currentAmount = ConvertAmountToFrom(currentAmount, rate);
                currentCurrency = rate.From;
                processedConversion = true;
            }
            return processedConversion;
        }

        private void TryConversionAppliyingCascade()
        {
            if (rates.Any(x => x.From == currentCurrency) || rates.Any(x => x.To == currentCurrency))
            {
                if (rates.Except(ratesUsed).Any(x => x.From == currentCurrency))
                {
                    RateModel rate = rates.First(x => x.From == currentCurrency);
                    currentAmount = ConvertAmountFromTo(currentAmount, rate);
                    currentCurrency = rate.To;
                    ratesUsed.Add(rate);
                }
                else if (rates.Except(ratesUsed).Any(x => x.To == currentCurrency))
                {
                    RateModel rate = rates.First(x => x.To == currentCurrency);
                    currentAmount = ConvertAmountToFrom(currentAmount, rate);
                    currentCurrency = rate.From;
                    ratesUsed.Add(rate);
                }
            }
            else
            {
                //throw new ApplicationException($"The type currency conversion not is posible", new Exception($"The ${transaction.CurrencyConverted} not included in list rates"));
            }
        }

        private async Task<IEnumerable<RateModel>> GetRates()
        {
            IEnumerable<RateModel> rates = await service.TryGetRates();
            return duplicateRatesCleaner.DeletingDuplicates(rates);
        }

        private decimal ConvertAmountFromTo(decimal amount, RateModel rate) => ApplyBankRounding(amount * rate.Rate);

        private decimal ConvertAmountToFrom(decimal amount, RateModel rate) => ApplyBankRounding(amount / rate.Rate);

        private decimal ApplyBankRounding(decimal amount) => decimal.Round(amount, DECIMAL_PLACES);
    }
}

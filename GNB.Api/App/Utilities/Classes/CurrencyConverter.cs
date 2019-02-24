using GNB.Api.App.Models;
using GNB.Api.App.Services;
using GNB.Api.App.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GNB.Api.App.Utilities
{
    public class CurrencyConverter : ICurrencyConverter
    {
        private readonly IRateService<RateModel> service;
        private const int DECIMAL_PLACES = 8;

        public CurrencyConverter(IRateService<RateModel> service)
        {
            this.service = service;
        }

        public async Task<TransactionViewModel> ApplyConversion(TransactionViewModel transaction) =>
            Equals(transaction.Currency, transaction.CurrencyConverted)
                ? AssignSameAmountToTransaction(transaction)
                : await CalculateNewAmountToTransaction(transaction);

        private async Task<TransactionViewModel> CalculateNewAmountToTransaction(TransactionViewModel transaction) =>
            new TransactionViewModel
            {
                Sku = transaction.Sku,
                Currency = transaction.Currency,
                Amount = transaction.Amount,
                CurrencyConverted = transaction.CurrencyConverted,
                AmountConverted = await CalculateNewAmount(transaction)
            };

        private TransactionViewModel AssignSameAmountToTransaction(TransactionViewModel transaction) =>
            new TransactionViewModel
            {
                Sku = transaction.Sku,
                Currency = transaction.Currency,
                Amount = transaction.Amount,
                CurrencyConverted = transaction.CurrencyConverted,
                AmountConverted = transaction.AmountConverted
            };

        private async Task<decimal> CalculateNewAmount(TransactionViewModel transaction)
        {
            decimal currentAmount = transaction.Amount;
            string currentCurrency = transaction.Currency;

            bool newAmountCalculated = false;
            IEnumerable<RateModel> rates = await service.TryGetRates();

            while (!newAmountCalculated)
            {
                if (rates.Any(x => x.From == currentCurrency && x.To == transaction.CurrencyConverted))
                {
                    RateModel rate = rates.Single(x => x.From == currentCurrency && x.To == transaction.CurrencyConverted);
                    currentAmount = ConvertAmountFromTo(currentAmount, rate);
                    currentCurrency = rate.To;
                }
                else if (rates.Any(x => x.From == transaction.CurrencyConverted && x.To == currentCurrency))
                {
                    RateModel rate = rates.Single(x => x.From == transaction.CurrencyConverted && x.To == currentCurrency);
                    currentAmount = ConvertAmountToFrom(currentAmount, rate);
                    currentCurrency = rate.From;
                }
                else if (rates.Any(x => x.From == currentCurrency) || rates.Any(x => x.To == currentCurrency))
                {
                    if (rates.Any(x => x.From == currentCurrency))
                    {
                        RateModel rate = rates.First(x => x.From == currentCurrency);
                        rates = rates.Where(x => x.From == rate.From && x.To == rate.To).Where(x => x.From != rate.To && x.To != rate.From).ToList();
                        currentAmount = ConvertAmountFromTo(currentAmount, rate);
                        currentCurrency = rate.To;
                    }
                    else if (rates.Any(x => x.To == currentCurrency))
                    {
                        RateModel rate = rates.First(x => x.To == currentCurrency);
                        rates = rates.Where(x => !x.Equals(rate));
                        currentAmount = ConvertAmountToFrom(currentAmount, rate);
                        currentCurrency = rate.To;
                    }
                }
                else
                {
                    throw new ApplicationException($"The type currency conversion not is posible", new Exception($"The ${transaction.CurrencyConverted} not included in list rates"));
                }

                newAmountCalculated = Equals(currentCurrency, transaction.CurrencyConverted);
            }

            return currentAmount;
        }

        private decimal ConvertAmountFromTo(decimal amount, RateModel rate) => decimal.Round(amount * rate.Rate, DECIMAL_PLACES);

        private decimal ConvertAmountToFrom(decimal amount, RateModel rate) => decimal.Round(amount / rate.Rate, DECIMAL_PLACES);
    }
}

using GNB.Api.Models;
using GNB.Api.Services;
using GNB.Api.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GNB.Api.Utilities
{
    public class CurrencyConverter : ICurrencyConverter
    {
        private readonly IRateService<RateModel> service;
        private IEnumerable<RateModel> rates;

        public string CurrentCurrency { get; set; }

        public decimal CurrentAmount { get; set; }

        public string RequiredCurrency { get; set; }

        public decimal AmountConverted { get; private set; }

        public CurrencyConverter(IRateService<RateModel> service)
        {
            this.service = service;
        }

        public async Task ApplyConversion(CurrencyViewModel currency, string requiredCurrency)
        {
            CurrentCurrency = currency.Name;
            CurrentAmount = currency.Amount;
            RequiredCurrency = requiredCurrency;
            AmountConverted = await CalculateNewAmount();
        }

        private async Task<decimal> CalculateNewAmount()
        {
            decimal currentAmount = CurrentAmount;
            string currentCurrency = CurrentCurrency;

            bool newAmountCalculated = false;
            rates = await service.TryGetRates();

            while (!newAmountCalculated)
            {
                if (rates.Any(x => x.From == currentCurrency && x.To == RequiredCurrency))
                {
                    RateModel rate = rates.Single(x => x.From == currentCurrency && x.To == RequiredCurrency);
                    currentAmount = ConvertAmountFromTo(currentAmount, rate);
                    currentCurrency = rate.To;
                }
                else if (rates.Any(x => x.From == RequiredCurrency && x.To == currentCurrency))
                {
                    RateModel rate = rates.Single(x => x.From == RequiredCurrency && x.To == currentCurrency);
                    currentAmount = ConvertAmountToFrom(currentAmount, rate);
                    currentCurrency = rate.To;
                }
                else
                {
                    if (rates.Any(x => x.From == currentCurrency))
                    {
                        RateModel rate = rates.First(x => x.From == currentCurrency);
                        currentAmount = ConvertAmountFromTo(currentAmount, rate);
                        currentCurrency = rate.To;
                    }
                    else if (rates.Any(x => x.To == currentCurrency))
                    {
                        RateModel rate = rates.First(x => x.To == currentCurrency);
                        currentAmount = ConvertAmountToFrom(currentAmount, rate);
                        currentCurrency = rate.To;
                    }
                }

                newAmountCalculated = currentCurrency == RequiredCurrency;
            }

            return currentAmount;
        }

        private decimal ConvertAmountFromTo(decimal amount, RateModel rate) => amount * rate.Rate;

        private decimal ConvertAmountToFrom(decimal amount, RateModel rate) => amount / rate.Rate;
    }
}

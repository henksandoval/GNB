using GNB.Api.App.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GNB.Api.App.ViewModels
{
    public class TransactionViewModel
    {
        public string Sku { get; set; }

        public decimal Amount { get; set; }

        public string Currency { get; set; }

        public decimal AmountConverted { get; set; }

        public string CurrencyConverted { get; set; }

        public TransactionViewModel() { }

        public TransactionViewModel(TransactionModel model)
        {
            Sku = model.Sku;
            Amount = model.Amount;
            Currency = model.Currency;
        }

        public void UpdateAmountTransactionByChangeCurrency(string newCurrency)
        {
            if (Currency != newCurrency)
            {

            }
            else
            {
                AmountConverted = Amount;
                CurrencyConverted = Currency;
            }
        }

        public override bool Equals(object other)
        {
            if (!(other is TransactionViewModel toCompareWith))
                return false;
            return Sku == toCompareWith.Sku && Amount == toCompareWith.Amount && Currency == toCompareWith.Currency && AmountConverted == toCompareWith.AmountConverted && CurrencyConverted == toCompareWith.CurrencyConverted;
        }

        public override int GetHashCode() => HashCode.Combine(Sku, Amount, Currency, AmountConverted, CurrencyConverted);
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GNB.Api.ViewModels
{
    public class TransactionViewModel
    {
        public string Sku { get; set; }

        public decimal Amount { get; set; }

        public string Currency { get; set; }

        public decimal AmountConverted { get; set; }

        public string CurrencyConverted { get; set; }


        public override bool Equals(object other)
        {
            if (!(other is TransactionViewModel toCompareWith))
                return false;
            return Sku == toCompareWith.Sku && Amount == toCompareWith.Amount && Currency == toCompareWith.Currency && AmountConverted == toCompareWith.AmountConverted && CurrencyConverted == toCompareWith.CurrencyConverted;
        }

        public override int GetHashCode() => HashCode.Combine(Sku, Amount, Currency, AmountConverted, CurrencyConverted);
    }
}
